using FoodApp.Properties;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs)]
    public class ngMoneyClientEntry {
        public bool canBuy { get; set; }
        public bool canRefund { get; set; }
        public decimal total { get; set; }
    }
}