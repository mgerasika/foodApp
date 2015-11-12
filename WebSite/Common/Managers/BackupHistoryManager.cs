using System;
using System.Collections.Generic;
using System.Timers;
using FoodApp.Client;

namespace FoodApp.Common
{
    public class BackupHistoryManager
    {
        public static BackupHistoryManager Inst = new BackupHistoryManager();
        private readonly Timer _timer = new Timer();


        private BackupHistoryManager() {
            _timer = new Timer();
            _timer.Interval = 60*1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e) {
            //start sunday
            if (DateTime.Now.Hour == 13) {
                DateTime dt = DateTime.Now;
                if (!HistoryManager.Inst.HasAnyEntry(dt)) {
                    int dayOfWeek = (int) dt.DayOfWeek - 1;
                    CreateHistoryByDay(dayOfWeek);
                }
            }
        }

        private static void CreateHistoryByDay(int dayOfWeek) {
            bool hasChanges = false;
            foreach (ngUserModel user in UsersManager.Inst.GetUsers()) {
                List<ngOrderModel> orders = OrderManager.Inst.GetOrders(user.UserId, dayOfWeek);

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
            ngHistoryModel model = HistoryManager.Inst.GetHistoryModelByUserId(ngUser.UserId);
            if (null == model) {
                model = new ngHistoryModel();
                HistoryManager.Inst.AddItemAndSave(model);
                model.UserId = ngUser.UserId;
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