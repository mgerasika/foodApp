using angularjs;
using FoodApp.Common;
using FoodApp.Controllers.api;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngFavoriteController : ngControllerBase
    {
        public static ngFavoriteController inst = new ngFavoriteController();

        private ngFavoriteController()
        {
        }

        public override string name {
            get { return "ngFavoriteController"; }
        }

       

        public JsArray<ngFoodRate> ngItems {
            get { return _scope["ngItems"].As<JsArray<ngFoodRate>>(); }
            set { _scope["ngItems"] = value; }
        }

        protected string getUrl() {
            return FavoriteFoodController.c_sGetFavorite;
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngItems = new JsArray<ngFoodRate>();

            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refresh(); });

            refresh();
        }

        public ngFoodItem getFoodItem(string id)
        {
            var item = ngFoodController.inst.findItemById(id);
            return item;
        }

        public void deleteOrder(string id) {
            serviceHlp.inst.SendDelete("json",
                getUrl() + "/" + ngAppController.inst.ngUserId + "/" + ngAppController.inst.ngDayOfWeek + "/" + id,
                new JsObject(), delegate { refresh(); }, onRequestFailed);
        }

        public void refresh() {
            serviceHlp.inst.SendGet("json",
                getUrl() + "/" + ngAppController.inst.ngUserId + "/" + ngAppController.inst.ngDayOfWeek,
                delegate(object o, JsString s, jqXHR arg3) {
                    ngItems = o.As<JsArray<ngFoodRate>>();
                    _scope.apply();
                }, onRequestFailed);
        }

        public void rateChanged(ngFoodRate rate,double newRate) {
            rate.Rate = newRate;

            serviceHlp.inst.SendPost("json", getUrl() + "/" + ngAppController.inst.ngUserId + "/" + rate.FoodId + "/" + rate.Rate,
                new JsObject(),
                delegate { ngOrderController.inst.refresh(); }, onRequestFailed);
        }
    }
}