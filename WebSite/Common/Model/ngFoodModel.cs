using FoodApp.Common;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngFoodItem : ngModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string FoodId { get; set; }
    }
}