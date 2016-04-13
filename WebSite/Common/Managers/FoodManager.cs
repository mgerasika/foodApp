using System;
using System.Collections.Generic;
using System.Diagnostics;
using FoodApp.Common.Parser;

namespace FoodApp.Common.Managers {
    public class FoodManager {
        public static FoodManager Inst = new FoodManager();

        public List<ngFoodItem> GetFoods(int day) {
            ExcelTable excelTable = ExcelManager.Inst.Doc.GetExcelTable(day);
            List<ngFoodItem> res = GetFoodsInternal(excelTable);
            return res;
        }

        public List<ngFoodItem> GetFoodsInternal(ExcelTable excelTable) {
            List<ngFoodItem> res = new List<ngFoodItem>();
            List<ExcelRow> rows = excelTable.Rows;
            foreach (ExcelRow row in rows) {
                if (row.HasPrice && !string.IsNullOrEmpty(row.Name)) {
                    ngFoodItem item = new ngFoodItem();
                    Debug.Assert(!string.IsNullOrEmpty(row.Name));
                    item.Name = row.Name;
                    item.Description = row.Description;

                    item.isContainer = row.IsContainer();
                    item.isBigContainer = row.IsBigContainer();
                    item.isSmallContainer = row.IsSmallContainer();
                    item.isSalat = row.IsSalat();
                    item.isGarnir = row.IsGarnir();
                    item.isMeatOrFish = row.IsMeatOrFish();
                    item.isKvasolevaOrChanachi = row.IsKvasolevaOrChanachi();
                    item.isFirst = row.IsFirst();
                    item.IsByWeightItem = row.IsByWeightItem();
                    item.Price = row.Price;
                    item.FoodId = row.GetFoodId();
                    item.Category = row.Category;

                    res.Add(item);
                }
            }
            return res;
        }

        public List<ngFoodItem> GetAllFoods() {
            List<ngFoodItem> res = new List<ngFoodItem>();
            for (int i = 0; i < 5; ++i) {
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
                if (item.FoodId.Equals(arg.FoodId, StringComparison.OrdinalIgnoreCase)) {
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
                if (ApiUtils.Equals(item.FoodId,foodId)) {
                    res = item;
                    break;
                }
            }
            return res;
        }

        public ngFoodItem GetFoodById(string foodId)
        {
            ngFoodItem res = null;
            List<ngFoodItem> foods = GetAllFoods();
            foreach (ngFoodItem item in foods)
            {
                if (ApiUtils.Equals(item.FoodId,foodId))
                {
                    res = item;
                    break;
                }
            }
            return res;
        }

        public ngFoodItem GetSmallContainer(int dayOfWeek) {
            ngFoodItem res = null;
            List<ngFoodItem> foods = GetFoods(dayOfWeek);
            foreach (ngFoodItem item in foods) {
                if (item.isContainer && item.isSmallContainer) {
                    res = item;
                    break;
                }
            }
            return res;
        }

        public ngFoodItem GetBigContainer(int dayOfWeek) {
            ngFoodItem res = null;
            List<ngFoodItem> foods = GetFoods(dayOfWeek);
            foreach (ngFoodItem item in foods) {
                if (item.isContainer && item.isBigContainer) {
                    res = item;
                    break;
                }
            }
            return res;
        }

        internal bool ChangePrice(ngUserModel user, int day, string foodId, decimal val)
        {
            bool res = true;
            ExcelTable excelTable = ExcelManager.Inst.Doc.GetExcelTable(day);
            ExcelRow row = excelTable.GetRowByFoodId(foodId);
            ExcelCell cell = row.EnsureCell(ColumnNames.Price);
            decimal prevVal = cell.Value;
            cell.Value = val;

            //request update cell
            res = false;

            return res;
        }
    }
}