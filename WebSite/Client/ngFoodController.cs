using angularjs;
using FoodApp.Common;
using FoodApp.Controllers;
using FoodApp.Controllers.api;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngFoodController : ngControllerBase {
        public static ngFoodController inst = new ngFoodController();

        private ngFoodController() {
        }

        public override string className {
            get { return "ngFoodController"; }
        }

        public override string @namespace {
            get { return WebApiResources.@namespace; }
        }

        public JsArray<JsArray<ngFoodItem>> ngFoods {
            get { return _scope["ngFoods"].As<JsArray<JsArray<ngFoodItem>>>(); }
            set { _scope["ngFoods"] = value; }
        }

        public JsArray<string> ngCategories {
            get { return _scope["ngCategories"].As<JsArray<string>>(); }
            set { _scope["ngCategories"] = value; }
        }

        public void buyClick(int day, string foodId, decimal value) {
            clientUtils.Inst.showLoading();
            serviceHlp.inst.SendPost("json",
                FoodsController.c_sFoodsPrefix + "/" + ngAppController.inst.ngUserId + "/" + day + "/" + foodId + "/" +
                value + "/",
                new JsObject(),
                delegate {
                    clientUtils.Inst.hideLoading();
                    ngOrderController.inst.refreshOrders();
                }, onRequestFailed);
        }

        public bool hasOrder(int day, string foodId) {
            bool res = false;
            ngOrderEntry order = ngOrderController.inst.getOrderByFoodId(day, foodId);
            if (null != order) {
                res = true;
            }
            return res;
        }


        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngFoods = new JsArray<JsArray<ngFoodItem>>();
            ngCategories = new JsArray<string>();
            ngCategories.push(EFoodCategories.Salat);
            ngCategories.push(EFoodCategories.First);
            ngCategories.push(EFoodCategories.Garnir);
            ngCategories.push(EFoodCategories.MeatOrFish);
            ngCategories.push(EFoodCategories.ComplexDinner);
            ngCategories.push(EFoodCategories.Breat);

            //eventManager.inst.subscribe(eventManager.dayOfWeekChanged, delegate(int n) { refresh(null); });
            //eventManager.inst.subscribe(eventManager.userIdChanged, delegate(int n) { refresh(null); });
            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) {
                refreshFoods(delegate {
                    JsFunction fn = HtmlContext.window.As<JsObject>()["initMenu"].As<JsFunction>();
                    fn.call();
                });
            });

            eventManager.inst.subscribe(eventManager.orderListChanged, delegate(int n) { _scope.apply(); });
        }

        public string getPrefix(JsNumber price) {
            JsString res = price.As<JsString>();
            if (res.indexOf(".") > 0) {
                res = res.substr(0, res.indexOf("."));
            }
            return res;
        }

        public string getSuffix(JsNumber price) {
            JsString res = price.As<JsString>();
            if (res.indexOf(".") > 0) {
                res = res.substr(res.indexOf(".") + 1);
            }
            else {
                res = "";
            }
            return res;
        }

        public void refreshFoods(JsAction complete) {
            serviceHlp.inst.SendGet("json", FoodsController.c_sFoodsPrefix + "/",
                delegate(object o, JsString s, jqXHR arg3) {
                    ngFoods = o.As<JsArray<JsArray<ngFoodItem>>>();


                    _scope.apply();

                    if (null != complete) {
                        complete();
                    }
                }, onRequestFailed);
        }

        public void change() {
            refreshFoods(null);
        }

        internal ngFoodItem findFoodById(string id) {
            ngFoodItem res = null;

            foreach (JsArray<ngFoodItem> dayItems in ngFoods) {
                foreach (ngFoodItem item in dayItems) {
                    if (ApiUtils.CompareFoodIds(item.FoodId,id)) {
                        res = item;
                        break;
                    }
                }
                if (null != res) {
                    break;
                }
            }
            return res;
        }

        public decimal getOrderCount(int day, string foodId) {
            decimal res = 0;
            ngOrderEntry order = ngOrderController.inst.getOrderByFoodId(day, foodId);
            if (null != order) {
                res = order.Count;
            }
            return res;
        }
    }
}