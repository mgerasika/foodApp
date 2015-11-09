using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using FoodApp.Controllers;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelRow
    {
        private ListEntry _entry;
        private ExcelTable _table;
        public List<ExcelCell> Cells = new List<ExcelCell>();

        public ExcelRow(ExcelTable table, ListEntry entry) {
            _entry = entry;
            _table = table;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string RowId { get; set; }
        public bool HasPrice { get; set; }
        public string Category { get; set; }

        internal void UpdateRow() {
            ExcelParser.Inst.RefreshAccessToken();

            var atomEntry = _entry.Update() as ListEntry;
            _entry = atomEntry;
        }

        internal ListEntry GetEntry() {
            return _entry;
        }

        internal ExcelCell GetCell(string columnName) {
            ExcelCell res = null;

            foreach (var cell in Cells) {
                if (cell.ColumnName.Equals(columnName)) {
                    res = cell;
                    break;
                }
            }
            return res;
        }

        internal ExcelCell EnsureCell(string columnName) {
            var res = GetCell(columnName);
            if (null == res) {
                res = new ExcelCell(this);
                res.ColumnName = columnName;
                Cells.Add(res);
            }
            return res;
        }

        public string GetFoodId() {
            var res = "";
            if (!string.IsNullOrEmpty(Category)) {
                res = new Regex(@"\W").Replace(Category, "");
            }
            if (!string.IsNullOrEmpty(Name)) {
                res += new Regex(@"\W").Replace(Name, "");
            }
            Debug.Assert(!string.IsNullOrEmpty(res));
            res = "_" + res;
            res = ApiUtils.GetLatinCodeFromCyrillic(res);
            return res;
        }

        
    }
}