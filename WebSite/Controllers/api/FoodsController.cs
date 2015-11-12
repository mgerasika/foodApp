using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;

namespace FoodApp.Controllers.api
{
    public class FoodsController : ApiController
    {
        public const string c_sFoodsPrefix = "api/foods";

        [HttpGet]
        [Route(c_sFoodsPrefix + "/")]
        public List<IList<ngFoodItem>> GetFoods()
        {
            List<IList<ngFoodItem>> allFoods = new List<IList<ngFoodItem>>();
            allFoods.Add(FoodManager.Inst.GetFoods(0));
            allFoods.Add(FoodManager.Inst.GetFoods(1));
            allFoods.Add(FoodManager.Inst.GetFoods(2));
            allFoods.Add(FoodManager.Inst.GetFoods(3));
            allFoods.Add(FoodManager.Inst.GetFoods(4));
            return allFoods;
        }

        [HttpGet]
        [Route(c_sFoodsPrefix + "/{userId}/{day}")]
        public IList<ngFoodItem> GetFoods(string userId, int day) {
            List<ngFoodItem> items = FoodManager.Inst.GetFoods(day);
            return items;
        }

        [HttpPost]
        [Route(c_sFoodsPrefix + "/{userId}/{day}/{id}/")]
        public bool Buy(string userId, int day, string id) {
            return OrderManager.Inst.Buy(userId, day, id);
        }
    }
}