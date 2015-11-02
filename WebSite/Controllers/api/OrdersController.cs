using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;
using GoogleAppsConsoleApplication;

namespace FoodApp.Controllers.api
{
    public class OrdersController : ApiController
    {
        [HttpGet]
        [Route("api/orders/{userId}/{day}")]
        public IList<ngOrderModel> GetOrders(string userId,int day)
        {
            List<ngOrderModel> items = OrderManager.Inst.GetOrders(userId,day);
            return items;
        }

        [HttpDelete]
        [Route("api/orders/{userId}/{day}/{id}")]
        public bool Delete(string userId,int day,string id) {
            OrderManager.Inst.Delete(userId,day,id);
            return true;
        }

       
    }
}
