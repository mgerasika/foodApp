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
        [Route(c_sOrdersPrefix + "/{email}/")]
        public IList<IList<ngOrderModel>> GetAllOrders(string email) {
            List<IList<ngOrderModel>> res = new List<IList<ngOrderModel>>();
            res.Add(OrderManager.Inst.GetOrders(email, 0));
            res.Add(OrderManager.Inst.GetOrders(email, 1));
            res.Add(OrderManager.Inst.GetOrders(email, 2));
            res.Add(OrderManager.Inst.GetOrders(email, 3));
            res.Add(OrderManager.Inst.GetOrders(email, 4));
            return res;
        }

        [HttpGet]
        [Route(c_sOrdersPrefix + "/{email}/{day}")]
        public IList<ngOrderModel> GetOrders(string email, int day) {
            List<ngOrderModel> items = OrderManager.Inst.GetOrders(email, day);
            return items;
        }

        [HttpDelete]
        [Route(c_sOrdersPrefix + "/{email}/{day}/{foodId}/")]
        public bool Delete(string email, int day, string foodId) {
            OrderManager.Inst.Delete(email, day, foodId);
            return true;
        }
    }
}