using angularjs;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public abstract class ngControllerBase : angularController
    {

        public override string className
        {
            get { return "ngControllerBase"; }
        }

        public override string @namespace
        {
            get { return WebApiResources.@namespace; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter)
        {
            base.init(scope, http, loc, filter);
        }

        protected ngControllerBase()
        {
        }

       

       

        protected void onRequestSuccess(object o, JsString s, jqXHR arg3)
        {
        }

        protected void onRequestFailed(JsError jsError, JsString jsString, jqXHR arg3)
        {
            jsUtils.inst.hideLoading();
        }
    }
}