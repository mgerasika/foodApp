namespace FoodApp.Common.Url {
    public class HistoryUrl {
        public const string c_sHistoryPrefix = "api/history";
        public const string c_sGetHistory = c_sHistoryPrefix + "/{userId}/";
        public const string c_sDeleteHistoryPrefix = "api/history/delete";
        public const string c_sDeleteHistory = "api/history/delete/{userId}/";
    }
}