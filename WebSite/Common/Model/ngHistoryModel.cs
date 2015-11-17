using System;
using System.Collections.Generic;
using System.Linq;
using FoodApp.Client;
using FoodApp.Controllers;
using SharpKit.JavaScript;

namespace FoodApp.Common
{
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngHistoryModel : ngModelBase
    {
        public ngHistoryModel() {
            Entries = new List<ngHistoryEntry>();
        }

        public string Email { get; set; }
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

        internal IDictionary<DateTime, List<ngHistoryEntry>> GroupByDate() {
            IDictionary<DateTime, List<ngHistoryEntry>> res = new Dictionary<DateTime, List<ngHistoryEntry>>();
            foreach (ngHistoryEntry entry in this.Entries) {
                if (!res.ContainsKey(entry.Date)) {
                     res[entry.Date] = new List<ngHistoryEntry>();
                }
                res[entry.Date].Add(entry);
            }
            return res;
        }

        internal IDictionary<string, List<ngHistoryEntry>> GroupByFoodId()
        {
            IDictionary<string, List<ngHistoryEntry>> res = new Dictionary<string, List<ngHistoryEntry>>();
            foreach (ngHistoryEntry entry in this.Entries)
            {
                if (!res.ContainsKey(entry.FoodId))
                {
                    res[entry.FoodId] = new List<ngHistoryEntry>();
                }
                res[entry.FoodId].Add(entry);
            }
            return res;
        }
    }
}