using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class JsMoneyApi : JsApiBase {
        public void Buy(int day, JsHandler<bool> handler) {
            string url = MoneyUrl.Inst.GetBuyUrl(_userId, day);
            SendGet(url, delegate(bool args) {
                if (null != handler) {
                    handler(args);
                }
            });
        }

        public void AddMoney(string userId,decimal val, JsHandler<bool> handler) {
           
            string url = MoneyUrl.Inst.GetAddMoneyUrl(userId, val);
            SendGet(url, delegate(bool args) {
                if (null != handler) {
                    handler(args);
                }
            });
        }

       

        public void RemoveMoney(string userId, decimal val, JsHandler<bool> handler)
        {
           
            string url = MoneyUrl.Inst.GetRemoveMoneyUrl(userId, val);
            SendGet(url, delegate(bool args)
            {
                if (null != handler)
                {
                    handler(args);
                }
            });
        }

        public void GetMoney(string userId, JsHandler<decimal> handler)
        {
            string url = MoneyUrl.Inst.GetMoneyUrl(userId);
            SendGet(url, delegate(decimal args)
            {
                if (null != handler)
                {
                    handler(args);
                }
            });
        }

        public void GetUsers(JsHandler<JsArray<ngUserModel>> handler)
        {
            string url = MoneyUrl.Inst.GetUsers();
            SendGet(url, delegate(JsArray<ngUserModel> args)
            {
                if (null != handler)
                {
                    handler(args);
                }
            });
        }

        public void CanBuy(int day, JsHandler<bool> handler) {
            string url = MoneyUrl.Inst.GetCanBuyUrl(_userId, day);
            SendGet(url, delegate(bool args) {
                if (null != handler) {
                    handler(args);
                }
            });
        }

        public void Refund(int day, JsHandler<bool> handler) {
            string url = MoneyUrl.Inst.GetRefundUrl(_userId, day);
            SendGet(url, delegate(bool args) {
                if (null != handler) {
                    handler(args);
                }
            });
        }

        public void CanRefund(int day, JsHandler<bool> handler) {
            string url = MoneyUrl.Inst.GetCanRefundUrl(_userId, day);
            SendGet(url, delegate(bool args) {
                if (null != handler) {
                    handler(args);
                }
            });
        }

        public JsMoneyApi(string serverUrl, string userId)
            : base(serverUrl, userId) {
        }
    }
}