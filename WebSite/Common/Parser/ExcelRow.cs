using System;
using System.Collections.Generic;
using System.Diagnostics;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelRow
    {
        private ListEntry _entry = null;
        private ExcelTable _table;

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string RowId { get; set; }
        public List<ExcelCell> Cells = new List<ExcelCell>();
        public string InternalId { get; set; }

        public ExcelRow(ExcelTable table, ListEntry entry) {
            _entry = entry;
            _table = table;
        }

        internal void UpdateRow()
        {
            ExcelManager.Inst.RefreshAccessToken();

            ListEntry atomEntry = _entry.Update() as ListEntry;
            _entry = atomEntry;
        }

        internal ListEntry GetEntry() {
            return _entry;
        }

        internal ExcelCell GetCell(string columnName) {
            ExcelCell res = null;

            foreach (ExcelCell cell in this.Cells) {
                if (cell.ColumnName.Equals(columnName)) {
                    res = cell;
                    break;
                }
            }
            return res;
        }

        internal ExcelCell EnsureCell(string columnName)
        {
            ExcelCell res = GetCell(columnName);
            if (null == res) {
                res = new ExcelCell(this);
                res.ColumnName = columnName;
                this.Cells.Add(res);
            }
            return res;
        }

        public bool HasPrice { get; set; }

        public string Category { get; set; }
    }
}