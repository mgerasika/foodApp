using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using FoodApp.Common;

namespace FoodApp.Controllers.api
{
    public class FavoriteFoodController : ApiController
    {
        public const string c_sGetFavorite = "api/favorite";

        [HttpGet]
        [Route(c_sGetFavorite + "/{userId}/{day}")]
        public IList<ngFoodRate> GetFavorite(string userId, int day) {
            var settings = UserSettingsManager.Inst.GetItem(userId);
            if (null == settings) {
                settings = new ngUsersSettingsModel();
                settings.UserId = userId;

                UserSettingsManager.Inst.AddItem(settings);
            }
            var favorite = settings.FoodRates;
            if (null == favorite) {
                settings.CrateFakeFoodRate();
                favorite = settings.FoodRates;
            }

            return favorite;
        }

        [HttpPost]
        [Route(c_sGetFavorite + "/{userId}/{foodId}/{rate}")]
        public bool ChangeRate(string userId, string foodId, double rate) {
            var settings = UserSettingsManager.Inst.GetItem(userId);
            Debug.Assert(null != settings);
            var rateObj = settings.GetFoodRateById(foodId);
            Debug.Assert(null != rateObj);
            rateObj.Rate = rate;
            UserSettingsManager.Inst.Save();
            return true;
        }
    }
}