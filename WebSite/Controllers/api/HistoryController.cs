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
        [Route(c_sHistory + "/{email}/")]
        public IList<ngHistoryEntry> GetHistory(string email) {
            List<ngHistoryEntry> res = new List<ngHistoryEntry>();
            ngHistoryModel model = HistoryManager.Inst.GetHistoryModelByEmail(email);
            if (null != model) {
                res = model.Entries;
            }
            return res;
        }
    }
}