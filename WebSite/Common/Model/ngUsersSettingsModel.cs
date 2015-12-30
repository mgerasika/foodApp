using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Client;
using SharpKit.JavaScript;

namespace FoodApp.Common
{
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngUsersSettingsModel : ngModelBase
    {
        public List<ngFoodRate> FoodRates { get; set; }

        public ngUsersSettingsModel() {
            this.FoodRates = new List<ngFoodRate>();
        }
        public string Email { get; set; }
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

        internal void Fix()
        {
            List<ngFoodRate> ratesThatNeedRemove = new List<ngFoodRate>();
            foreach (ngFoodRate foodRate in FoodRates) {
                ngFoodItem food = FoodManager.Inst.GetFoodById(foodRate.FoodId);
                if (null == food) {
                    ratesThatNeedRemove.Add(foodRate);
                }
               else if (food.isContainer) {
                    ratesThatNeedRemove.Add(foodRate);
                }
            }

            foreach (ngFoodRate rateToRemove in ratesThatNeedRemove) {
                FoodRates.Remove(rateToRemove);
            }
        }
    }
}