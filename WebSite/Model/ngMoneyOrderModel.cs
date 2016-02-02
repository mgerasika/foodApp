using System;
using SharpKit.JavaScript;

namespace FoodApp.Common.Model {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngMoneyOrderModel : ngModelBase {
        public EMoneyOperation Operation { get; set; }
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Value { get; set; }
    }
}