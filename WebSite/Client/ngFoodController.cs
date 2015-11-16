using angularjs;
using FoodApp.Common;
using FoodApp.Controllers.api;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngFoodController : ngControllerBase
    {
        public static ngFoodController inst = new ngFoodController();

        private ngFoodController() {
        }

        public override string className
        {
            get { return "ngFoodController"; }
        }

        public override string @namespace
        {
            get { return WebApiResources.@namespace; }
        }

        public JsArray<JsArray<ngFoodItem>> ngFoods
        {
            get { return _scope["ngFoods"].As<JsArray<JsArray<ngFoodItem>>>(); }
            set { _scope["ngFoods"] = value; }
        }

        public JsArray<string> ngCategories
        {
            get { return _scope["ngCategories"].As<JsArray<string>>(); }
            set { _scope["ngCategories"] = value; }
        }

        public void buyClick(int day, uint row) {
            serviceHlp.inst.SendPost("json",
                FoodsController.c_sFoodsPrefix + "/" + ngAppController.inst.ngUserEmail + "/" + day + "/" + row + "/",
                new JsObject(),
                delegate { ngOrderController.inst.refreshOrders(); }, onRequestFailed);
        }


        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngFoods = new JsArray<JsArray<ngFoodItem>>();
            ngCategories = new JsArray<string>();


            //eventManager.inst.subscribe(eventManager.dayOfWeekChanged, delegate(int n) { refresh(null); });
            //eventManager.inst.subscribe(eventManager.userIdChanged, delegate(int n) { refresh(null); });
            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) {
                refreshFoods(delegate {
                    JsFunction fn = HtmlContext.window.As<JsObject>()["initMenu"].As<JsFunction>();
                    fn.call();
                });
            });
        }

        public void refreshFoods(JsAction complete) {
            serviceHlp.inst.SendGet("json", FoodsController.c_sFoodsPrefix + "/",
                delegate(object o, JsString s, jqXHR arg3) {
                    ngFoods = o.As<JsArray<JsArray<ngFoodItem>>>();

                    foreach (ngFoodItem item in ngFoods[0]) {
                        if (!clientUtils.Inst.isEmpty(item.Category) &&
                            !clientUtils.Inst.Contains(ngCategories, item.Category)) {
                            ngCategories.push(item.Category);
                        }
                    }


                    _scope.apply();

                    if (null != complete) {
                        complete();
                    }
                }, onRequestFailed);
        }

        public void change() {
            refreshFoods(null);
        }

        internal ngFoodItem findItemById(string id) {
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
    }
}