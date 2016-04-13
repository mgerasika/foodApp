using angularjs;
using FoodApp.Common;
using FoodApp.Controllers;
using FoodApp.Properties;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngAppController : ngControllerBase {
        public static ngAppController inst = new ngAppController();


        public override string className {
            get { return "ngAppController"; }
        }

        public override string @namespace {
            get { return WebApiResources.@namespace; }
        }

        public string ngUserId {
            get { return _scope["ngUserId"].As<string>(); }
            set { _scope["ngUserId"] = value; }
        }

        public int ngDayOfWeek {
            get { return _scope["ngDayOfWeek"].As<int>(); }
            set { _scope["ngDayOfWeek"] = value; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);

            ngDayOfWeek = 0;
            ngUserId = HtmlContext.document.getElementById("userId").As<HtmlInputElement>().value;
            JsService.Inst.Init(jsCommonUtils.inst.getLocation(), ngUserId);
            HtmlContext.console.log(@HomeController.UserIdQueryString + "=" + ngUserId);
            HtmlContext.window.setTimeout(delegate { eventManager.inst.fire(eventManager.settingsLoaded, ""); }, 1);

            
            eventManager.inst.subscribe(eventManager.dayChanged, delegate(int day) {
                HtmlContext.console.log("day changed " + day);
                ngDayOfWeek = day;
            });
        }

        private ngAppController() {
        }
    }
}