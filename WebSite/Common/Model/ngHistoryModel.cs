using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Client;
using FoodApp.Controllers;
using SharpKit.JavaScript;

namespace FoodApp.Common {
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

        internal IDictionary<string, List<ngHistoryEntry>> GroupByDate()
        {
            IDictionary<string, List<ngHistoryEntry>> res = new Dictionary<string, List<ngHistoryEntry>>();
            foreach (ngHistoryEntry entry in Entries) {
                ngFoodItem food = FoodManager.Inst.GetFoodById(entry.FoodId);
                if (null != food && !food.isContainer) {
                    string dateTime = entry.Date.ToShortDateString();
                    if (!res.ContainsKey(dateTime)) {
                        res[dateTime] = new List<ngHistoryEntry>();
                    }
                    res[dateTime].Add(entry);
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

        internal IDictionary<string, List<ngHistoryEntry>> GroupByDate(int dayOfWeek)
        {
            IDictionary<string, List<ngHistoryEntry>> res = new Dictionary<string, List<ngHistoryEntry>>();
            foreach (ngHistoryEntry entry in Entries)
            {
                ngFoodItem food = FoodManager.Inst.GetFoodById(entry.FoodId);
                int ofWeek = (int)entry.Date.DayOfWeek;
                if (null != food && !food.isContainer && (ofWeek == dayOfWeek+1))
                {
                    string dateTime = entry.Date.ToShortDateString();
                    if (!res.ContainsKey(dateTime))
                    {
                        res[dateTime] = new List<ngHistoryEntry>();
                    }
                    res[dateTime].Add(entry);
                }
            }
            return res;
        }
    }
}