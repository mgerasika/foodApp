using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using FoodApp.Common;

namespace FoodApp.Controllers.api
{
    public class HistoryController : ApiController
    {
        public const string c_sHistory = "api/history";

        [HttpGet]
        [Route(c_sHistory + "/{userId}/")]
        public IList<ngHistoryEntry> GetHistory(string userId) {
            List<ngHistoryEntry> res = new List<ngHistoryEntry>();
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            ngHistoryModel model = HistoryManager.Inst.GetHistoryModelByUser(user);
            if (null != model) {
                res = model.Entries;
            }
            return res;
        }
    }
}