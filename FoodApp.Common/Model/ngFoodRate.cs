using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Json, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class ngFoodRate : ngModelBase
    {
        public string FoodId { get; set; }
        public double Rate { get; set; }
    }
}