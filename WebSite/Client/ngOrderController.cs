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

        public override string name {
            get { return "ngOrderController"; }
        }

        public ngOrderModel ngOrderModel {
            get { return _scope["ngOrderModel"].As<ngOrderModel>(); }
            set { _scope["ngOrderModel"] = value; }
        }

        public JsArray<ngOrderModel> ngItems {
            get { return _scope["ngItems"].As<JsArray<ngOrderModel>>(); }
            set { _scope["ngItems"] = value; }
        }

        protected string getUrl() {
            return OrdersController.c_sOrdersPrefix;
        }

        public ngFoodItem getFoodItem(string id) {
            var item = ngFoodController.inst.findItemById(id);
            return item;
        }

        public decimal getTotalPrice() {
            decimal res = 0;
            foreach (ngOrderModel item in ngItems) {
                ngFoodItem food = getFoodItem(item.FoodId);
                if (null != food) {
                    res += (food.Price*item.Count);
                }
            }
            return res;
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngOrderModel = new ngOrderModel();
            ngItems = new JsArray<ngOrderModel>();

            eventManager.inst.subscribe(eventManager.dayOfWeekChanged, delegate(int n) { refresh(); });
            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refresh(); });
        }

        public void deleteOrder(string id) {
            serviceHlp.inst.SendDelete("json",
                getUrl() + "/" + ngAppController.inst.ngUserId + "/" + ngAppController.inst.ngDayOfWeek + "/" + id,
                new JsObject(), delegate { refresh(); }, onRequestFailed);
        }

        public void completeOrder()
        {
            serviceHlp.inst.SendPost("json",
                getUrl() + "/" + ngAppController.inst.ngUserId + "/" + ngAppController.inst.ngDayOfWeek ,
                new JsObject(), delegate { refresh(); }, onRequestFailed);
        }

        public void refresh() {
            serviceHlp.inst.SendGet("json",
                getUrl() + "/" + ngAppController.inst.ngUserId + "/" + ngAppController.inst.ngDayOfWeek,
                delegate(object o, JsString s, jqXHR arg3) {
                    ngItems = o.As<JsArray<ngOrderModel>>();
                    _scope.apply();
                }, onRequestFailed);
        }
    }
}