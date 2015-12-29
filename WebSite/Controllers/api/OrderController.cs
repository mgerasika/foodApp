using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;

namespace FoodApp.Controllers.api {
    public class OrderController : ApiController {
        public const string c_sOrdersPrefix = "api/orders";

        [HttpGet]
        [Route(c_sOrdersPrefix + "/{userId}/")]
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
        [Route(c_sOrdersPrefix + "/{userId}/{day}")]
        public IList<ngOrderEntry> GetOrders(string userId, int day) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            List<ngOrderEntry> items = OrderManager.Inst.GetOrders(user, day);
            return items;
        }

        [HttpDelete]
        [Route(c_sOrdersPrefix + "/{userId}/{day}/{foodId}/")]
        public bool Delete(string userId, int day, string foodId) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            OrderManager.Inst.Delete(user, day, foodId);
            return true;
        }
    }
}