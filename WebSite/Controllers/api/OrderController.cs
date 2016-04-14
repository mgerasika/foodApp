using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;
using FoodApp.Common.Managers;

namespace FoodApp.Controllers.api {
    public class OrderController : ApiControllerBase, IOrderController {
        [HttpGet]
        [Route(OrderUrl.c_sGetAllOrders)]
        public IList<IList<ngOrderEntry>> GetAllOrders(string userId) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            List<IList<ngOrderEntry>> res = new List<IList<ngOrderEntry>>();
            res.Add(OrderManager.Inst.GetOrders(user, 0));
            res.Add(OrderManager.Inst.GetOrders(user, 1));
            res.Add(OrderManager.Inst.GetOrders(user, 2));
            res.Add(OrderManager.Inst.GetOrders(user, 3));
            res.Add(OrderManager.Inst.GetOrders(user, 4));
            return res;
        }

        [HttpGet]
        [Route(OrderUrl.c_sGetOrdersByDay)]
        public IList<ngOrderEntry> GetOrdersByDay(string userId, int day) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            List<ngOrderEntry> items = OrderManager.Inst.GetOrders(user, day);
            return items;
        }

        [HttpDelete]
        [Route(OrderUrl.c_sDeleteOrder)]
        public bool Delete(string userId, int day, string foodId) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            OrderManager.Inst.Delete(user, day, foodId);
            return true;
        }
    }
}