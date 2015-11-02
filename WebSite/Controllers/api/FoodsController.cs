﻿using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;

namespace FoodApp.Controllers.api
{
    public class FoodsController : ApiController
    {
        public const string c_sFoodsPrefix = "api/foods";

        [HttpGet]
        [Route(c_sFoodsPrefix + "/{userId}/{day}")]
        public IList<ngFoodItem> GetFoods(string userId, int day) {
            var items = FoodManager.Inst.GetFoods(userId, day);
            return items;
        }

        [HttpPost]
        [Route(c_sFoodsPrefix + "/{userId}/{day}/{id}")]
        public bool Buy(string userId, int day, string id) {
            return OrderManager.Inst.Buy(userId, day, id);
        }
    }
}