using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Client;
using GoogleAppsConsoleApplication;

namespace FoodApp.Common {
    public class FoodManager {
        public static FoodManager Inst = new FoodManager();

        public List<ngFoodItem> GetFoods(int day) {
            List<ngFoodItem> res = new List<ngFoodItem>();
            ExcelTable excelTable = ExcelParser.Inst.Doc.GetExcelTable(day);
            List<ExcelRow> rows = excelTable.Rows;
            foreach (ExcelRow row in rows) {
                if (row.HasPrice) {
                    ngFoodItem item = new ngFoodItem();
                    Debug.Assert(!string.IsNullOrEmpty(row.Name));
                    item.Name = row.Name;
                    item.Description = row.Description;
                    item.Category = EFoodCategories.GetCategory(row);
                    item.IsByWeight = row.IsByWeight();
                    item.Price = row.Price;
                    item.FoodId = row.GetFoodId();
                    res.Add(item);
                }
            }
            return res;
        }

        public List<ngFoodItem> GetAllFoods() {
            List<ngFoodItem> res = new List<ngFoodItem>();
            for (int i = 1; i <= 5; ++i) {
                List<ngFoodItem> items = GetFoods(i);
                foreach (ngFoodItem item in items) {
                    if (!HasFood(res, item)) {
                        res.Add(item);
                    }
                }
            }
            return res;
        }

        private bool HasFood(List<ngFoodItem> items, ngFoodItem arg) {
            bool res = false;
            foreach (ngFoodItem item in items) {
                Debug.Assert(!string.IsNullOrEmpty(item.Name));
                Debug.Assert(!string.IsNullOrEmpty(arg.Name));
                if (item.Name.Equals(arg.Name, StringComparison.OrdinalIgnoreCase)) {
                    res = true;
                    break;
                }
            }
            return res;
        }

        public ngFoodItem GetFoodById(int dayOfWeek, string foodId) {
            ngFoodItem res = null;
            List<ngFoodItem> foods = GetFoods(dayOfWeek);
            foreach (ngFoodItem item in foods) {
                if (item.FoodId.Equals(foodId)) {
                    res = item;
                    break;
                }
            }
            return res;
        }
    }
}