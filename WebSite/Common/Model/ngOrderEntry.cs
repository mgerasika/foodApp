using FoodApp.Common;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngOrderEntry : ngModelBase
    {
        public string FoodId { get; set; }
        public decimal Count { get; set; }
        public decimal FoodPrice { get; set; }
    }
}