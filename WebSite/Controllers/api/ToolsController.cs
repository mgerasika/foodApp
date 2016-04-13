using System.Web.Http;
using FoodApp.Common;
using FoodApp.Common.Managers;

namespace FoodApp.Controllers.api {
    public class ToolsController : ApiControllerBase {
        [HttpGet]
        [Route(ToolsUrl.c_sClearTodayOrders)]
        public bool ClearTodayOrders() {
            ClearOrdersManager.Inst.ClearTodayOrders();
            return true;
        }
    }
}