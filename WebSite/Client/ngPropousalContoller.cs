using angularjs;
using FoodApp.Common;
using FoodApp.Controllers.api;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngPropousalContoller : ngControllerBase
    {
        public static ngPropousalContoller inst = new ngPropousalContoller();

        private ngPropousalContoller()
        {
        }

        public override string className
        {
            get { return "ngPropousalContoller"; }
        }

        public JsArray<JsArray<ngFoodRate>> ngItems {
            get { return _scope["ngItems"].As<JsArray<JsArray<ngFoodRate>>>(); }
            set { _scope["ngItems"] = value; }
        }

      

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngItems = new JsArray<JsArray<ngFoodRate>>();

            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refreshPropousals(); });
        }

        public ngFoodItem getFoodItem(string id)
        {
            ngFoodItem item = ngFoodController.inst.findFoodById(id);
            return item;
        }

        public void refreshPropousals() {
            serviceHlp.inst.SendGet("json",
                PropousalController.c_sGetPropousal + "/" + ngAppController.inst.ngUserId + "/" ,
                delegate(object o, JsString s, jqXHR arg3) {
                    ngItems = o.As<JsArray<JsArray<ngFoodRate>>>();
                    _scope.apply();
                }, onRequestFailed);
        }
    }
}