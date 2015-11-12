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
            ngFoodItem item = ngFoodController.inst.findItemById(id);
            return item;
        }

        public decimal getTotalPrice(int day) {
            decimal res = 0;
            JsArray<ngOrderModel> ngOrderModels = ngOrders[day];
            if (ngOrderModels != null) {
                foreach (ngOrderModel item in ngOrderModels) {
                    ngFoodItem food = getFoodItem(item.FoodId);
                    if (null != food) {
                        res += (food.Price*item.Count);
                    }
                }
            }
            return res;
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngOrders = new JsArray<JsArray<ngOrderModel>>();

            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refreshOrders(); });
            eventManager.inst.subscribe(eventManager.orderCompleted, delegate(int n) { refreshOrders(); });
        }

        public void deleteOrder(int day, string id) {
            serviceHlp.inst.SendDelete("json",
                OrdersController.c_sOrdersPrefix + "/" + ngAppController.inst.ngUserId + "/" + day + "/" + id + "/",
                new JsObject(), delegate { refreshOrders(); }, onRequestFailed);
        }

        public void refreshOrders() {
            serviceHlp.inst.SendGet("json",
                OrdersController.c_sOrdersPrefix + "/" + ngAppController.inst.ngUserId,
                delegate(object o, JsString s, jqXHR arg3) {
                    ngOrders = o.As<JsArray<JsArray<ngOrderModel>>>();
                    _scope.apply();
                }, onRequestFailed);
        }
    }
}