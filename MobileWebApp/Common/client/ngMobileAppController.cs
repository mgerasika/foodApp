using angularjs;
using FoodApp.Common;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace MobileWebApp.Common.client {
    [JsType(JsMode.Prototype, Filename = MobileApiResources._fileClientTmpJs)]
    public class ngMobileAppController : ngMobileControllerBase {
        public static ngMobileAppController inst = new ngMobileAppController();

        private ngMobileAppController() {
        }


        public override string className {
            get { return "ngMobileAppController"; }
        }


        public string ngUserId {
            get { return _scope["ngUserId"].As<string>(); }
            set { _scope["ngUserId"] = value; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);

            ngUserId = HtmlContext.document.getElementById("userId").As<HtmlInputElement>().value;
            JsService.Inst.Init("http://www.gam-gam.lviv.ua/", ngUserId);

            HtmlContext.window.setTimeout(delegate { eventManager.inst.fire(eventManager.settingsLoaded, ""); }, 1);
        }
    }
}