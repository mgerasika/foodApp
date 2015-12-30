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

        public override string className
        {
            get { return "ngFavoriteController"; }
        }

        public JsArray<ngFoodRate> ngFavoriteItems {
            get { return _scope["ngFavoriteItems"].As<JsArray<ngFoodRate>>(); }
            set { _scope["ngFavoriteItems"] = value; }
        }

      

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngFavoriteItems = new JsArray<ngFoodRate>();

            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refreshFavorite(); });

           
        }

        public ngFoodItem getFoodItem(string id)
        {
            ngFoodItem item = ngFoodController.inst.findFoodById(id);
            return item;
        }

        public void refreshFavorite() {
            serviceHlp.inst.SendGet("json",
                FavoriteFoodController.c_sGetFavorite + "/" + ngAppController.inst.ngUserId + "/",
                delegate(object o, JsString s, jqXHR arg3) {
                    ngFavoriteItems = o.As<JsArray<ngFoodRate>>();
                    _scope.apply();
                }, onRequestFailed);
        }

        public void rateChanged(ngFoodRate rate,double newRate) {
            rate.Rate = newRate;

            serviceHlp.inst.SendPost("json", FavoriteFoodController.c_sGetFavorite + "/" + ngAppController.inst.ngUserId + "/" + rate.FoodId + "/" + rate.Rate + "/",
                new JsObject(),
                delegate {}, onRequestFailed);
        }
    }
}