using angularjs;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngTraceController : ngControllerBase {
        public static ngTraceController inst = new ngTraceController();
        private ngTraceController() {
        }

        public override string className {
            get { return "ngTraceController"; }
        }


        public ngTraceModel ngTraceModel {
            get { return _scope["ngTraceModel"].As<ngTraceModel>(); }
            set { _scope["ngTraceModel"] = value; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);

            requestGetTraces();
        }

        private void requestGetTraces() {
            jQuery.ajax(new AjaxSettings {
                type = "GET",
                dataType = "json",
                url = jsUtils.inst.getLocation() + "/api/trace",
                success = delegate(object o, JsString s, jqXHR arg3) {
                    ngTraceModel res = o.As<ngTraceModel>();
                    this.ngTraceModel = res;
                    _scope.apply();
                }
            });
        }

        public void onDeleteClick(string id) {
            jQuery.ajax(new AjaxSettings {
                type = "DELETE",
                dataType = "json",
                url = jsUtils.inst.getLocation() + "/api/trace/" + id,
                success = delegate(object o, JsString s, jqXHR arg3) {
                    ngTraceModel res = o.As<ngTraceModel>();
                    requestGetTraces();
                }
            });
        }

        public void onDeleteAllClick() {
            jQuery.ajax(new AjaxSettings
            {
                type = "DELETE",
                dataType = "json",
                url = jsUtils.inst.getLocation() + "/api/trace",
                success = delegate(object o, JsString s, jqXHR arg3) {
                    ngTraceModel res = o.As<ngTraceModel>();
                    requestGetTraces();
                }
            });
        }

    }
}