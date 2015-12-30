using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Client;

namespace FoodApp.Common {
    public class UserSettingsManager : ManagerBase<ngUsersSettingsModel> {
        public static UserSettingsManager Inst = new UserSettingsManager();

        protected override string FileName {
            get { return "userSettings.json"; }
        }

        protected override string GetId(ngUsersSettingsModel obj) {
            return obj.UserId;
        }

        public void Fix() {
            foreach (ngUsersSettingsModel item in GetItems()) {
                ngUserModel user = UsersManager.Inst.GetUserByEmail(item.Email);
                Debug.Assert(null != user);
                item.UserId = user.Id;
            }
            Save();
        }

        private void ParseHistoryAndMakeRateByUser(ngUserModel user) {
            ngUsersSettingsModel lSettings = Inst.EnsureUserSettings(user);
            lSettings.Fix();

            ngHistoryModel history = HistoryManager.Inst.GetHistoryModelByUser(user);
            if (null != history) {
                IDictionary<string, List<ngHistoryEntry>> groupFoods = history.GroupByFoodId();
                IDictionary<string, List<ngHistoryEntry>> groupDates = history.GroupByDate();
                int historyDaysCount = groupDates.Count;
                foreach (KeyValuePair<string, List<ngHistoryEntry>> food in groupFoods) {
                    int foodEntriesCount = food.Value.Count;
                    Debug.Assert(historyDaysCount >= foodEntriesCount);
                    double lRate = foodEntriesCount/(double) historyDaysCount;

                    string foodId = food.Key;
                    ngFoodItem ngFoodItem = FoodManager.Inst.GetFoodById(foodId);
                    if (!ngFoodItem.isContainer) {
                        ngFoodRate foodRate = lSettings.EnsureFoodRate(foodId);
                        foodRate.Rate = lRate;
                    }
                }
            }
        }

        internal ngUsersSettingsModel GetUserSettings(ngUserModel user) {
            return GetItem(user.Id);
        }

        internal ngUsersSettingsModel EnsureUserSettings(ngUserModel user) {
            ngUsersSettingsModel res = GetUserSettings(user);
            if (null == res) {
                res = new ngUsersSettingsModel();
                res.Email = user.Email;
                res.UserId = user.Id;
                Inst.Add(res);
            }
            return res;
        }

        internal void Init() {
            foreach (ngUserModel user in UsersManager.Inst.GetUsers()) {
                if (user.Email.Equals("mgerasika@gmail.com")) {
                    int x;
                }
                ParseHistoryAndMakeRateByUser(user);
            }
            Inst.Save();
        }
    }
}