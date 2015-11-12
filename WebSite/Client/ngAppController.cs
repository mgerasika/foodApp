using angularjs;
using FoodApp.Common;
using FoodApp.Controllers;
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


        public override string className
        {
            get { return "ngAppController"; }
        }

        public override string @namespace {
            get { return WebApiResources.@namespace; }
        }

        public string ngUserId
        {
            get { return _scope["ngUserId"].As<string>(); }
            set { _scope["ngUserId"] = value; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);

            ngUserId = HtmlContext.document.getElementById("userId").As<HtmlInputElement>().value;
            HtmlContext.console.log(@HomeController.EmailQueryString + "=" + this.ngUserId);
            HtmlContext.window.setTimeout(delegate() {
                eventManager.inst.fire(eventManager.settingsLoaded,"");
            },1);

        }

       
       
      
    }
}