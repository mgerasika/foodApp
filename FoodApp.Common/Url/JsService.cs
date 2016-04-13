using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsService {
        public JsFoodApi FoodApi { get; set; }
        public JsOrderApi OrderApi { get; set; }
        public JsMoneyApi MoneyApi { get; set; }
        public JsToolsApi ToolsApi { get; set; }

        public static JsService Inst = new JsService();

        public void Init(string serverUrl,string userId) {
            FoodApi = new JsFoodApi(serverUrl,userId);
            OrderApi = new JsOrderApi(serverUrl,userId);
            MoneyApi = new JsMoneyApi(serverUrl,userId);
            ToolsApi = new JsToolsApi(serverUrl, userId);
        }
    }
}