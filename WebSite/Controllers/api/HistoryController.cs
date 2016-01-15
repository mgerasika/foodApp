using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using FoodApp.Common;

namespace FoodApp.Controllers.api
{
    public class HistoryController : ApiController
    {
        public const string c_sHistory = "api/history";
        public const string c_sDeleteHistory = "api/history/delete";

        [HttpGet]
        [Route(c_sHistory + "/{userId}/")]
        public List<ngHistoryGroupEntry> GetHistory(string userId) {
            List<ngHistoryGroupEntry> res = new List<ngHistoryGroupEntry>();
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            ngHistoryModel model = HistoryManager.Inst.GetHistoryModelByUser(user);
            if (null != model)
            {
                res = model.GroupByDate();
            }
            return res;
        }

        [HttpPost]
        [Route(c_sDeleteHistory + "/{userId}/")]
        public bool DeleteHistory(string userId, ngHistoryGroupEntry group)
        {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            ngHistoryModel model = HistoryManager.Inst.GetHistoryModelByUser(user);
            DateTime dateTime = DateTime.Parse(@group.DateStr);
            List<ngHistoryEntry> entriesByDate = model.GetEntriesByDate(dateTime);
            foreach (ngHistoryEntry entry in entriesByDate) {
                bool res = model.Entries.Remove(entry);   
                Debug.Assert(res);
            }
            HistoryManager.Inst.Save();
            
            return true;
        }
    }
}