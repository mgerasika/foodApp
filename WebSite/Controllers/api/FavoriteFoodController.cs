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
        [Route(c_sGetFavorite + "/{email}/")]
        public IList<ngFoodRate> GetFavorite(string email) {
            ngUsersSettingsModel settings = UserSettingsManager.Inst.GetUserSettings(email);
            if (null == settings) {
                settings = new ngUsersSettingsModel();
                settings.Email = email;

                UserSettingsManager.Inst.AddItemAndSave(settings);
            }
            List<ngFoodRate> favorite = settings.FoodRates;
            return favorite;
        }

        [HttpPost]
        [Route(c_sGetFavorite + "/{email}/{foodId}/{rate}/")]
        public bool ChangeRate(string email, string foodId, double rate) {
            ngUsersSettingsModel settings = UserSettingsManager.Inst.GetUserSettings(email);
            Debug.Assert(null != settings);
            ngFoodRate rateObj = settings.GetFoodRateById(foodId);
            Debug.Assert(null != rateObj);
            rateObj.Rate = rate;
            UserSettingsManager.Inst.Save();
            return true;
        }
    }
}