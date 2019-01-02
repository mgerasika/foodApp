using angularjs;
using FoodApp.Common;
using SharpKit.JavaScript;

namespace MobileWebApp.Common.client {
    [JsType(JsMode.Prototype, Filename = MobileApiResources._fileClientTmpJs)]
    public class ngMobileOrderController : ngMobileControllerBase {
        public static ngMobileOrderController inst = new ngMobileOrderController();

        private ngMobileOrderController() {
        }

        public override string className {
            get { return "ngMobileOrderController"; }
        }

        public JsArray<JsArray<ngOrderEntry>> ngOrderEntries {
            get { return _scope["ngOrderEntries"].As<JsArray<JsArray<ngOrderEntry>>>(); }
            set { _scope["ngOrderEntries"] = value; }
        }


        public ngFoodItem getFoodItem(string id) {
            ngFoodItem item = ngMobileFoodController.inst.findFoodById(id);
            return item;
        }

        public string formatCount(ngOrderEntry order) {
            string res = order.Count.As<string>() + "";
            ngFoodItem food = getFoodItem(order.FoodId);
            if (food.isByWeightItem) {
                res = JsContext.parseInt((order.Count*100).As<string>(), 10).As<string>() + "";
            }
            return res;
        }

        public decimal getTotalPrice(int day) {
            decimal res = 0;
            JsArray<ngOrderEntry> ngOrderModels = ngOrderEntries[day];
            if (ngOrderModels != null) {
                foreach (ngOrderEntry item in ngOrderModels) {
                    ngFoodItem food = getFoodItem(item.FoodId);
                    if (null != food) {
                        res += jsUtils.inst.fixNumber(food.Price*item.Count);
                        res = jsUtils.inst.fixNumber(res);
                    }
                }
            }
            res = jsUtils.inst.fixNumber(res);
            return res;
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngOrderEntries = new JsArray<JsArray<ngOrderEntry>>();

            eventManager.inst.subscribe(eventManager.deviceReady, delegate(int n) { refreshOrders(); });
            eventManager.inst.subscribe(eventManager.orderCompleted, delegate(int n) { refreshOrders(); });
        }

        public void deleteOrder(int day, string foodId) {
            jsUtils.inst.showLoading();

            JsService.Inst.OrderApi.DeleteOrder( day, foodId, delegate(bool res) {
                jsUtils.inst.hideLoading();
                refreshOrders();
                if (!res) {
                    
                }
            });

        }

        public void refreshOrders() {
            JsService.Inst.OrderApi.GetAllOrders(ngMobileAppController.inst.ngUserId, delegate(JsArray<JsArray<ngOrderEntry>> tmp) {
                while (ngOrderEntries.length > 0) {
                    ngOrderEntries.pop();
                }
                foreach (JsArray<ngOrderEntry> obj in tmp) {
                    ngOrderEntries.push(obj);
                }
                eventManager.inst.fire(eventManager.orderListChanged, ngOrderEntries);
                _scope.apply();
            });
        }

        public JsArray<ngOrderEntry> getOrders(int day) {
            return ngOrderEntries[day];
        }

        public ngOrderEntry getOrderByFoodId(int day, string foodId) {
            ngOrderEntry res = null;
            JsArray<ngOrderEntry> orders = inst.getOrders(day);
            foreach (ngOrderEntry order in orders) {
                if (order.FoodId == foodId) {
                    res = order;
                    break;
                }
            }
            return res;
        }
    }
}