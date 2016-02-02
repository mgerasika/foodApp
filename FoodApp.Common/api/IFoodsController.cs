using System.Collections.Generic;
using FoodApp.Common.Model;

namespace FoodApp.Common.api {
    public interface  IFoodsController {
        List<IList<ngFoodItem>> GetAllFoods();

        IList<ngFoodItem> GetFoodsByDay(string userId, int day);

        bool Buy(string userId, int day, string foodId, decimal val);

        bool ChangePrice(string userId, int day, string foodId, decimal val);

    }
}