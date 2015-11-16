using angularjs;
using FoodApp.Common;
using FoodApp.Controllers.api;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngHistoryController : ngControllerBase
    {
        public static ngHistoryController inst = new ngHistoryController();

        private ngHistoryController()
        {
        }

        public override string className
        {
            get { return "ngHistoryController"; }
        }

        public JsArray<ngHistoryEntry> ngItems {
            get { return _scope["ngItems"].As<JsArray<ngHistoryEntry>>(); }
            set { _scope["ngItems"] = value; }
        }

       

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngItems = new JsArray<ngHistoryEntry>();

            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refresh(); });

            eventManager.inst.subscribe(eventManager.orderCompleted, delegate(int n) { refresh(); });
        }

        public ngFoodItem getFoodItem(string id)
        {
            ngFoodItem item = ngFoodController.inst.findItemById(id);
            return item;
        }

        public void refresh() {
            serviceHlp.inst.SendGet("json",
                 HistoryController.c_sHistory + "/" + ngAppController.inst.ngUserEmail + "/",
                delegate(object o, JsString s, jqXHR arg3) {
                    ngItems = o.As<JsArray<ngHistoryEntry>>();
                    _scope.apply();
                }, onRequestFailed);
        }

       
    }
}