using System;
using System.Collections.Generic;
using FoodApp.Client;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngMoneyModel : ngModelBase {
        public ngMoneyModel() {
            MoneyOrders = new List<ngMoneyOrderModel>();
        }

        public string UserId { get; set; }
        public decimal Total { get; set; }
        public List<ngMoneyOrderModel> MoneyOrders { get; set; }
    }

    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngMoneyOrderModel : ngModelBase {
        public EMoneyOperation Operation { get; set; }
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Value { get; set; }
    }
}