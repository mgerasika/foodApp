using SharpKit.JavaScript;

namespace FoodApp.Common.Url {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class FoodsUrl : UrlBase
    {
        public static FoodsUrl Inst = new FoodsUrl();
        public const string c_sFoodsPrefix = "api/foods";
        public const string c_sGetFoodsByDay = c_sFoodsPrefix + "/{userId}/{day}/";
        public const string c_sBuy = c_sFoodsPrefix + "/{userId}/{day}/{foodId}/{val}/";
        public const string c_sChangePricePrefix = c_sFoodsPrefix + "/changeprice/";
        public const string c_sChangePrice = c_sFoodsPrefix + "/changeprice/{userId}/{day}/{foodId}/{val}/";

        public string GetFoodsByDayUrl(string userId,int day) {
            string res = FormatUrl(c_sGetFoodsByDay,userId,day.As<string>());
            return res;
        }
    }
}