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
        [Route(c_sGetFavorite + "/{userId}/")]
        public IList<ngFoodRate> GetFavorite(string userId)
        {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            ngUsersSettingsModel settings = UserSettingsManager.Inst.GetUserSettings(user);
            if (null == settings) {
                settings = new ngUsersSettingsModel();
                settings.Email = user.Email;
                settings.UserId = user.Id;

                UserSettingsManager.Inst.AddItemAndSave(settings);
            }
            List<ngFoodRate> favorite = settings.FoodRates;
            return favorite;
        }

        [HttpPost]
        [Route(c_sGetFavorite + "/{userId}/{foodId}/{rate}/")]
        public bool ChangeRate(string userId, string foodId, double rate)
        {
            ngUserModel user = UsersManager.Inst.GetUserById(userId);
            ngUsersSettingsModel settings = UserSettingsManager.Inst.GetUserSettings(user);
            Debug.Assert(null != settings);
            ngFoodRate rateObj = settings.GetFoodRateById(foodId);
            Debug.Assert(null != rateObj);
            rateObj.Rate = rate;
            UserSettingsManager.Inst.Save();
            return true;
        }
    }
}