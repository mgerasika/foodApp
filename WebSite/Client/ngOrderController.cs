using angularjs;
using FoodApp.Common;
using FoodApp.Controllers.api;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngOrderController : ngControllerBase
    {
        public static ngOrderController inst = new ngOrderController();

        private ngOrderController() {
        }

        public override string className
        {
            get { return "ngOrderController"; }
        }

        public JsArray<JsArray<ngOrderModel>> ngOrders
        {
            get { return _scope["ngOrders"].As<JsArray<JsArray<ngOrderModel>>>(); }
            set { _scope["ngOrders"] = value; }
        }


        public ngFoodItem getFoodItem(string id) {
            ngFoodItem item = ngFoodController.inst.findFoodById(id);
            return item;
        }

        public decimal getTotalPrice(int day) {
            decimal res = 0;
            JsArray<ngOrderModel> ngOrderModels = ngOrders[day];
            if (ngOrderModels != null) {
                foreach (ngOrderModel item in ngOrderModels) {
                    ngFoodItem food = getFoodItem(item.FoodId);
                    if (null != food) {
                        res += clientUtils.Inst.fixNumber(food.Price*item.Count);
                        res = clientUtils.Inst.fixNumber(res);
                    }
                }
            }
            res = clientUtils.Inst.fixNumber(res);
            return res;
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngOrders = new JsArray<JsArray<ngOrderModel>>();

            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refreshOrders(); });
            eventManager.inst.subscribe(eventManager.orderCompleted, delegate(int n) { refreshOrders(); });
        }

        public void deleteOrder(int day, string row) {
            clientUtils.Inst.showLoading();
            serviceHlp.inst.SendDelete("json",
                OrdersController.c_sOrdersPrefix + "/" + ngAppController.inst.ngUserEmail + "/" + day + "/" + row + "/",
                new JsObject(), delegate {
                    clientUtils.Inst.hideLoading();
                    refreshOrders();
                }, onRequestFailed);
        }

        public void refreshOrders() {
            serviceHlp.inst.SendGet("json",
                OrdersController.c_sOrdersPrefix + "/" + ngAppController.inst.ngUserEmail + "/",
                delegate(object o, JsString s, jqXHR arg3) {
                    JsArray<JsArray<ngOrderModel>> tmp = o.As<JsArray<JsArray<ngOrderModel>>>();
                    while (ngOrders.length>0) {
                        ngOrders.pop();
                    }
                    foreach (JsArray<ngOrderModel> obj in tmp) {
                        ngOrders.push(obj);
                    }
                    _scope.apply();
                }, onRequestFailed);
        }
    }
}