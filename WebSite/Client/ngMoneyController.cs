using angularjs;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngMoneyController : ngControllerBase {
        public static ngMoneyController inst = new ngMoneyController();

        public override string className {
            get { return "ngMoneyController"; }
        }

        public override string @namespace {
            get { return WebApiResources.@namespace; }
        }

        public ngMoneyClientEntry ngEntry {
            get { return _scope["ngEntry"].As<ngMoneyClientEntry>(); }
            set { _scope["ngEntry"] = value; }
        }

        public JsArray<ngUserModel> ngUsers {
            get { return _scope["ngUsers"].As<JsArray<ngUserModel>>(); }
            set { _scope["ngUsers"] = value; }
        }

        public ngAddMoneyModel ngAddMoneyModel {
            get { return _scope["ngAddMoneyModel"].As<ngAddMoneyModel>(); }
            set { _scope["ngAddMoneyModel"] = value; }
        }


        public void buyMoneyClick() {
            jsUtils.inst.showLoading();

            JsService.Inst.MoneyApi.Buy(ngAppController.inst.ngDayOfWeek, delegate {
                refresh();
                jsUtils.inst.hideLoading();
            });
        }

        public void addMoneyClick(string userId, decimal val) {
            jsUtils.inst.showLoading();

            val = fixDecimal(val);
            JsService.Inst.MoneyApi.AddMoney(userId, val, delegate {
                requestGetMoney(userId, delegate(decimal arg) { ngAddMoneyModel.total = arg;
                    _scope.apply(); });
               
                jsUtils.inst.hideLoading();
            });
        }

        private static decimal fixDecimal(decimal val)
        {
            //JsString jsString = val.As<JsString>().toString();
            //val = jsString.replace(".", ",").As<decimal>();
            return val;
        }

        public void checkMoneyClick(string userId) {
            jsUtils.inst.showLoading();
            requestGetMoney(userId, delegate(decimal arg) {
                ngAddMoneyModel.total = arg; _scope.apply();
                jsUtils.inst.hideLoading();
            });
        }

        public void removeMoneyClick(string userId, decimal val) {
            jsUtils.inst.showLoading();

            val = fixDecimal(val);
            JsService.Inst.MoneyApi.RemoveMoney(userId, val, delegate {
                requestGetMoney(userId, delegate(decimal arg) { ngAddMoneyModel.total = arg; _scope.apply();});
                
                jsUtils.inst.hideLoading();
            });
        }

        public void requestGetMoney(string userId, JsAction<decimal> complete) {
            JsService.Inst.MoneyApi.GetMoney(userId, delegate(decimal res) {
                if (null != complete) {
                    complete(res);
                }
            });
        }

        public void refundMoneyClick() {
            jsUtils.inst.showLoading();

            JsService.Inst.MoneyApi.Refund(ngAppController.inst.ngDayOfWeek, delegate {
                refresh();
                jsUtils.inst.hideLoading();
            });
        }

        public void canBuyMoneyClick(int day) {
            jsUtils.inst.showLoading();

            requestCanBuy(day, delegate { jsUtils.inst.hideLoading(); });
        }

        public void canRefundMoneyClick(int day) {
            jsUtils.inst.showLoading();

            requestCanRefund(day, delegate { jsUtils.inst.hideLoading(); });
        }


        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);

            ngEntry = new ngMoneyClientEntry();
            ngAddMoneyModel = new ngAddMoneyModel();
            ngUsers = new JsArray<ngUserModel>();

            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) {
                HtmlContext.console.log("settings loaded");

                HtmlContext.window.setTimeout(delegate() {
                    eventManager.inst.subscribe(eventManager.orderListChanged, delegate(int n2)
                    {
                        HtmlContext.console.log("order list changed");
                        refresh();
                    });

                }, 1000);
            });

            eventManager.inst.subscribe(eventManager.dayChanged, delegate(int n) {
                HtmlContext.console.log("money day changed");
                refresh();
            });

           
            requestGetUsers(delegate { });
        }

        private ngMoneyController() {
        }

        private void requestCanBuy(int day, JsAction complete) {
            JsService.Inst.MoneyApi.CanBuy(day, delegate(bool res) {
                ngEntry.canBuy = jsCommonUtils.inst.toBool(res);

                HtmlContext.console.log("request can buy for day = " + day + " response = " + ngEntry.canBuy);
                if (null != complete) {
                    complete();
                }
            });
        }

        private void requestGetUsers(JsAction complete) {
            JsService.Inst.UsersApi.GetUsers(delegate(JsArray<ngUserModel> res) {
                JsArray<ngUserModel> tmp = new JsArray<ngUserModel>();
                foreach (ngUserModel user in res) {
                    if (-1 != user.Email.As<JsString>().indexOf("darwins")) {
                        tmp.Add(user);
                    }
                }
                ngUsers = tmp;

                if (null != complete) {
                    complete();
                }
            });
        }

        public bool hasOrders()
        {
            int day = ngAppController.inst.ngDayOfWeek;
            JsArray<ngOrderEntry> orders = ngOrderController.inst.getOrders(day);
            bool res = orders != null && orders.length > 0;
            return res;
        }

        private void requestCanRefund(int day, JsAction complete) {
            JsService.Inst.MoneyApi.CanRefund(day, delegate(bool res) {
                ngEntry.canRefund = jsCommonUtils.inst.toBool(res);

                HtmlContext.console.log("request can refund for day = " + day + " response = " + ngEntry.canRefund);
                if (null != complete) {
                    complete();
                }
            });
        }

        private void refresh() {
            string userId = JsService.Inst.MoneyApi.getUserId();

            HtmlContext.console.log("money refresh");
            jsUtils.inst.showLoading();
            int day = ngAppController.inst.ngDayOfWeek;
            requestCanBuy(day, delegate {
                requestCanRefund(day, delegate {
                    requestGetMoney(userId, delegate(decimal val) {
                        ngEntry.total = val;
                        
                        _scope.apply();
                        jsUtils.inst.hideLoading();
                    });
                });
            });
        }
    }
}