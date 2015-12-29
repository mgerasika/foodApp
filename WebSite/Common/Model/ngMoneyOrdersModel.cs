using System;
using FoodApp.Client;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngMoneyLoggerModel : ngModelBase {
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string OrderId { get; set; }
        public EMoneyOperation Operation { get; set; }
        public decimal Value { get; set; }
    }
}