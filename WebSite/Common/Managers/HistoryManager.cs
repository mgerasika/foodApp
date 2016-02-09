using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Client;
using FoodApp.Common.Model;

namespace FoodApp.Common.Managers {
    public class HistoryManager : ManagerBase<ngHistoryModel> {
        public static HistoryManager Inst = new HistoryManager();

        protected override string FileName {
            get { return "history.json"; }
        }

        protected override string GetId(ngHistoryModel obj) {
            return obj.UserId;
        }

        public void CreateFake() {
        }

        public void Fix() {
            //add user id
            List<ngHistoryModel> items = GetItems();
            foreach (ngHistoryModel item in items) {
                ngUserModel user = UsersManager.Inst.GetUserByEmail(item.Email);
                Debug.Assert(null != user);
                item.UserId = user.Id;
            }

            ngHistoryModel mgerasikaModel = GetHistoryModelByEmail("mgerasika@gmail.com");
            ngHistoryModel mherasikaDarwinsgrove = GetHistoryModelByEmail("mherasika@darwinsgrove.com");
            if (null != mgerasikaModel && mherasikaDarwinsgrove != null) {
                foreach (ngHistoryEntry entry in mgerasikaModel.Entries) {
                    mherasikaDarwinsgrove.Entries.Add(entry);
                }
                mgerasikaModel.Entries.Clear();
                items.Remove(mgerasikaModel);
                Save();
            }

            foreach (ngHistoryModel item in items) {
                ngUserModel user = UsersManager.Inst.GetUserByEmail(item.Email);
                Debug.Assert(null != user);
                item.UserId = user.Id;
            }

            //fix dublicate
            foreach (ngHistoryModel item in items) {
                List<ngHistoryEntry> entries = item.Entries;
                List<ngHistoryEntry> dublicates = new List<ngHistoryEntry>();
                for (int i = 0; i < entries.Count; i++) {
                    ngHistoryEntry entry = entries[i];
                    for (int j = i + 1; j < entries.Count; j++) {
                        ngHistoryEntry tmp = entries[j];
                        if (ApiUtils.Equals(entry.FoodId, tmp.FoodId) && ApiUtils.EqualDate(entry.Date, tmp.Date)) {
                            dublicates.Add(tmp);
                        }
                    }
                }

                if (dublicates.Count > 0) {
                    foreach (ngHistoryEntry dublicate in dublicates) {
                        item.Entries.Remove(dublicate);
                    }
                }
            }

            Save();
        }

        public void FixFoodIds() {
            List<ngHistoryModel> items = GetItems();
            List<ngFoodItem> allFoods = FoodManager.Inst.GetAllFoods();
            foreach (ngHistoryModel ngHistoryModel in items) {
                foreach (ngHistoryEntry entry in ngHistoryModel.Entries) {
                    foreach (ngFoodItem food in allFoods) {
                        if (ApiUtils.IsSeamsFoodIds(entry.FoodId, food.FoodId)) {
                            entry.FoodId = food.FoodId;
                            break;
                        }
                    }
                }
            }

            Save();
        }

        private ngHistoryModel GetHistoryModelByEmail(string email) {
            ngHistoryModel res = null;
            List<ngHistoryModel> items = GetItems();
            foreach (ngHistoryModel item in items) {
                if (item.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) {
                    res = item;
                    break;
                }
            }
            return res;
        }

        internal ngHistoryModel GetHistoryModelByUser(ngUserModel user) {
            return GetItem(user.Id);
        }

        internal bool HasAnyEntry(DateTime dt) {
            bool res = false;
            foreach (ngUserModel user in UsersManager.Inst.GetUsers()) {
                ngHistoryModel model = GetHistoryModelByUser(user);
                if (null != model) {
                    List<ngHistoryEntry> entries = model.GetEntriesByDate(dt);
                    if (entries.Count > 0) {
                        res = true;
                        break;
                    }
                }
            }
            return res;
        }
    }
}