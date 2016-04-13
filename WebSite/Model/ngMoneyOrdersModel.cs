using System;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.JavaScript;

namespace FoodApp.Model {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngMoneyLoggerModel : ngModelBase {
        public string UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string OrderId { get; set; }
        public EMoneyOperation Operation { get; set; }
        public decimal Value { get; set; }
    }
}