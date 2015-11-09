using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Client;
using GoogleAppsConsoleApplication;

namespace FoodApp.Common
{
    public class FoodManager
    {
        public static FoodManager Inst = new FoodManager();

        public List<ngFoodItem> GetFoods(int day) {
            var res = new List<ngFoodItem>();
            var excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            var rows = excelTable.Rows;
            foreach (var row in rows) {
                if (row.HasPrice) {
                    var item = new ngFoodItem();
                    Debug.Assert(!string.IsNullOrEmpty(row.Name));
                    item.Name = row.Name;
                    item.Description = row.Description;
                    item.Category = row.Category;
                    item.Price = row.Price;
                    item.RowId = row.RowId;
                    item.FoodId = row.GetFoodId();
                    res.Add(item);
                }
            }
            return res;
        }

        public List<ngFoodItem> GetAllFoods() {
            var res = new List<ngFoodItem>();
            for (var i = 1; i <= 5; ++i) {
                var items = GetFoods(i);
                foreach (var item in items) {
                    if (!HasFood(res, item)) {
                        res.Add(item);
                    }
                }
            }
            return res;
        }

        private bool HasFood(List<ngFoodItem> items, ngFoodItem arg) {
            var res = false;
            foreach (var item in items) {
                Debug.Assert(!string.IsNullOrEmpty(item.Name));
                Debug.Assert(!string.IsNullOrEmpty(arg.Name));
                if (item.Name.Equals(arg.Name, StringComparison.OrdinalIgnoreCase)) {
                    res = true;
                    break;
                }
            }
            return res;
        }
    }
}