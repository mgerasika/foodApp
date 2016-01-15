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
        private Random _random = new Random(((int)DateTime.Now.Ticks));

        private PropousalManager() {
            
        }

        public List<ngHistoryEntry> MakePropousal(ngUserModel user, int dayOfWeek) {
            List<ngHistoryEntry> res = new List<ngHistoryEntry>();
            ngHistoryModel history = HistoryManager.Inst.GetHistoryModelByUser(user);
            if (null != history) {
                List<List<ngHistoryEntry>> groups = FindPossibleMenus(dayOfWeek, history);
                if (groups.Count > 0) {
                    groups = groups.OrderByDescending(g => g.Count).ToList();

                    int randomNumber = _random.Next(groups.Count);
                    res = groups[randomNumber];
                }
            }
            return res;
        }

        private List<List<ngHistoryEntry>> FindPossibleMenus(int dayOfWeek, ngHistoryModel history) {
            List<List<ngHistoryEntry>> groups = new List<List<ngHistoryEntry>>();
            List<ngHistoryGroupEntry> allHistoryByDate = history.GroupByDate(dayOfWeek);
            List<ngFoodItem> todayFoods = FoodManager.Inst.GetFoods(dayOfWeek);
            foreach (ngHistoryGroupEntry entry in allHistoryByDate)
            {
                if (HasFoodsInMenu(entry.Entries, todayFoods)) {
                    groups.Add(entry.Entries);
                }
            }
            return groups;
        }

        private bool HasFoodsInMenu(List<ngHistoryEntry> historyPerDay, List<ngFoodItem> todayFoods) {
            bool res = todayFoods.Count>0 && historyPerDay.Count>0;
            foreach (ngHistoryEntry entry in historyPerDay) {
                if (!HasFood(todayFoods, entry.FoodId)) {
                    res = false;
                    break;
                }
            }
            return res;
        }

        private static bool HasFood(List<ngFoodItem> todayFoods, string foodId) {
            bool res = false;
            foreach (ngFoodItem food in todayFoods) {
                if (ApiUtils.Equals(food.FoodId,foodId)) {
                    res = true;
                    break;
                }
            }
            return res;
        }
    }
}