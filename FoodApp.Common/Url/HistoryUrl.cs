using SharpKit.JavaScript;

namespace FoodApp.Common.Url {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class HistoryUrl : UrlBase
    {
        public const string c_sHistoryPrefix = "api/history";
        public const string c_sGetHistory = c_sHistoryPrefix + "/{userId}/";
        public const string c_sDeleteHistoryPrefix = "api/history/delete";
        public const string c_sDeleteHistory = "api/history/delete/{userId}/";
    }
}