using System.Collections.Generic;
using FoodApp.Client;
using GoogleAppsConsoleApplication;

namespace FoodApp.Common
{
    public class FoodManager
    {
        public static FoodManager Inst = new FoodManager();

        public List<ngFoodItem> GetFoods(string userId,int day) {
            var res = new List<ngFoodItem>();
            var excelTable = ExcelManager.Inst.Doc.GetExcelTable(day);
            var rows = excelTable.Rows;
            foreach (var row in rows) {
                if (row.HasPrice) {
                    var item = new ngFoodItem();
                    item.Name = row.Name;
                    item.Description = row.Description;
                    item.Category = row.Category;
                    item.Price = row.Price;
                    item.FoodId = row.RowId;
                    res.Add(item);
                }
            }
            return res;
        }
    }
}