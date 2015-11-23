using System;
using System.Collections.Generic;
using System.Timers;
using FoodApp.Client;

namespace FoodApp.Common {
    public class ClearOrdersManager {
        public static ClearOrdersManager Inst = new ClearOrdersManager();
        private readonly Timer _timer = new Timer();


        private ClearOrdersManager() {
            _timer = new Timer();
            _timer.Interval = 60*1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {
            //start sunday
            if (DateTime.Now.Hour == 14)
            {
                DateTime dt = DateTime.Now;
                int dayOfWeek = (int) dt.DayOfWeek - 1;
                if (OrderManager.Inst.HasOrders(dayOfWeek)) {
                    Dictionary<ngUserModel, List<ngOrderModel>> res = new Dictionary<ngUserModel, List<ngOrderModel>>();
                    List<ngUserModel> users = UsersManager.Inst.GetUniqueUsers();
                    foreach (ngUserModel user in users) {
                        List<ngOrderModel> orders = OrderManager.Inst.GetOrders(user.Email, dayOfWeek);
                        if (orders.Count > 0) {
                            res.Add(user, orders);
                            foreach (ngOrderModel order in orders) {
                                order.Count = 0;
                            }
                        }
                    }

                    BatchCellUpdater.Update(dayOfWeek, res);
                }
            }
        }

        private static void CreateHistoryByDay(int dayOfWeek) {
            bool hasChanges = false;
            foreach (ngUserModel user in UsersManager.Inst.GetUsers()) {
                List<ngOrderModel> orders = OrderManager.Inst.GetOrders(user.Email, dayOfWeek);

                if (orders.Count > 0) {
                    AddHistoryEntryToModel(user, orders, dayOfWeek);
                    hasChanges = true;
                }
            }
            if (hasChanges) {
                HistoryManager.Inst.Save();
            }
        }

        private static void AddHistoryEntryToModel(ngUserModel ngUser, List<ngOrderModel> orders, int dayOfWeek) {
            ngHistoryModel model = HistoryManager.Inst.GetHistoryModelByEmail(ngUser.Email);
            if (null == model) {
                model = new ngHistoryModel();
                HistoryManager.Inst.AddItemAndSave(model);
                model.Email = ngUser.Email;
                model.Entries = new List<ngHistoryEntry>();
            }

            foreach (ngOrderModel ngOrderModel in orders) {
                ngHistoryEntry entry = new ngHistoryEntry();
                entry.Date = DateTime.Now;
                entry.FoodId = ngOrderModel.FoodId;
                entry.Count = ngOrderModel.Count;
                ngFoodItem food = FoodManager.Inst.GetFoodById(dayOfWeek, ngOrderModel.FoodId);
                entry.FoodPrice = food.Price;
                model.Entries.Add(entry);
            }
        }
    }
}