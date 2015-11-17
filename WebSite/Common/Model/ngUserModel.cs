using FoodApp.Client;
using SharpKit.JavaScript;

namespace FoodApp.Common
{
     [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs, Export = true)]
    public class ngUserModel : ngModelBase
    {
        public uint Column { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }

        public ngUserModel()
        {
        }


        public string Picture { get; set; }
        public string GoogleName { get; set; }
    }
}