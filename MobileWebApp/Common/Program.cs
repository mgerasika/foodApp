using FoodApp.Common;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace MobileWebApp.Common {
    [JsType(JsMode.Global, Filename = MobileApiResources._fileClientTmpJs, Export = true)]
    public static class Program {
        [JsMethod(Global = true)]
        public static void Main() {
            HtmlContext.window.onerror = delegate(ErrorEvent evt) { HtmlContext.alert("error:"+evt); };

            HtmlContext.window.setTimeout(delegate { eventManager.inst.fire(eventManager.deviceReady, ""); }, 1);
           
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