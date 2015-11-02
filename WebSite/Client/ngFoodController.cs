using angularjs;
using FoodApp.Common;
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

        public override string name {
            get { return "ngFoodController"; }
        }

        public override string @namespace {
            get { return WebApiResources.@namespace; }
        }

        public JsArray<ngFoodItem> ngItems {
            get { return _scope["ngItems"].As<JsArray<ngFoodItem>>(); }
            set { _scope["ngItems"] = value; }
        }

        public void buyClick(string rowId) {
            serviceHlp.inst.SendPost("json", getUrl() +"/" + ngAppController.inst.ngUserId + "/" + ngAppController.inst.ngDayOfWeek + "/" + rowId,
                new JsObject(),
                delegate { ngOrderController.inst.refresh(); }, onRequestFailed);
        }

        protected string getUrl() {
            return "api/foods";
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngItems = new JsArray<ngFoodItem>();

            eventManager.inst.subscribe(eventManager.dayOfWeekChanged, delegate(int n) { refresh(); });
            eventManager.inst.subscribe(eventManager.userIdChanged, delegate(int n) { refresh(); });
            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refresh(); });
        }

        public void refresh() {
            serviceHlp.inst.SendGet("json", getUrl() + "/" + ngAppController.inst.ngUserId + "/" + ngAppController.inst.ngDayOfWeek,
                delegate(object o, JsString s, jqXHR arg3) {
                    ngItems = o.As<JsArray<ngFoodItem>>();
                    _scope.apply();
                }, onRequestFailed);
        }

        public void change() {
            refresh();
        }

        internal ngFoodItem findItemById(string id) {
            ngFoodItem res = null;

            foreach (var item in ngItems) {
                if (item.FoodId == id) {
                    res = item;
                    break;
                }
            }
            return res;
        }
    }
}