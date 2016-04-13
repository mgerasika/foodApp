using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Json, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class ngUserModel : ngModelBase {
        public uint Column { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

        public string Picture { get; set; }
        public string GoogleName { get; set; }
        public bool IsAdmin { get; set; }

        public string GoogleFirstName { get; set; }
    }
}