using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Timers;
using FoodApp.Client;
using FoodApp.Common.Managers;
using Google.GData.Spreadsheets;
using Timer = System.Timers.Timer;

namespace FoodApp.Common.Parser {
    public class ExcelManager {
        private static readonly object _lockObj = new object();
        public static ExcelManager Inst = new ExcelManager();
        private readonly List<ExcelParser> _parsers = new List<ExcelParser>();
        private readonly Timer _timer = new Timer();

        public SpreadsheetsService SpreadsheetsService {
            get { return GetParser().SpreadsheetsService; }
        }

        public ExcelDoc Doc {
            get { return GetParser().Doc; }
        }


        public void RefreshAccessToken() {
            lock (_parsers) {
                foreach (ExcelParser parser in _parsers) {
                    if (parser.IsInit) {
                        parser.RefreshAccessToken();
                    }
                }
            }
        }

        internal void Init() {
            if (GetParser() == null) {
                ExcelParser parser = new ExcelParser();
                parser.Init();
                lock (_parsers) {
                    _parsers.Add(parser);
                }
            }
        }

        private ExcelManager() {
            _timer = new Timer();
#if DEBUG
            _timer.Interval = 1*60*1000; //every 30 minutes
#else
                  _timer.Interval = 5*60*1000; //every 30 minutes
#endif
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {
            //start sunday
            CultureInfo info = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;
            if (DateTime.Now.Hour >= 7 || DateTime.Now.Hour <= 12) {
                ExcelParser newParser = new ExcelParser();
                newParser.Init();
                lock (_parsers) {
                    ExcelParser previous = GetParser();
                    _parsers.Add(newParser);

                    if (_parsers.Count > 2) {
                        _parsers.RemoveAt(0);
                    }
                    Debug.Assert(_parsers.Count == 2);


                    string msg = "";
                    if (CompareExcelDocuments(previous, newParser, out msg)) {
                        List<ngUserModel> users = UsersManager.Inst.GetUsers();
                        foreach (ngUserModel user in users) {
                            if (user.Email.Contains("darwins") || user.Email.Contains("stiystil"))
                            {
                                //ApiUtils.SendEmail(user, "Документ змінено", msg);
                            }
                        }
                    }

                    CompareOrders(previous, newParser);
                }
            }
        }

        private bool CompareExcelDocuments(ExcelParser prevParser, ExcelParser newParser, out string docMsg) {
            bool hasFoodChanges = false;
            StringBuilder sb = new StringBuilder();
            for (int day = 0; day < 5; ++day) {
                ExcelTable prevTable = prevParser.Doc.GetExcelTable(day);
                ExcelTable newTable = newParser.Doc.GetExcelTable(day);

                List<ngFoodItem> prevFoodsList = FoodManager.Inst.GetFoodsInternal(prevTable);
                List<ngFoodItem> newFoodList = FoodManager.Inst.GetFoodsInternal(newTable);

                foreach (ngFoodItem prevItem in prevFoodsList) {
                    ngFoodItem newItem = ApiUtils.GetFoodById(newFoodList, prevItem.FoodId);
                    if (null == newItem) {
                        hasFoodChanges = true;
                        sb.AppendFormat("Видалено '{0}'\n", prevItem.Name);
                    }
                    else {
                        if (newItem.Name != null && !newItem.Name.Equals(prevItem.Name)) {
                            hasFoodChanges = true;
                            sb.AppendFormat("Змінено назву '{0}' на '{1}'\n", prevItem.Name, newItem.Name);
                        }
                        if (newItem.Description != null && !newItem.Description.Equals(prevItem.Description)) {
                            hasFoodChanges = true;
                            sb.AppendFormat("Змінено опис '{0}' '{1}' на '{2}'\n", prevItem.Name, prevItem.Description, newItem.Description);
                        }
                        if (!newItem.Price.Equals(prevItem.Price)) {
                            hasFoodChanges = true;
                            sb.AppendFormat("Змінено ціну '{0}' '{1}' to '{2}'\n", prevItem.Name, prevItem.Price, newItem.Price);
                        }
                    }
                }

                foreach (ngFoodItem newItem in newFoodList) {
                    ngFoodItem prevItem = ApiUtils.GetFoodById(prevFoodsList, newItem.FoodId);
                    if (null == prevItem) {
                        hasFoodChanges = true;
                        sb.AppendFormat("Додано '{0}'\n", newItem.Name);
                    }
                }
            }

            docMsg = sb.ToString();
            return hasFoodChanges;
        }

        private void CompareOrders(ExcelParser prevParser, ExcelParser newParser) {
            int day = (int) DateTime.Now.DayOfWeek - 1;
            ExcelTable prevTable = prevParser.Doc.GetExcelTable(day);
            ExcelTable newTable = newParser.Doc.GetExcelTable(day);

            List<ngUserModel> users = GetUsersThatUseGamGam();
            foreach (ngUserModel user in users) {
                List<ngOrderEntry> prevOrders = OrderManager.Inst.GetOrdersInternal(user, prevTable);
                List<ngOrderEntry> newOrders = OrderManager.Inst.GetOrdersInternal(user, newTable);

                bool isChanged = false;
                foreach (ngOrderEntry prevOrderEntry in prevOrders) {
                    ngOrderEntry newOrderEntry = ApiUtils.GetOrderByFoodId(newOrders, prevOrderEntry.FoodId);
                    if (null == newOrderEntry || (null != newOrderEntry && newOrderEntry.Count != prevOrderEntry.Count)) {
                        isChanged = true;
                    }
                }

                if (isChanged) {
                    string prevOrdersMsg = getOrdersMessage(prevOrders, prevTable);
                    string newOrdersMsg = getOrdersMessage(newOrders, newTable);
                    StringBuilder sbUserMessage = new StringBuilder();
                    sbUserMessage.AppendFormat("Ваше замовлення змінено! Передивіться зміни.\n");
                    sbUserMessage.AppendFormat("Було:\n");
                    sbUserMessage.AppendFormat(prevOrdersMsg);
                    sbUserMessage.AppendFormat("\nЗмінено на:\n");
                    sbUserMessage.AppendFormat(newOrdersMsg);

                    ApiUtils.SendEmail(user, "Замовлення змінено.", sbUserMessage.ToString());
                }
            }
        }

        private string getOrdersMessage(List<ngOrderEntry> orders, ExcelTable tbl) {
            List<ngFoodItem> foods = FoodManager.Inst.GetFoodsInternal(tbl);

            StringBuilder res = new StringBuilder();
            foreach (ngOrderEntry order in orders) {
                ngFoodItem foodItem = ApiUtils.GetFoodById(foods, order.FoodId);
                Debug.Assert(null != foodItem);
                res.AppendFormat(" {0} кількість - {1}\n", foodItem.Name, order.Count);
            }
            return res.ToString();
        }

        private ExcelParser GetParser() {
            ExcelParser res = null;
            lock (_parsers) {
                if (_parsers.Count > 0) {
                    res = _parsers[_parsers.Count - 1];
                    Debug.Assert(null != res);
                }
            }

            return res;
        }

        private static List<ngUserModel> GetUsersThatUseGamGam() {
            List<ngUserModel> users = new List<ngUserModel>();
            List<ngTraceModel> traces = ApiTraceManager.Inst.GetTracesByDay(DateTime.Now);
            foreach (ngTraceModel model in traces) {
                ngUserModel user = UsersManager.Inst.GetUserByName(model.UserName);
                if (user != null && !users.Contains(user)) {
                    users.Add(user);
                }
            }
            return users;
        }
    }
}