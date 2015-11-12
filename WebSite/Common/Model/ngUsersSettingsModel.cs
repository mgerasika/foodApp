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

        public ngUsersSettingsModel() {
            this.FoodRates = new List<ngFoodRate>();
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

        internal ngFoodRate EnsureFoodRate(string foodId)
        {
            ngFoodRate res = GetFoodRateById(foodId);
            if (null == res)
            {
                res = new ngFoodRate();
                res.FoodId = foodId;
                this.FoodRates.Add(res);
            }
            return res;
        }
    }
}