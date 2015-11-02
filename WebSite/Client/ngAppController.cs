using angularjs;
using FoodApp.Common;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngAppController : ngControllerBase
    {
        public static ngAppController inst = new ngAppController();

        private ngAppController()
        {
        }

        public override string name {
            get { return "ngAppController"; }
        }

        public override string @namespace {
            get { return WebApiResources.@namespace; }
        }

        public int ngDayOfWeek {
            get { return _scope["ngDayOfWeek"].As<int>(); }
            set { _scope["ngDayOfWeek"] = value; }
        }

        public string ngUserId
        {
            get { return _scope["ngUserId"].As<string>(); }
            set { _scope["ngUserId"] = value; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);

            ngDayOfWeek = 1;
            this.ngUserId = "_cx0b9";
            HtmlContext.window.setTimeout(delegate() {
                eventManager.inst.fire(eventManager.settingsLoaded, ngDayOfWeek);
            },1);

        }

        public void changeDayOfWeek()
        {
            eventManager.inst.fire(eventManager.dayOfWeekChanged,ngDayOfWeek);
        }

        public void changeUserId()
        {
            eventManager.inst.fire(eventManager.dayOfWeekChanged, ngDayOfWeek);
        }
      
    }
}