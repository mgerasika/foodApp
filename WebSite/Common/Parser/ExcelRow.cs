using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FoodApp.Common;
using FoodApp.Controllers;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication {
    public class ExcelRow {
        private readonly List<CellEntry> _entry;
        private readonly ExcelTable _table;
        public List<ExcelCell> Cells = new List<ExcelCell>();


        public ExcelRow(ExcelTable table, uint row, List<CellEntry> entry) {
            _entry = entry;
            _table = table;
            Row = row;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        private uint Row { get; set; }
        public bool HasPrice { get; set; }
        public string OriginalCategory { get; set; }
        public string Category { get; set; }

        public ExcelTable GetTable() {
            return _table;
        }

        internal ExcelCell GetCell(uint column) {
            ExcelCell res = null;

            foreach (ExcelCell cell in Cells) {
                if (cell.Column.Equals(column)) {
                    res = cell;
                    break;
                }
            }
            return res;
        }

        internal ExcelCell GetCellByBatchId(string batchId) {
            ExcelCell res = null;

            foreach (ExcelCell cell in Cells) {
                if (cell.GetBatchID().Equals(batchId)) {
                    res = cell;
                    break;
                }
            }
            return res;
        }

        public static ExcelCell GetCellByBatchId(List<ExcelCell> cells, string batchId) {
            ExcelCell res = null;

            foreach (ExcelCell cell in cells) {
                if (cell.GetBatchID().Equals(batchId)) {
                    res = cell;
                    break;
                }
            }
            return res;
        }

        public string GetFoodId() {
            string res = "";
            if (!string.IsNullOrEmpty(OriginalCategory)) {
                res = new Regex(@"\W").Replace(OriginalCategory, "");
            }
            if (!string.IsNullOrEmpty(Name)) {
                res += new Regex(@"\W").Replace(Name, "");
            }
            if (!string.IsNullOrEmpty(res)) {
                res = "_" + res;
                res = ApiUtils.GetLatinCodeFromCyrillic(res);
            }
            return res;
        }


        internal ExcelCell EnsureCell(uint column) {
            ExcelCell cell = GetCell(column);
            if (null == cell) {
                cell = new ExcelCell(this, null);
                cell.Column = column;
                cell.Row = Row;
                Cells.Add(cell);
            }
            return cell;
        }

        private string ParseCategory() {
            string res = "";

            bool hasCaption = false;
            for (int j = 0; j < _entry.Count; ++j) {
                CellEntry element = _entry[j];
                if (element.Column.Equals(ColumnNames.Price)) {
                    string val = element.Value;
                    if (!string.IsNullOrEmpty(val) && val.ToLower().Contains("ціна")) {
                        hasCaption = true;
                        break;
                    }
                }
            }

            if (hasCaption) {
                for (int j = 0; j < _entry.Count; ++j) {
                    CellEntry element = _entry[j];
                    if (element.Column.Equals(ColumnNames.Name)) {
                        res = element.Value;
                        break;
                    }
                }
            }
            return res;
        }

        public void Parse(ref string lCategory) {
            string tmpCategory = ParseCategory();
            if (!string.IsNullOrEmpty(tmpCategory)) {
                lCategory = tmpCategory.Replace(":", "");
            }
            OriginalCategory = lCategory;

            for (int j = 0; j < _entry.Count; ++j) {
                CellEntry cell = _entry[j];

                if (cell.Column.Equals(ColumnNames.Description)) {
                    Description = cell.Value;
                }
                else if (cell.Column.Equals(ColumnNames.Name)) {
                    Name = cell.Value;
                }
                else if (cell.Column.Equals(ColumnNames.Price)) {
                    string price = cell.Value;
                    if (!string.IsNullOrEmpty(price)) {
                        string str = price;

                        decimal lPrice = 0;
                        if (ApiUtils.TryDecimalParse(str, out lPrice)) {
                            Price = lPrice;
                            HasPrice = true;
                        }
                    }
                }
                else {
                    ExcelCell excelCell = new ExcelCell(this, cell);
                    Cells.Add(excelCell);
                    excelCell.Parse();
                }
            }


            if (OriginalCategory != null && Name != null && OriginalCategory.Contains("Налисники") &&
                !Name.Contains("Контейнери")) {
                Name = OriginalCategory + " " + Name;
            }
        }

        public static string GetNewCategory(string category, string name) {
            string res = category;
            if (category.Equals("Салати", StringComparison.OrdinalIgnoreCase)) {
                res = EFoodCategories.Salat;
            }

            else if (category.Equals("Гарніри", StringComparison.OrdinalIgnoreCase)) {
                res = EFoodCategories.Garnir;
            }

            else if (category.Equals("Перші страви", StringComparison.OrdinalIgnoreCase)) {
                res = EFoodCategories.First;
            }
            else if (name != null && name.Contains("Контейнери")) {
                res = EFoodCategories.PlactisContainer;
            }
            else if (name != null && name.Contains("батон")) {
                res = EFoodCategories.Breat;
            }
            else if (category.Contains("Комплексний")) {
                res = EFoodCategories.ComplexDinner;
            }
            else if (name != null && name.Contains("Налисники")) {
                res = EFoodCategories.Garnir;
            }

            else {
                res = EFoodCategories.MeatOrFish;
            }

            return res;
        }

        internal void MergeEntry(CellEntry newEntry) {
            CellEntry old = null;
            foreach (CellEntry obj in _entry) {
                if (obj.Column == newEntry.Column && obj.Row == newEntry.Row) {
                    old = obj;
                    break;
                }
            }

            if (null != old) {
                _entry.Remove(old);
            }
            _entry.Add(newEntry);
        }

        public bool IsByWeightItem() {
            bool res = false;
            if (Category.Equals(EFoodCategories.Salat, StringComparison.OrdinalIgnoreCase)) {
                res = true;
            }
            if (Name.Contains("Стегна кур.запечені") ||
                Name.Contains("Кур.філе в гриб") ||
                Name.Contains("Буженина") ||
                Name.Contains("Ковбаска смажена") ||
                Name.Contains("Телятина") ||
                Name.Contains("Горохове пюре") ||
                Name.Contains("Курка відварна") ||
                Name.Contains("Шашлик") ||
                Name.Contains("Курка відварна") ||
                Name.Contains("Курка відварна") ||
                Name.Contains("Курка відварна") ||
                Name.Contains("Печінка з цибулею")) {
                res = true;
            }

            return res;
        }

        public bool IsFirst() {
            bool res = false;
            if (Category.Equals(EFoodCategories.First)) {
                res = true;
            }
            return res;
        }

        public bool IsMeatOrFish() {
            bool res = false;
            if (Category.Equals(EFoodCategories.MeatOrFish)) {
                res = true;
            }
            return res;
        }

        public bool IsSalat() {
            bool res = false;
            if (Category.Equals(EFoodCategories.Salat)) {
                res = true;
            }
            return res;
        }

        public bool IsGarnir() {
            bool res = false;
            if (Category.Equals(EFoodCategories.Garnir)) {
                res = true;
            }
            return res;
        }

        public bool IsKvasolevaOrChanachi() {
            return Name.Contains(EFoodCategories.Kvasoleva) || Name.Contains(EFoodCategories.Chanachi);
        }

        public bool IsContainer() {
            bool res = false;
            if (Name.Contains(EFoodCategories.PlactisContainer)) {
                res = true;
            }
            return res;
        }

        public bool IsSmallContainer() {
            bool res = false;
            if (Name.Contains(EFoodCategories.PlactisContainer) && Name.Contains("1")) {
                res = true;
            }
            return res;
        }

        public bool IsBigContainer() {
            bool res = false;
            if (Name.Contains(EFoodCategories.PlactisContainer) && Name.Contains("2")) {
                res = true;
            }
            return res;
        }
    }
}