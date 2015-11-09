using System;
using System.Collections.Generic;
using FoodApp.Client;
using SharpKit.JavaScript;

namespace FoodApp.Common
{
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngFoodRate : ngModelBase
    {
        public string FoodId { get; set; }
        public double Rate { get; set; }
    }

    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngUsersSettingsModel : ngModelBase
    {
        public List<ngFoodRate> FoodRates { get; set; }

        public void CrateFakeFoodRate()
        {
            if (null == this.FoodRates) {
                this.FoodRates = new List<ngFoodRate>();

                List<ngFoodItem> ngFoodItems = FoodManager.Inst.GetFoods(1);
                foreach (ngFoodItem foodItem in ngFoodItems) {
                    ngFoodRate rate = new ngFoodRate();
                    rate.FoodId = foodItem.RowId;
                    rate.Rate = 0.3;
                    this.FoodRates.Add(rate);
                }
            }
        }

        public string UserId { get; set; }

        public ngFoodRate GetFoodRateById(string foodId) {
            ngFoodRate res = null;
            foreach (ngFoodRate r in this.FoodRates) {
                if (r.FoodId.Equals(foodId)) {
                    res = r;
                    break;
                }
            }
            return res;
        }
    }
}