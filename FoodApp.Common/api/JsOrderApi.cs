using System;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsOrderApi : JsApiBase {
        public JsOrderApi(string serverUrl, string userId) : base(serverUrl, userId) {
        }


        public void GetOrders(JsHandler<JsArray<JsArray<ngOrderEntry>>> handler) {
            string url = OrderUrl.Inst.GetAllOrdersUrl(_userId);
            SendGet(url, delegate(JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.Assert(null != res);
                if (null != handler) {
                    handler(res.As<JsArray<JsArray<ngOrderEntry>>>());
                }
            });
        }

        public void DeleteOrder(int day, string foodId, JsHandler<bool> handler) {
            string url = OrderUrl.Inst.GetDeleteOrderUrl(_userId,day,foodId);
            SendDelete(url, delegate (JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.Assert(null != res);
                if (null != handler)
                {
                    handler(res.As<bool>());
                }
            });
        }
    }
}