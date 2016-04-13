using angularjs;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngToolsController : ngControllerBase
    {
        public static ngToolsController inst = new ngToolsController();


        public override string className
        {
            get { return "ngToolsController"; }
        }

        public override string @namespace
        {
            get { return WebApiResources.@namespace; }
        }

        public void clearTodayOrdersClick() {
            jsUtils.inst.showLoading();

            JsService.Inst.ToolsApi.ClearTodayOrders(delegate(bool data) {
                jsUtils.inst.hideLoading();
            });
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter)
        {
            base.init(scope, http, loc, filter);
        }

        private ngToolsController()
        {
        }
    }
}