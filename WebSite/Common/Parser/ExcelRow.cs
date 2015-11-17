using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public string GetFoodId() {
            string res = "";
            if (!string.IsNullOrEmpty(Category)) {
                res = new Regex(@"\W").Replace(Category, "");
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

        private string GetCategory() {
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

        public void Parse(ref string category) {
            string tmpCategory = GetCategory();
            if (!string.IsNullOrEmpty(tmpCategory)) {
                category = tmpCategory.Replace(":", "");
            }
            Category = category;

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

            if (Category.Contains("Налисники") && !Name.Contains("Контейнери"))
            {
                Name = Category + " " + Name;
            }
        }

        internal void Merge(CellEntry newEntry) {
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

        internal bool IsByWeight() {
            bool res = false;
            if (this.Category.Equals("Салати", StringComparison.OrdinalIgnoreCase)) {
                res = true;
            }
            if (this.Name.Contains("Стегна кур.запечені")||
                this.Name.Contains("Кур.філе в гриб")||
                this.Name.Contains("Буженина")||
                this.Name.Contains("Ковбаска смажена") ||
                this.Name.Contains("Телятина") ||
                this.Name.Contains("Горохове пюре") ||
                this.Name.Contains("Курка відварна") ||
                this.Name.Contains("Шашлик") ||
                this.Name.Contains("Курка відварна") ||
                this.Name.Contains("Курка відварна") ||
                this.Name.Contains("Курка відварна") ||
                this.Name.Contains("Печінка з цибулею"))
            {
                res = true;
            }

            return res;
        }
    }
}