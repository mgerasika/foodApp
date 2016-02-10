using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Common;
using FoodApp.Common.Managers;
using FoodApp.Common.Model;
using FoodApp.Common.Url;

namespace FoodApp.Controllers.api {
    public class PropousalController : ApiControllerBase
    {
        [HttpGet]
        [Route(PropousalUrl.c_sGetPropousalByDay)]
        public IList<ngHistoryEntry> GetPropousalByDay(string userId, int dayOfWeek) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            List<ngHistoryEntry> res = PropousalManager.Inst.MakePropousal(user, dayOfWeek);
            return res;
        }

        [HttpGet]
        [Route(PropousalUrl.c_sGetPropousals)]
        public List<List<ngHistoryEntry>> GetPropousals(string userId) {
            List<List<ngHistoryEntry>> res = new List<List<ngHistoryEntry>>();
            for (int i = 0; i < 5; i++) {
                ngUserModel user = UsersManager.Inst.GetUserById(userId);
                List<ngHistoryEntry> tmp = PropousalManager.Inst.MakePropousal(user, i);
                res.Add(tmp);
            }

            return res;
        }

        [HttpPost]
        [Route(PropousalUrl.c_sBuy)]
        public bool Buy(string userId, int dayOfWeek, List<ngHistoryEntry> entry) {
            foreach (ngHistoryEntry obj in entry) {
                ngUserModel user = UsersManager.Inst.GetUserById(userId);
                OrderManager.Inst.Buy(user, dayOfWeek, obj.FoodId, obj.Count);
            }

            return true;
        }
    }
}