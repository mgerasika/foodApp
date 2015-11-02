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

        public List<ngOrderModel> GetOrders(string userId,int day)
        {
            List<ngOrderModel> res = new List<ngOrderModel>();
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            List<ExcelRow> rows = excelTable.Rows;
            foreach (ExcelRow row in rows) {
                ExcelCell cell = row.GetCell(userId);
                if (cell != null && row.HasPrice && cell.Value>0) {
                    ngOrderModel item = new ngOrderModel();
                    item.Count = cell.Value;
                    item.FoodId = row.RowId;
                    res.Add(item);
                }
            }

            return res;
        }

        public bool Buy(string userId,int day,string rowId)
        {
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowById(rowId);
            ExcelCell cell = row.EnsureCell(userId);
            cell.Value += 1;

            cell.Update();
            return true;
        }

        internal bool Delete(string userId,int day,string rowId)
        {
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowById(rowId);
            ExcelCell cell = row.GetCell(userId);
            cell.Value = 0;
            cell.Update();
            return true;
        }

        internal void CompleteOrder(string userId, int day)
        {
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            List<ExcelRow> rows = excelTable.Rows;
            foreach (ExcelRow row in rows)
            {
                ExcelCell cell = row.GetCell(userId);
                if (cell != null && row.HasPrice && cell.Value > 0)
                {
                   //Code should be here!!!
                }
            }
        }
    }
}