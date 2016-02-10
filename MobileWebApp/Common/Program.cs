using FoodApp.Common.Model;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace FoodApp.Common.Url {
    [JsType(JsMode.Global, Filename = MobileApiResources._fileClientTmpJs, Export = true)]
    public static class Program {
        [JsMethod(Global = true)]
        public static void Main() {
            HtmlContext.window.onerror = delegate(ErrorEvent evt) { HtmlContext.alert("error:"+evt); };


            string userId = "47258346-5281-4fd7-9234-d5eb994c534c";
            JsService.Inst.Init("http://www.gam-gam.lviv.ua/", userId);

            JsService.Inst.FoodApi.GetFoods(0, delegate(JsArray<ngFoodItem> data) { HtmlContext.alert(data.length); });
        }

        [JsMethod(GlobalCode = true)]
        public static void GlobalCode() {
            bool isSharp = -1 != HtmlContext.window.location.href.indexOf("cshtml");
            if (isSharp) {
                Main();
            }
        }
    }
}