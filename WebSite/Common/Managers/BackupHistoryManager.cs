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
            if (DateTime.Now.Hour == 12) {
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
                List<ngOrderEntry> orders = OrderManager.Inst.GetOrders(user, dayOfWeek);

                if (orders.Count > 0) {
                    AddHistoryEntryToModel(user, orders, dayOfWeek);
                    hasChanges = true;
                }
            }
            if (hasChanges) {
                HistoryManager.Inst.Save();
            }
        }

        private static void AddHistoryEntryToModel(ngUserModel ngUser, List<ngOrderEntry> orders, int dayOfWeek) {
            ngHistoryModel model = HistoryManager.Inst.GetHistoryModelByUser(ngUser);
            if (null == model) {
                model = new ngHistoryModel();
                HistoryManager.Inst.AddItemAndSave(model);
                model.Email = ngUser.Email;
                model.UserId = ngUser.Id;
                model.Entries = new List<ngHistoryEntry>();
            }

            foreach (ngOrderEntry ngOrderModel in orders) {
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