using System;
using System.Collections.Generic;
using System.Linq;
using FoodApp.Client;
using FoodApp.Controllers;
using GoogleAppsConsoleApplication;

namespace FoodApp.Common {
    public class PropousalManager
    {
        public static PropousalManager Inst = new PropousalManager();

        private PropousalManager() {
            
        }

        public List<ngHistoryEntry> MakePropousal(ngUserModel user, int dayOfWeek) {
            List<ngHistoryEntry> res = new List<ngHistoryEntry>();
            ngHistoryModel history = HistoryManager.Inst.GetHistoryModelByUser(user);
            if (null != history) {
                List<List<ngHistoryEntry>> groups = FindPossibleMenus(dayOfWeek, history);
                if (groups.Count > 0) {
                    groups = groups.OrderByDescending(g => g.Count).ToList();

                    int randomNumber = (new Random()).Next(groups.Count-1);
                    res = groups[randomNumber];
                }
            }
            return res;
        }

        private List<List<ngHistoryEntry>> FindPossibleMenus(int dayOfWeek, ngHistoryModel history) {
            List<List<ngHistoryEntry>> groups = new List<List<ngHistoryEntry>>();
            IDictionary<string, List<ngHistoryEntry>> allHistoryByDate = history.GroupByDate();
            List<ngFoodItem> todayFoods = FoodManager.Inst.GetFoods(dayOfWeek);
            foreach (KeyValuePair<string, List<ngHistoryEntry>> entry in allHistoryByDate) {
                if (HasFoodsInMenu(entry.Value, todayFoods)) {
                    groups.Add(entry.Value);
                }
            }
            return groups;
        }

        private bool HasFoodsInMenu(List<ngHistoryEntry> historyPerDay, List<ngFoodItem> todayFoods) {
            bool res = todayFoods.Count>0 && historyPerDay.Count>0;
            foreach (ngHistoryEntry entry in historyPerDay) {
                if (HasFood(todayFoods, entry.FoodId)) {
                    res = false;
                    break;
                }
            }
            return res;
        }

        private static bool HasFood(List<ngFoodItem> todayFoods, string foodId) {
            bool res = false;
            foreach (ngFoodItem food in todayFoods) {
                if (food.FoodId.Equals(foodId)) {
                    res = true;
                    break;
                }
            }
            return res;
        }
    }
}