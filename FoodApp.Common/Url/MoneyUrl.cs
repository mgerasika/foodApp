using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class MoneyUrl : UrlBase {
        private const string c_sMoneyPrefix = "api/money/";
        public const string c_sGetUsers = c_sMoneyPrefix + "users/";
        public const string c_sGetMoney = c_sMoneyPrefix + "getMoney/{userId}/";
        public const string c_sAddMoney = c_sMoneyPrefix + "addMoney/{userId}/{val}/";
        public const string c_sRemoveMoney = c_sMoneyPrefix + "removeMoney/{userId}/{val}/";
        public const string c_sBuy = c_sMoneyPrefix + "buy/{userId}/{day}/";
        public const string c_sCanBuy = c_sMoneyPrefix + "canBuy/{userId}/{day}/";
        public const string c_sRefund = c_sMoneyPrefix + "refund/{userId}/{day}/";
        public const string c_sCanRefund = c_sMoneyPrefix + "canRefund/{userId}/{day}/";
        public static MoneyUrl Inst = new MoneyUrl();

        public string GetBuyUrl(string userId, int day) {
            string res = FormatUrl(c_sBuy, userId, day.As<string>());
            return res;
        }

        public string GetAddMoneyUrl(string userId, decimal val) {
            string res = FormatUrl(c_sAddMoney, userId, val.As<string>());
            return res;
        }

        public string GetRemoveMoneyUrl(string userId, decimal val)
        {
            string res = FormatUrl(c_sRemoveMoney, userId, val.As<string>());
            return res;
        }

        public string GetMoneyUrl(string userId)
        {
            string res = FormatUrl(c_sGetMoney, userId);
            return res;
        }

        public string GetUsers()
        {
            string res = FormatUrl(c_sGetUsers);
            return res;
        }

        public string GetCanBuyUrl(string userId, int day) {
            string res = FormatUrl(c_sCanBuy, userId, day.As<string>());
            return res;
        }

        public string GetRefundUrl(string userId, int day) {
            string res = FormatUrl(c_sRefund, userId, day.As<string>());
            return res;
        }

        public string GetCanRefundUrl(string userId, int day) {
            string res = FormatUrl(c_sCanRefund, userId, day.As<string>());
            return res;
        }
    }
}