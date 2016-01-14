using FoodApp.Common;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngFoodItem : ngModelBase {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string FoodId { get; set; }
        public bool IsByWeightItem { get; set; }
        public bool isContainer { get; set; }
        public bool isSmallContainer { get; set; }
        public bool isBigContainer { get; set; }
        public bool isSalat { get; set; }
        public bool isGarnir { get; set; }
        public bool isMeatOrFish { get; set; }
        public bool isFirst { get; set; }
        public bool isKvasolevaOrChanachi { get; set; }
    }
}