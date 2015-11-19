using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;

namespace FoodApp.Controllers.api {
    public class FoodsController : ApiController {
        public const string c_sFoodsPrefix = "api/foods";

        [HttpGet]
        [Route(c_sFoodsPrefix + "/")]
        public List<IList<ngFoodItem>> GetAllFoods() {
            List<IList<ngFoodItem>> allFoods = new List<IList<ngFoodItem>>();
            for (int i = 0; i < 5; ++i) {
                List<ngFoodItem> foods = FoodManager.Inst.GetFoods(i);
                allFoods.Add(foods);
            }

            return allFoods;
        }

        [HttpGet]
        [Route(c_sFoodsPrefix + "/{email}/{day}")]
        public IList<ngFoodItem> GetFoodsByDay(string email, int day) {
            List<ngFoodItem> items = FoodManager.Inst.GetFoods(day);
            return items;
        }

        [HttpPost]
        [Route(c_sFoodsPrefix + "/{email}/{day}/{foodId}/{val}/")]
        public bool Buy(string email, int day, string foodId, decimal val) {
            return OrderManager.Inst.Buy(email, day, foodId, val);
        }
    }
}