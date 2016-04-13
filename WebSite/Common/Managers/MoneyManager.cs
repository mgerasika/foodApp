using System;
using System.Diagnostics;
using FoodApp.Model;

namespace FoodApp.Common.Managers {
    public class MoneyManager : ManagerBase<ngMoneyModel> {
        public static MoneyManager Inst = new MoneyManager();
        private static readonly object _lockObject = new object();


        public ngMoneyModel EnsureMoneyModel(ngUserModel user) {
            ngMoneyModel res = GetItem(user.Id);
            if (null == res) {
                lock (_lockObject) {
                    if (null == GetItem(user.Id)) {
                        res = new ngMoneyModel();
                        res.UserId = user.Id;
                        AddItemAndSave(res);
                    }
                }
            }
            return res;
        }

        public bool CanRefund(ngUserModel user, string order) {
            bool res = false;
            lock (_lockObject) {
                ngMoneyOrderModel moneyOrder = GetMoneyOrderModel(user, order);
                if (moneyOrder != null && moneyOrder.Operation == EMoneyOperation.Buy) {
                    res = true;
                }
            }
            return res;
        }

        public bool CanBuy(ngUserModel user, decimal val, string orderId) {
            bool res = false;
            lock (_lockObject) {
                ngMoneyModel moneyModel = EnsureMoneyModel(user);
                Debug.Assert(null != moneyModel);
                ngMoneyOrderModel moneyOrder = GetMoneyOrderModel(user, orderId);
                if (null != moneyOrder && moneyOrder.Operation == EMoneyOperation.Buy) {
                    res = false;
                }
                else if (moneyModel.Total - val > 0) {
                    res = true;
                }
            }
            return res;
        }

        public void Buy(ngUserModel user, decimal val, string orderId) {
            if (CanBuy(user, val, orderId)) {
                lock (_lockObject) {
                    ngMoneyModel moneyModel = EnsureMoneyModel(user);
                    Debug.Assert(null != moneyModel);
                    ngMoneyOrderModel moneyOrder = GetMoneyOrderModel(user, orderId);
                    if (null == moneyOrder) {
                        moneyOrder = new ngMoneyOrderModel();
                        moneyModel.MoneyOrders.Add(moneyOrder);
                    }
                    moneyOrder.Id = orderId;
                    moneyOrder.Operation = EMoneyOperation.Buy;
                    moneyOrder.DateTime = DateTime.Now;
                    moneyOrder.Value = val;
                    moneyModel.Total -= val;
                    Save();

                    MoneyLogger.Inst.CreateBuyLog(user, val, orderId);
                }
            }
        }

        public void Refund(ngUserModel user, string orderId) {
            if (CanRefund(user, orderId)) {
                lock (_lockObject) {
                    ngMoneyModel moneyModel = EnsureMoneyModel(user);
                    Debug.Assert(null != moneyModel);
                    ngMoneyOrderModel moneyOrder = GetMoneyOrderModel(user, orderId);
                    moneyOrder.Operation = EMoneyOperation.Refund;
                    moneyModel.Total += moneyOrder.Value;

                    moneyModel.DeleteOrder(moneyOrder);
                    Save();

                    MoneyLogger.Inst.CreateRefundLog(user, moneyOrder.Value, orderId);
                }
            }
        }

        public void Add(ngUserModel user, decimal total) {
            lock (_lockObject) {
                ngMoneyModel moneyModel = EnsureMoneyModel(user);
                Debug.Assert(null != moneyModel);
                moneyModel.Total += total;
                Save();

                MoneyLogger.Inst.CreateAddMoneyLog(user, total);
            }
        }

        public void Remove(ngUserModel user, decimal total) {
            lock (_lockObject) {
                ngMoneyModel moneyModel = EnsureMoneyModel(user);
                Debug.Assert(null != moneyModel);
                moneyModel.Total -= total;
                Save();

                MoneyLogger.Inst.CreateRemoveMoneyLog(user, total);
            }
        }

        internal decimal GetMoney(ngUserModel user) {
            decimal res = 0;

            lock (_lockObject) {
                ngMoneyModel moneyModel = EnsureMoneyModel(user);
                Debug.Assert(null != moneyModel);
                res = moneyModel.Total;
            }
            return res;
        }

        private MoneyManager() {
        }

        protected override string FileName {
            get { return "money.json"; }
        }

        protected override string GetId(ngMoneyModel obj) {
            return obj.UserId;
        }

        protected ngMoneyOrderModel GetMoneyOrderModel(ngUserModel user, string id) {
            ngMoneyModel moneyModel = EnsureMoneyModel(user);
            ngMoneyOrderModel res = null;
            foreach (ngMoneyOrderModel ngMoneyOrderModel in moneyModel.MoneyOrders) {
                if (ngMoneyOrderModel.Id.Equals(id)) {
                    res = ngMoneyOrderModel;
                    break;
                }
            }
            return res;
        }
    }
}