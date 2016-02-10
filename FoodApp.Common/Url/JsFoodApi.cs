using FoodApp.Common.Model;
using SharpKit.JavaScript;

namespace FoodApp.Common.Url {

    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsService {
        public JsFoodApi FoodApi { get; set; }

        public static JsService Inst = new JsService();

        public void Init(string serverUrl,string userId) {
            FoodApi = new JsFoodApi(serverUrl,userId);
        }
    }

    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsFoodApi : JsApiBase {

        public JsFoodApi(string serverUrl,string userId) : base(serverUrl,userId) {
            
        }
        public void GetFoods(int day,JsHandler<JsArray<ngFoodItem>> handler)
        {
            string url = FoodsUrl.Inst.GetFoodsByDayUrl(_userId,day);
            SendGet(url, delegate (JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.Assert(null != res);
                if (null != handler)
                {
                    handler(res.As<JsArray<ngFoodItem>>());
                }
            });
        }
    }
}