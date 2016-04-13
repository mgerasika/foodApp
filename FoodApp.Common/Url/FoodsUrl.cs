using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class FoodsUrl : UrlBase {
        private const string c_sFoodsPrefix = "api/foods";
        public const string c_sAllFoods = c_sFoodsPrefix + "/";
        public const string c_sGetFoodsByDay = c_sFoodsPrefix + "/{userId}/{day}/";
        public const string c_sBuy = c_sFoodsPrefix + "/{userId}/{day}/{foodId}/{val}/";
        private const string c_sChangePricePrefix = c_sFoodsPrefix + "/changeprice/";
        public const string c_sChangePrice = c_sFoodsPrefix + "/changeprice/{userId}/{day}/{foodId}/{val}/";
        public static FoodsUrl Inst = new FoodsUrl();

        public string GetFoodsByDayUrl(string userId, int day) {
            string res = FormatUrl(c_sGetFoodsByDay, userId, day.As<string>());
            return res;
        }

        public string GetAllFoodsUrl(string userId) {
            string res = FormatUrl(c_sAllFoods, userId);
            return res;
        }

        public string GetBuyUrl(string userId, int day, string foodId, decimal value) {
            string res = FormatUrl(c_sBuy, userId, day.As<string>(), foodId, value.As<string>());
            return res;
        }

        public string GetChangePriceUrl(string userId, int day, string foodId, decimal value) {
            string res = FormatUrl(c_sChangePrice, userId, day.As<string>(), foodId, value.As<string>());
            return res;
        }
    }
}