using angularjs;
using FoodApp.Common;
using FoodApp.Common.Model;
using FoodApp.Common.Url;
using FoodApp.Controllers.api;
using FoodApp.Properties;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngPropousalContoller : ngControllerBase {
        public static ngPropousalContoller inst = new ngPropousalContoller();

        private ngPropousalContoller() {
        }

        public override string className {
            get { return "ngPropousalContoller"; }
        }

        public JsArray<JsArray<ngHistoryEntry>> ngItems {
            get { return _scope["ngItems"].As<JsArray<JsArray<ngHistoryEntry>>>(); }
            set { _scope["ngItems"] = value; }
        }


        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngItems = new JsArray<JsArray<ngHistoryEntry>>();

            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { requestRefreshPropousals(null); });
        }

        public ngFoodItem getFoodItem(string id) {
            ngFoodItem item = ngFoodController.inst.findFoodById(id);
            return item;
        }

        private void requestRefreshPropousals(JsAction handler) {
            serviceHlp.inst.SendGet("json",
                PropousalUrl.c_sGetPropousal + "/" + ngAppController.inst.ngUserId + "/",
                delegate(object o, JsString s, jqXHR arg3) {
                    ngItems = o.As<JsArray<JsArray<ngHistoryEntry>>>();
                    _scope.apply();

                    if (null != handler) {
                        handler();
                    }
                }, onRequestFailed);
        }

        
        
        
        public void refreshPropousalsClick() {
        jsUtils.inst.showLoading();

            requestRefreshPropousals( delegate() {
                jsUtils.inst.hideLoading();
            });
        }

        public bool hasPropousal(int dayOfWeek) {
            JsArray<ngHistoryEntry> ngFoodRates = ngItems[dayOfWeek];
            return ngFoodRates != null && ngFoodRates.length > 0;
        }


        public void makePropousalClick(int dayOfWeek) {
            jsUtils.inst.showLoading();
            JsArray<ngHistoryEntry> ngFoodRates = ngItems[dayOfWeek];

            serviceHlp.inst.SendPost("json",
                PropousalUrl.c_sGetPropousal + "/" + ngAppController.inst.ngUserId + "/" + dayOfWeek + "/", JSON.stringify(ngFoodRates),
                delegate {
                    ngOrderController.inst.refreshOrders();
                    jsUtils.inst.hideLoading();
                }, onRequestFailed);
        }
    }
}