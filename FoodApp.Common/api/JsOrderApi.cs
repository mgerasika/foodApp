using System;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsOrderApi : JsApiBase {
        public JsOrderApi(string serverUrl, string userId) : base(serverUrl, userId) {
        }


        public void GetAllOrders(string userId,JsHandler<JsArray<JsArray<ngOrderEntry>>> handler) {
            string url = OrderUrl.Inst.GetAllOrdersUrl(userId);
            SendGet(url, delegate(JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.assert(null != res);
                if (null != handler) {
                    handler(res.As<JsArray<JsArray<ngOrderEntry>>>());
                }
            });
        }

        public void GetOrdersByDay(string userId,int day, JsHandler<JsArray<ngOrderEntry>> handler)
        {
            string url = OrderUrl.Inst.GetOrdersByDayUrl(userId,day);
            SendGet(url, delegate(JsArray<ngOrderEntry> args)
            {
                object res = Deserealize(args);
                jsCommonUtils.inst.assert(null != res);
                if (null != handler)
                {
                    handler(res.As<JsArray<ngOrderEntry>>());
                }
            });
        }

        public void DeleteOrder(int day, string foodId, JsHandler<bool> handler) {
            string url = OrderUrl.Inst.GetDeleteOrderUrl(_userId,day,foodId);
            SendDelete(url, delegate (JsArray<ngFoodItem> args) {
                object res = Deserealize(args);
                jsCommonUtils.inst.assert(null != res);
                if (null != handler)
                {
                    handler(res.As<bool>());
                }
            });
        }
    }
}