using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Client;
using FoodApp.Controllers;
using GoogleAppsConsoleApplication;

namespace FoodApp.Common {
    public class OrderManager {
        public static OrderManager Inst = new OrderManager();

        public List<ngOrderModel> GetOrders(string email, int day) {
            List<ngOrderModel> res = new List<ngOrderModel>();
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            List<ExcelRow> rows = excelTable.Rows;
            foreach (ExcelRow row in rows) {
                ngUserModel user = UsersManager.Inst.GetUserByEmail(email);
                ExcelCell cell = row.GetCell(user.Column);
                if (cell != null && row.HasPrice && cell.Value > 0) {
                    ngOrderModel item = new ngOrderModel();
                    item.Count = cell.Value;
                    item.FoodId = row.GetFoodId();
                    res.Add(item);
                }
            }

            return res;
        }

        public bool Buy(string email, int day, string foodId, decimal val) {

            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowByFoodId(foodId);
            ngUserModel user = UsersManager.Inst.GetUserByEmail(email);
            ExcelCell cell = row.EnsureCell(user.Column);
            cell.Value += val;

            List<ngOrderModel> oldOrders = GetOrders(email,day);
            List<ngOrderModel> newOrders = ApiUtils.AddContainersToFood(email,day,oldOrders);
            BatchCellUpdater.Update(email,day,newOrders);

            return true;
        }

        internal bool Delete(string email, int day, string foodId) {
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowByFoodId(foodId);
            ngUserModel user = UsersManager.Inst.GetUserByEmail(email);
            ExcelCell cell = row.GetCell(user.Column);
            cell.Value = 0;

            ngOrderModel deletedOrder = new ngOrderModel();
            deletedOrder.FoodId = foodId;
            deletedOrder.Count = 0;
            List<ngOrderModel> oldOrders = GetOrders(email, day);
            oldOrders.Add(deletedOrder);
            
            List<ngOrderModel> newOrders = ApiUtils.AddContainersToFood(email, day, oldOrders);
            BatchCellUpdater.Update(email, day, newOrders);

            return true;
        }

        internal List<ngFoodItem> GetOrderedFoods(string email, int day) {
            List<ngFoodItem> res = new List<ngFoodItem>();
            List<ngOrderModel> orders = GetOrders(email, day);
            foreach (ngOrderModel order in orders) {
                ngFoodItem ngFoodItem = FoodManager.Inst.GetFoodById(day, order.FoodId);
                res.Add(ngFoodItem);
            }
            return res;
        }
    }
}