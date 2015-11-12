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
        [Route(c_sOrdersPrefix + "/{userId}")]
        public IList<IList<ngOrderModel>> GetAllOrders(string userId) {
            List<IList<ngOrderModel>> res = new List<IList<ngOrderModel>>();
            res.Add(OrderManager.Inst.GetOrders(userId, 0));
            res.Add(OrderManager.Inst.GetOrders(userId, 1));
            res.Add(OrderManager.Inst.GetOrders(userId, 2));
            res.Add(OrderManager.Inst.GetOrders(userId, 3));
            res.Add(OrderManager.Inst.GetOrders(userId, 4));
            return res;
        }

        [HttpGet]
        [Route(c_sOrdersPrefix + "/{userId}/{day}")]
        public IList<ngOrderModel> GetOrders(string userId, int day) {
            List<ngOrderModel> items = OrderManager.Inst.GetOrders(userId, day);
            return items;
        }

        [HttpDelete]
        [Route(c_sOrdersPrefix + "/{userId}/{day}/{rowId}/")]
        public bool Delete(string userId, int day, string rowId) {
            OrderManager.Inst.Delete(userId, day, rowId);
            return true;
        }
    }
}