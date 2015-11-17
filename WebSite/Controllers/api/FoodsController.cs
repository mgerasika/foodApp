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
        [Route(c_sFoodsPrefix + "/{email}/{day}")]
        public IList<ngFoodItem> GetFoods(string email, int day)
        {
            List<ngFoodItem> items = FoodManager.Inst.GetFoods(day);
            return items;
        }

        [HttpPost]
        [Route(c_sFoodsPrefix + "/{email}/{day}/{foodId}/{val}/")]
        public bool Buy(string email, int day, string foodId, decimal val)
        {
            return OrderManager.Inst.Buy(email, day, foodId,val);
        }
    }
}