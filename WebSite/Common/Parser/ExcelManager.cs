using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Timers;
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

        public List<ExcelParser> Parsers {
            get { return _parsers; }
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
            _timer.Interval = 1*30*1000; //every 30 minutes
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
                            if (user.IsAdmin && user.Email.Contains("darwins")) {
                                ApiUtils.SendEmail(user, "Документ змінено", msg);
                            }
                        }
                    }
                }
            }
        }

        private bool CompareExcelDocuments(ExcelParser prevParser, ExcelParser newParser, out string msg) {
            bool hasChanges = false;
            StringBuilder sb = new StringBuilder();
            for (int day = 0; day < 5; ++day) {
                ExcelTable prevTable = prevParser.Doc.GetExcelTable(day);
                ExcelTable newTable = newParser.Doc.GetExcelTable(day);

                List<ngFoodItem> prevFoodsList = FoodManager.Inst.GetFoodsInternal(prevTable);
                List<ngFoodItem> newFoodList = FoodManager.Inst.GetFoodsInternal(newTable);

                foreach (ngFoodItem prevItem in prevFoodsList) {
                    ngFoodItem newItem = ApiUtils.GetFoodById(newFoodList, prevItem.FoodId);
                    if (null == newItem) {
                        hasChanges = true;
                        sb.AppendFormat("Видалено або перейменовано '{0}'\n", prevItem.Name);
                    }
                    else {
                        if (newItem.Name != null && !newItem.Name.Equals(prevItem.Name)) {
                            hasChanges = true;
                            sb.AppendFormat("Змінено назву '{0}' на '{1}'\n", prevItem.Name, newItem.Name);
                        }
                        if (newItem.Description != null && !newItem.Description.Equals(prevItem.Description)) {
                            hasChanges = true;
                            sb.AppendFormat("Змінено опис '{0}' '{1}' на '{2}'\n", prevItem.Name, prevItem.Description, newItem.Description);
                        }
                        if (!newItem.Price.Equals(prevItem.Price)) {
                            hasChanges = true;
                            sb.AppendFormat("Змінено ціну '{0}' '{1}' to '{2}'\n",prevItem.Name, prevItem.Price, newItem.Price);
                        }
                    }
                }

                foreach (ngFoodItem newItem in newFoodList) {
                    ngFoodItem prevItem = ApiUtils.GetFoodById(prevFoodsList, newItem.FoodId);
                    if (null == prevItem) {
                        hasChanges = true;
                        sb.AppendFormat("Додано або перейменовано'{0}'\n", newItem.Name);
                    }
                }
            }

            msg = sb.ToString();
            return hasChanges;
        }

        private ExcelParser GetParser() {
            ExcelParser res = null;
            lock (_parsers) {
                if (_parsers.Count > 0) {
                    res = _parsers[_parsers.Count - 1];
                }
            }

            return res;
        }
    }
}