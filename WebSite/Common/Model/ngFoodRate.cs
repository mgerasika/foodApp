using FoodApp.Client;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngFoodRate : ngModelBase
    {
        public string FoodId { get; set; }
        public double Rate { get; set; }
    }
}