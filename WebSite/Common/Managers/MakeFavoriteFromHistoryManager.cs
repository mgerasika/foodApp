using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FoodApp.Client;
using FoodApp.Controllers.api;
using GoogleAppsConsoleApplication;

namespace FoodApp.Common
{
    public class MakeFavoriteFromHistoryManager 
    {
        public static MakeFavoriteFromHistoryManager Inst = new MakeFavoriteFromHistoryManager();
       
        public void ParseHistoryAndMakeRate() {
            foreach (ngUserModel user in UsersManager.Inst.GetUsers()) {
                ParseHistoryAndMakeRateByUserId(user);
            }
            UserSettingsManager.Inst.Save();
        }   
        
        private void ParseHistoryAndMakeRateByUserId(ngUserModel user) {
            ngUsersSettingsModel lSettings = UserSettingsManager.Inst.EnsureUserSettings(user.UserId);

            ngHistoryModel history = HistoryManager.Inst.GetHistoryModelByUserId(user.UserId);
            if (null != history) {
                IDictionary<string, List<ngHistoryEntry>> groupFoods = history.GroupByFoodId();

                IDictionary<DateTime, List<ngHistoryEntry>> groupDates = history.GroupByDate();
                int historyDaysCount = groupDates.Count;
                foreach (KeyValuePair<string, List<ngHistoryEntry>> food in groupFoods) {
                    int foodEntriesCount = food.Value.Count;
                    Debug.Assert(historyDaysCount >= foodEntriesCount);
                    double lRate = foodEntriesCount/(double)historyDaysCount;

                    string foodId = food.Key;
                    ngFoodRate foodRate = lSettings.EnsureFoodRate(foodId);
                    foodRate.Rate = lRate;
                }
            }

        }
    }
}