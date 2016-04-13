using FoodApp.Properties;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs)]
    public class ngAddMoneyModel
    {
        public string userId { get; set; }
        public decimal value { get; set; }
        public decimal total { get; set; }
    }
}