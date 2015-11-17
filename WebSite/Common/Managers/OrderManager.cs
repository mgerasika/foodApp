using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Client;
using GoogleAppsConsoleApplication;

namespace FoodApp.Common
{
    public class OrderManager 
    {
        public static OrderManager Inst = new OrderManager();

        public List<ngOrderModel> GetOrders(string email,int day)
        {
            List<ngOrderModel> res = new List<ngOrderModel>();
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            List<ExcelRow> rows = excelTable.Rows;
            foreach (ExcelRow row in rows) {
                ngUserModel user = UsersManager.Inst.GetUserByEmail(email);
                ExcelCell cell = row.GetCell(user.Column);
                if (cell != null && row.HasPrice && cell.Value>0) {
                    ngOrderModel item = new ngOrderModel();
                    item.Count = cell.Value;
                    item.FoodId = row.GetFoodId();
                    res.Add(item);
                }
            }

            return res;
        }

        public bool Buy(string email,int day,string foodId,decimal val)
        {
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowByFoodId(foodId);

            ngUserModel user = UsersManager.Inst.GetUserByEmail(email);
            ExcelCell cell = row.EnsureCell(user.Column);
            cell.Value += val;
            cell.Update();
            return true;
        }

        internal bool Delete(string email,int day,string rowId)
        {
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowByFoodId(rowId);
            ngUserModel user = UsersManager.Inst.GetUserByEmail(email);
            ExcelCell cell = row.GetCell(user.Column);
            cell.Value = 0;
            cell.Update();
            return true;
        }
    }
}