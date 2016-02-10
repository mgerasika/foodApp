using SharpKit.JavaScript;

namespace FoodApp.Common.Url {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class OrderUrl : UrlBase
    {
        public const string c_sOrdersPrefix = "api/orders";
        public const string c_sGetAllOrders = c_sOrdersPrefix + "/{userId}/";
        public const string c_sGetOrders = c_sOrdersPrefix + "/{userId}/{day}";
        public const string c_sDeleteOrder = c_sOrdersPrefix + "/{userId}/{day}/{foodId}/";
    }
}