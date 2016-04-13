using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsFoodApi : JsApiBase {
        public void GetFoods(int day, JsHandler<JsArray<ngFoodItem>> handler) {
            string url = FoodsUrl.Inst.GetFoodsByDayUrl(_userId, day);
            SendGet(url, delegate(JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.Assert(null != res);
                if (null != handler) {
                    handler(res.As<JsArray<ngFoodItem>>());
                }
            });
        }

        public void GetAllFoods(JsHandler<JsArray<JsArray<ngFoodItem>>> handler) {
            string url = FoodsUrl.Inst.GetAllFoodsUrl(_userId);
            SendGet(url, delegate(JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.Assert(null != res);
                if (null != handler) {
                    handler(res.As<JsArray<JsArray<ngFoodItem>>>());
                }
            });
        }

        public void Buy(int day, string foodId, decimal value, JsHandler<bool> handler) {
            string url = FoodsUrl.Inst.GetBuyUrl(_userId, day, foodId, value);
            SendPost(url, new JsObject(), delegate(JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.Assert(null != res);
                if (null != handler) {
                    handler(res.As<bool>());
                }
            });
        }

        public void ChangePrice(int day, string foodId, decimal value, JsHandler<string> handler) {
            string url = FoodsUrl.Inst.GetChangePriceUrl(_userId, day, foodId, value);
            SendPost(url, new JsObject(), delegate(JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.Assert(null != res);
                if (null != handler) {
                    handler(res.As<string>());
                }
            });
        }

        public JsFoodApi(string serverUrl, string userId) : base(serverUrl, userId) {
        }
    }
}