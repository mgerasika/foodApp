using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using FoodApp.Common;

namespace FoodApp.Controllers.api
{
    public class PropousalController : ApiController
    {
        public const string c_sGetPropousal = "api/propousal";

        [HttpGet]
        [Route(c_sGetPropousal + "/{userId}/{dayOfWeek}/")]
        public IList<ngHistoryEntry> GetPropousalByDay(string userId, int dayOfWeek)
        {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            List<ngHistoryEntry> res = PropousalManager.Inst.MakePropousal(user,dayOfWeek);
            return res;
        }

        [HttpGet]
        [Route(c_sGetPropousal + "/{userId}/")]
        public List<List<ngHistoryEntry>> GetPropousals(string userId)
        {
            List<List<ngHistoryEntry>> res = new List<List<ngHistoryEntry>>();
            for (int i = 0; i < 5; i++) {
                ngUserModel user = UsersManager.Inst.GetUserById(userId);
                List<ngHistoryEntry> tmp = PropousalManager.Inst.MakePropousal(user, i);
                res.Add(tmp);
            }

            return res;
        }
    }
}