using angularjs;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngErrorController : ngControllerBase {
        public static ngErrorController inst = new ngErrorController();

        private ngErrorController() {
        }

        public override string className {
            get { return "ngErrorController"; }
        }


        public ngErrorModel ngErrorModel {
            get { return _scope["ngErrorModel"].As<ngErrorModel>(); }
            set { _scope["ngErrorModel"] = value; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);

            requestGetErrors();
        }

        private void requestGetErrors() {
            jQuery.ajax(new AjaxSettings {
                type = "GET",
                dataType = "json",
                url = jsUtils.inst.getLocation() + "/api/error",
                success = delegate(object o, JsString s, jqXHR arg3) {
                    ngErrorModel res = o.As<ngErrorModel>();
                    this.ngErrorModel = res;
                    _scope.apply();
                }
            });
        }

        public void onDeleteClick(string id) {
            jQuery.ajax(new AjaxSettings {
                type = "DELETE",
                dataType = "json",
                url = jsUtils.inst.getLocation() + "/api/error/" + id,
                success = delegate(object o, JsString s, jqXHR arg3) {
                    ngErrorModel res = o.As<ngErrorModel>();
                    requestGetErrors();
                }
            });
        }

        public void onDeleteAllClick() {
            jQuery.ajax(new AjaxSettings {
                type = "DELETE",
                dataType = "json",
                url = jsUtils.inst.getLocation() + "/api/error",
                success = delegate(object o, JsString s, jqXHR arg3) {
                    ngErrorModel res = o.As<ngErrorModel>();
                    requestGetErrors();
                }
            });
        }
    }
}