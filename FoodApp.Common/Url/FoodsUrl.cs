namespace FoodApp.Common.Url {
    public class FoodsUrl {
        public const string c_sFoodsPrefix = "api/foods";
        public const string c_sGetFoodsByDay = c_sFoodsPrefix + "/{userId}/{day}";
        public const string c_sBuy = c_sFoodsPrefix + "/{userId}/{day}/{foodId}/{val}/";
        public const string c_sChangePricePrefix = c_sFoodsPrefix + "/changeprice";
        public const string c_sChangePrice = c_sFoodsPrefix + "/changeprice/{userId}/{day}/{foodId}/{val}";
    }
}