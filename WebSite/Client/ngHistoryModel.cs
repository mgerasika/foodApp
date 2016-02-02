using System;
using System.Collections.Generic;
using FoodApp.Controllers;
using SharpKit.JavaScript;

namespace FoodApp.Common.Model {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngHistoryModel : ngModelBase {
        public ngHistoryModel() {
            Entries = new List<ngHistoryEntry>();
        }

        public string Email { get; set; }
        public string UserId { get; set; }
        public List<ngHistoryEntry> Entries { get; set; }

        internal List<ngHistoryEntry> GetEntriesByDate(DateTime dt) {
            List<ngHistoryEntry> res = new List<ngHistoryEntry>();
            foreach (ngHistoryEntry entry in Entries) {
                if (ApiUtils.EqualDate(entry.Date, dt)) {
                    res.Add(entry);
                }
            }
            return res;
        }

        internal List<ngHistoryGroupEntry> GroupByDate(int dayOfWeek)
        {
            List<ngHistoryGroupEntry> res = new List<ngHistoryGroupEntry>();
            foreach (ngHistoryEntry entry in Entries)
            {
                ngFoodItem food = FoodManager.Inst.GetFoodById(entry.FoodId);
                int ofWeek = (int)entry.Date.DayOfWeek;
                if (null != food && !food.isContainer && (ofWeek == dayOfWeek + 1))
                {
                    string key = entry.Date.ToShortDateString();
                    if (!HasGroupByDate(res, key))
                    {
                        ngHistoryGroupEntry newGroup = new ngHistoryGroupEntry();
                        newGroup.DateStr = key;
                        res.Add(newGroup);
                    }
                    GetGroupByDate(res, key).Entries.Add(entry);
                }
            }
            return res;
        }

        internal List<ngHistoryGroupEntry> GroupByDate() {
            List<ngHistoryGroupEntry> res = new List<ngHistoryGroupEntry>();
            foreach (ngHistoryEntry entry in Entries) {
                ngFoodItem food = FoodManager.Inst.GetFoodById(entry.FoodId);
                if (null != food && !food.isContainer) {
                    string key = entry.Date.ToShortDateString();
                    if (!HasGroupByDate(res,key)) {
                        ngHistoryGroupEntry newGroup = new ngHistoryGroupEntry();
                        newGroup.DateStr = key;
                        res.Add(newGroup);
                    }
                    GetGroupByDate(res,key).Entries.Add(entry);
                }
            }
            return res;
        }

        private bool HasGroupByDate(List<ngHistoryGroupEntry> items, string dateStr) {
            bool res = false;
            foreach (ngHistoryGroupEntry entry in items) {
                if (entry.DateStr.Equals(dateStr, StringComparison.OrdinalIgnoreCase)) {
                    res = true;
                    break;
                }
            }
            return res;
        }

        private ngHistoryGroupEntry GetGroupByDate(List<ngHistoryGroupEntry> items, string dateStr)
        {
            ngHistoryGroupEntry res = null;
            foreach (ngHistoryGroupEntry entry in items)
            {
                if (entry.DateStr.Equals(dateStr, StringComparison.OrdinalIgnoreCase))
                {
                    res = entry;
                    break;
                }
            }
            return res;
        }

       
        internal IDictionary<string, List<ngHistoryEntry>> GroupByFoodId() {
            IDictionary<string, List<ngHistoryEntry>> res = new Dictionary<string, List<ngHistoryEntry>>();
            foreach (ngHistoryEntry entry in Entries) {
                ngFoodItem food = FoodManager.Inst.GetFoodById(entry.FoodId);
                if (null != food && !food.isContainer) {
                    if (!res.ContainsKey(entry.FoodId)) {
                        res[entry.FoodId] = new List<ngHistoryEntry>();
                    }
                    res[entry.FoodId].Add(entry);
                }
            }
            return res;
        }

       
    }
}