using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;
using GoogleAppsConsoleApplication;

namespace FoodApp.Controllers.api
{
    
    
    public class FoodsController : ApiController
    {
        [HttpGet]
        [Route("api/foods/{userId}/{day}")]
        public IList<ngFoodItem> GetFoods(string userId,int day)
        {
            List<ngFoodItem> items = FoodManager.Inst.GetFoods(userId,day);
            return items;
        }

        [HttpPost]  
        [Route("api/foods/{userId}/{day}/{id}")]
        public bool Buy(string userId,int day,string id) {
            return OrderManager.Inst.Buy(userId,day,id);
        }

        
    }
}