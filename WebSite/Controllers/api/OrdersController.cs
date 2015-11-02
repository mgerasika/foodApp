using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;

namespace FoodApp.Controllers.api
{
    public class OrdersController : ApiController
    {
        public const string c_sOrdersPrefix = "api/orders";

        [HttpGet]
        [Route(c_sOrdersPrefix + "/{userId}/{day}")]
        public IList<ngOrderModel> GetOrders(string userId, int day) {
            var items = OrderManager.Inst.GetOrders(userId, day);
            return items;
        }

        [HttpDelete]
        [Route(c_sOrdersPrefix + "/{userId}/{day}/{id}")]
        public bool Delete(string userId, int day, string id) {
            OrderManager.Inst.Delete(userId, day, id);
            return true;
        }
    }
}