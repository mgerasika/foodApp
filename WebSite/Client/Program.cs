using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Global, Filename = WebApiResources._fileClientJs)]
    public static class Program {
        [JsMethod(Global = true)]
        public static void onTabChanged(int day) {
            eventManager.inst.fire(eventManager.dayChanged, day);
        }
    }
}