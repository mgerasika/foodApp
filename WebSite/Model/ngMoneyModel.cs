using System.Collections.Generic;
using SharpKit.JavaScript;

namespace FoodApp.Common.Model {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngMoneyModel : ngModelBase {
        public ngMoneyModel() {
            MoneyOrders = new List<ngMoneyOrderModel>();
        }

        public string UserId { get; set; }
        public decimal Total { get; set; }
        public List<ngMoneyOrderModel> MoneyOrders { get; set; }
    }
}