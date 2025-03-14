using System;
using System.Collections.Generic;
using FoodApp.Common.Parser;

namespace FoodApp.Common.Managers {
    public class OrderManager {
        public static OrderManager Inst = new OrderManager();

        public string GetOrderId(ngUserModel user, int day) {
            string res = "user:" + user.Id + " dayofweek:" + day + " date:" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day;
            return res;
        }

        public List<ngOrderEntry> GetOrders(ngUserModel user, int day) {
           
            ExcelTable excelTable = ExcelManager.Inst.Doc.GetExcelTable(day);
            List<ngOrderEntry> res = GetOrdersInternal(user, excelTable);

            return res;
        }

        public List<ngOrderEntry> GetOrdersInternal(ngUserModel user, ExcelTable excelTable) {
            List<ngOrderEntry> res = new List<ngOrderEntry>();
            if (null != excelTable) {
                List<ExcelRow> rows = excelTable.Rows;
                foreach (ExcelRow row in rows) {
                    ExcelCell cell = row.GetCell(user.Column);
                    if (cell != null && row.HasPrice && cell.Value > 0) {
                        ngOrderEntry item = new ngOrderEntry();
                        item.Count = cell.Value;
                        item.FoodId = row.GetFoodId();
                        res.Add(item);
                    }
                }
            }
            return res;
        }

        public bool Buy(ngUserModel user, int day, string foodId, decimal val) {
            bool res = false;
            /*
            ExcelTable excelTable = ExcelManager.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowByFoodId(foodId);
            ExcelCell cell = row.EnsureCell(user.Column);
            decimal prevVal = cell.Value;
            cell.Value = val;
            */
            bool isNewFood = true;
            List<ngOrderEntry> orders = GetOrders(user, day);
            foreach (ngOrderEntry order in orders) {
                if (order.FoodId.Equals(foodId, StringComparison.OrdinalIgnoreCase))
                {
                    order.Count = val;
                    isNewFood = false;
                }
            }
            if (isNewFood) {
                ngOrderEntry item = new ngOrderEntry();
                item.Count = val;
                item.FoodId = foodId;
                orders.Add(item);
            }

            List<ngOrderEntry> newOrders = ApiUtils.AddContainersToFood(user, day, orders);
            if (BatchCellUpdater.Update(user, day, newOrders)) {
                res = true;
            }

            return res;
        }

        internal bool Delete(ngUserModel user, int day, string foodId) {
            bool res = false;
            /*
            ExcelTable excelTable = ExcelManager.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowByFoodId(foodId);
            ExcelCell cell = row.GetCell(user.Column);
            cell.Value = 0;
             * */
            List<ngOrderEntry> orders = GetOrders(user, day);
            foreach (ngOrderEntry order in orders) {
                if (order.FoodId.Equals(foodId, StringComparison.OrdinalIgnoreCase)) {
                    order.Count = 0;
                }
            }
            /*
            List<ngOrderEntry> oldOrders = GetOrders(user, day);
            oldOrders.Add(deletedOrder);
            */

            List<ngOrderEntry> newOrders = ApiUtils.AddContainersToFood(user, day, orders);
            if (BatchCellUpdater.Update(user, day, newOrders)) {
                res = true;
            }

            return res;
        }

        internal List<ngFoodItem> GetOrderedFoods(ngUserModel user, int day) {
            List<ngFoodItem> res = new List<ngFoodItem>();
            List<ngOrderEntry> orders = GetOrders(user, day);
            foreach (ngOrderEntry order in orders) {
                ngFoodItem ngFoodItem = FoodManager.Inst.GetFoodById(day, order.FoodId);
                res.Add(ngFoodItem);
            }
            return res;
        }

        internal bool HasOrders(int dayOfWeek) {
            bool res = false;
            List<ngUserModel> users = UsersManager.Inst.GetUsers();
            foreach (ngUserModel user in users) {
                List<ngOrderEntry> orders = GetOrders(user, dayOfWeek);
                if (orders.Count > 0) {
                    res = true;
                    break;
                }
            }
            return res;
        }

        internal List<ngOrderEntry> GetOrders(int dayOfWeek) {
            List<ngOrderEntry> res = new List<ngOrderEntry>();
            List<ngUserModel> users = UsersManager.Inst.GetUsers();
            foreach (ngUserModel user in users) {
                List<ngOrderEntry> orders = GetOrders(user, dayOfWeek);
                if (orders.Count > 0) {
                    res.AddRange(orders);
                }
            }
            return res;
        }


        internal decimal CalcTotal(ngUserModel user, int day) {
            List<ngOrderEntry> orders = GetOrders(user, day);
            decimal res = 0;
            foreach (ngOrderEntry item in orders) {
                ngFoodItem food = FoodManager.Inst.GetFoodById(item.FoodId);
                res += item.Count*food.Price;
            }
            return res;
        }
    }
}