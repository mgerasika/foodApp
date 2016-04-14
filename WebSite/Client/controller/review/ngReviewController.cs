using angularjs;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Json, Filename = WebApiResources._fileClientJs)]
    public class ngReviewModel {
        public ngUserModel user { get; set; }
        public JsArray<ngOrderEntry> orders { get; set; }
    }


    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngReviewController : ngControllerBase {
        public static ngReviewController inst = new ngReviewController();


        public override string className {
            get { return "ngReviewController"; }
        }

        public override string @namespace {
            get { return WebApiResources.@namespace; }
        }

        public JsArray<ngReviewModel> ngReviewModel {
            get { return _scope["ngReviewModel"].As<JsArray<ngReviewModel>>(); }
            set { _scope["ngReviewModel"] = value; }
        }

        public bool ngIsInit
        {
            get { return _scope["ngIsInit"].As<bool>(); }
            set { _scope["ngIsInit"] = value; }
        }

        public JsArray<ngFoodItem> ngFoods {
            get { return _scope["ngFoods"].As<JsArray<ngFoodItem>>(); }
            set { _scope["ngFoods"] = value; }
        }

        public JsArray<ngOrderEntry> ngTotalOrders {
            get { return _scope["ngTotalOrders"].As<JsArray<ngOrderEntry>>(); }
            set { _scope["ngTotalOrders"] = value; }
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);

            ngReviewModel = new JsArray<ngReviewModel>();
            ngFoods = new JsArray<ngFoodItem>();
            ngTotalOrders = new JsArray<ngOrderEntry>();

            eventManager.inst.subscribe(eventManager.dayChanged, delegate(int n) { requestGetFoods(); });
        }

        public ngFoodItem findFoodById(string id) {
            ngFoodItem res = null;

            foreach (ngFoodItem item in ngFoods) {
                if (item.FoodId == id) {
                    res = item;
                    break;
                }
            }

            return res;
        }

        public ngFoodItem getFoodItem_ReviewCtrl(string id) {
            ngFoodItem item = findFoodById(id);
            return item;
        }

        public string formatCount_ReviewCtrl(ngOrderEntry order) {
            string res = order.Count.As<string>() + "";
            ngFoodItem food = getFoodItem_ReviewCtrl(order.FoodId);
            if (food.isByWeightItem) {
                res = JsContext.parseInt((order.Count*100).As<string>(), 10).As<string>() + "";
            }
            return res;
        }

        private ngReviewController() {
        }

        private void requestGetFoods() {
            JsService.Inst.FoodApi.GetFoodsByDay(JsService.Inst.FoodApi.getUserId(), ngAppController.inst.ngDayOfWeek, delegate(JsArray<ngFoodItem> data) {
                ngFoods = data;

                requestGetUsers();
            });
        }

        private void requestGetUsers() {
            JsService.Inst.UsersApi.GetUsers(delegate(JsArray<ngUserModel> users) {
                JsArray<ngUserModel> tmp = new JsArray<ngUserModel>();
                foreach (ngUserModel ngUser in users) {
                    if (-1 != ngUser.Email.As<JsString>().indexOf("darwins")) {
                        tmp.Add(ngUser);
                    }
                }

                if (tmp.length > 0) {
                    requestGetOrders(tmp);
                }
            });
        }

        private void requestGetOrders(JsArray<ngUserModel> tmp) {
            getOrdersRecursive(tmp, new JsArray<ngReviewModel>(), delegate(JsArray<ngReviewModel> result) {
                ngIsInit = true;
                ngTotalOrders = createTotalOrders(result);
                ngReviewModel = result;
                _scope.apply();
            });
        }

        private JsArray<ngOrderEntry> createTotalOrders(JsArray<ngReviewModel> items) {
            JsArray<ngOrderEntry> res = new JsArray<ngOrderEntry>();
            foreach (ngReviewModel reviewModel in items) {
                foreach (ngOrderEntry order in reviewModel.orders) {
                    ngFoodItem foodItem = findFoodById(order.FoodId);
                    jsCommonUtils.inst.assert(null != foodItem);
                    if (!conainsFoodId(res, order.FoodId)) {
                        ngOrderEntry totalOrder = new ngOrderEntry();
                        totalOrder.FoodId = order.FoodId;
                        if (foodItem.isByWeightItem) {
                            totalOrder.Count = 1;
                        }
                        else {
                            totalOrder.Count = order.Count;
                        }
                        res.Add(totalOrder);
                    }
                    else {
                        ngOrderEntry totalOrder = getOrderByFoodId(res, order.FoodId);
                        if (foodItem.isByWeightItem) {
                            totalOrder.Count += 1;
                        }
                        else {
                            totalOrder.Count += order.Count;
                        }
                    }
                }
            }


            res = groupByCategory(res);
            return res;
        }

        private JsArray<ngOrderEntry> groupByCategory(JsArray<ngOrderEntry> items) {
            JsArray<ngOrderEntry> res = new JsArray<ngOrderEntry>();

            JsArray<ngOrderEntry> salats = new JsArray<ngOrderEntry>();
            JsArray<ngOrderEntry> first = new JsArray<ngOrderEntry>();
            JsArray<ngOrderEntry> garnirs = new JsArray<ngOrderEntry>();
            JsArray<ngOrderEntry> others = new JsArray<ngOrderEntry>();
            foreach (ngOrderEntry item in items) {
                ngFoodItem food = findFoodById(item.FoodId);
                if (food.isSalat) {
                    salats.Add(item);
                }
                else if (food.isFirst) {
                    first.Add(item);
                }
                else if (food.isGarnir) {
                    garnirs.Add(item);
                }
                else {
                    others.Add(item);
                }
            }

            jsUtils.inst.addRange(res, salats);
            jsUtils.inst.addRange(res, first);
            jsUtils.inst.addRange(res, garnirs);
            jsUtils.inst.addRange(res, others);
            return res;
        }

        private bool conainsFoodId(JsArray<ngOrderEntry> items, string foodId) {
            bool res = false;
            foreach (ngOrderEntry item in items) {
                if (item.FoodId == foodId) {
                    res = true;
                    break;
                }
            }
            return res;
        }

        private ngOrderEntry getOrderByFoodId(JsArray<ngOrderEntry> items, string foodId) {
            ngOrderEntry res = null;
            foreach (ngOrderEntry item in items) {
                if (item.FoodId == foodId) {
                    res = item;
                    break;
                }
            }
            return res;
        }

        private void getOrdersRecursive(JsArray<ngUserModel> users, JsArray<ngReviewModel> res, JsHandler<JsArray<ngReviewModel>> complete) {
            if (users.length > 0) {
                ngUserModel ngUser = users.pop();
                JsService.Inst.OrderApi.GetOrdersByDay(ngUser.Id, ngAppController.inst.ngDayOfWeek, delegate(JsArray<ngOrderEntry> data) {
                    if (data.length > 0) {
                        ngReviewModel model = new ngReviewModel();
                        model.user = ngUser;
                        model.orders = removeContainersFromOrders(data);
                        res.Add(model);
                    }

                    getOrdersRecursive(users, res, complete);
                });
            }
            else {
                complete(res);
            }
        }

        private JsArray<ngOrderEntry> removeContainersFromOrders(JsArray<ngOrderEntry> data) {
            JsArray<ngOrderEntry> res = new JsArray<ngOrderEntry>();
            foreach (ngOrderEntry entry in data) {
                ngFoodItem foodItem = findFoodById(entry.FoodId);
                if (!(foodItem.isContainer || foodItem.isBigContainer || foodItem.isSmallContainer)) {
                    res.Add(entry);
                }
            }
            return res;
        }
    }
}