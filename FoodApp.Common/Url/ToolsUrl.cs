using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class ToolsUrl : UrlBase
    {
        private const string c_sFoodsPrefix = "api/tools";
        public const string c_sClearTodayOrders = c_sFoodsPrefix + "/clearTodayOrders/";
        public static ToolsUrl Inst = new ToolsUrl();

        public string GetClearTodayOrdersUrl()
        {
            string res = FormatUrl(c_sClearTodayOrders);
            return res;
        }

       
    }
}