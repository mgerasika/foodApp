using angularjs;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace MobileWebApp.Common.client {
    [JsType(JsMode.Prototype, Filename = MobileApiResources._fileClientTmpJs)]
    public abstract class ngMobileControllerBase : angularController {
        public override string className {
            get { return "ngMobileControllerBase"; }
        }

        public override string @namespace {
            get { return MobileApiResources.@namespace; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
        }


        protected void onRequestSuccess(object o, JsString s, jqXHR arg3) {
        }

        protected void onRequestFailed(JsError jsError, JsString jsString, jqXHR arg3) {
        }
    }
}