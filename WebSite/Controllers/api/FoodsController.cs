using System.Collections.Generic;
using System.Web.Http;
using FoodApp.Common;
using FoodApp.Common.Managers;

namespace FoodApp.Controllers.api {
    public class FoodsController : ApiControllerBase, IFoodsController {
        [HttpGet]
        [Route(FoodsUrl.c_sAllFoods)]
        public List<IList<ngFoodItem>> GetAllFoods() {
            List<IList<ngFoodItem>> allFoods = new List<IList<ngFoodItem>>();
            for (int i = 0; i < 5; ++i) {
                List<ngFoodItem> foods = FoodManager.Inst.GetFoods(i);
                allFoods.Add(foods);
            }

            return allFoods;
        }

        [HttpGet]
        [Route(FoodsUrl.c_sGetFoodsByDay)]
        public IList<ngFoodItem> GetFoodsByDay(string userId, int day) {
            List<ngFoodItem> items = FoodManager.Inst.GetFoods(day);
            return items;
        }

        [HttpPost]
        [Route(FoodsUrl.c_sBuy)]
        public bool Buy(string userId, int day, string foodId, decimal val) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            return OrderManager.Inst.Buy(user, day, foodId, val);
        }

        [HttpPost]
        [Route(FoodsUrl.c_sChangePrice)]
        public bool ChangePrice(string userId, int day, string foodId, decimal val) {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            return FoodManager.Inst.ChangePrice(user, day, foodId, val);
        }
    }
}