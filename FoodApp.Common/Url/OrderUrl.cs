using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class OrderUrl : UrlBase {
        private const string c_sOrdersPrefix = "api/orders";
        public const string c_sGetAllOrders = c_sOrdersPrefix + "/{userId}/";
        public const string c_sGetOrdersByDay = c_sOrdersPrefix + "/{userId}/{day}";
        public const string c_sDeleteOrder = c_sOrdersPrefix + "/{userId}/{day}/{foodId}/";
        public static OrderUrl Inst = new OrderUrl();

        protected OrderUrl() {
        }

        public string GetAllOrdersUrl(string ngUserId) {
            return FormatUrl(c_sGetAllOrders, ngUserId);
        }

        public string GetOrdersByDayUrl(string ngUserId,int day)
        {
            return FormatUrl(c_sGetOrdersByDay, ngUserId,day.As<string>());
        }

        public string GetDeleteOrderUrl(string ngUserId, int day, string foodId) {
            return FormatUrl(c_sDeleteOrder, ngUserId, day.As<string>(), foodId);
        }
    }
}