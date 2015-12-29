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
        [Route(c_sFoodsPrefix + "/{userId}/{day}")]
        public IList<ngFoodItem> GetFoodsByDay(string userId, int day) {
            List<ngFoodItem> items = FoodManager.Inst.GetFoods(day);
            return items;
        }

        [HttpPost]
        [Route(c_sFoodsPrefix + "/{userId}/{day}/{foodId}/{val}/")]
        public bool Buy(string userId, int day, string foodId, decimal val) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            return OrderManager.Inst.Buy(user, day, foodId, val);
        }
    }
}