using angularjs;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.Html;
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
            jsUtils.inst.showLoading();

            JsService.Inst.FoodApi.Buy( day, foodId, value, delegate(bool res) {
                jsUtils.inst.hideLoading();
                ngOrderController.inst.refreshOrders();

                if (!res) {
                    showErrorPopup();
                }
            });
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
            ngCategories.push(CategoryNames.Salat);
            ngCategories.push(CategoryNames.First);
            ngCategories.push(CategoryNames.Garnir);
            ngCategories.push(CategoryNames.MeatOrFish);
            ngCategories.push(CategoryNames.ComplexDinner);
            ngCategories.push(CategoryNames.Breat);

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
            JsService.Inst.FoodApi.GetAllFoods(delegate(JsArray<JsArray<ngFoodItem>> data) {
                ngFoods = data;
                _scope.apply();

                if (null != complete) {
                    complete();
                }
            });
        }


        internal ngFoodItem findFoodById(string id) {
            ngFoodItem res = null;

            foreach (JsArray<ngFoodItem> dayItems in ngFoods) {
                foreach (ngFoodItem item in dayItems) {
                    if (item.FoodId == id) {
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

        public void changePrice(int day, ngFoodItem ngFoodItem) {
            jsUtils.inst.showLoading();
            JsService.Inst.FoodApi.ChangePrice( day, ngFoodItem.FoodId, ngFoodItem.Price, delegate {
                jsUtils.inst.hideLoading();
                refreshFoods(delegate { });
            });
        }
    }
}