using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsToolsApi : JsApiBase
    {
        public JsToolsApi(string serverUrl, string userId)
            : base(serverUrl, userId)
        {
        }



        public void ClearTodayOrders(JsHandler<bool> handler)
        {
            string url = ToolsUrl.Inst.GetClearTodayOrdersUrl();
            SendGet(url, delegate(bool args)
            {
                if (null != handler)
                {
                    handler(args);
                }
            });
        }
    }
}