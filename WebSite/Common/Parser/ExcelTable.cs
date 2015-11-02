using System.Collections.Generic;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelTable
    {
        private WorksheetEntry _entry;
        public List<ExcelRow> Rows = new List<ExcelRow>();

        public string Title { get; set; }

        public ExcelTable(int day, WorksheetEntry entry) {
            _entry = entry;
            DayOfWeek = day;
        }

        internal ExcelRow GetRowById(string rowId) {
            ExcelRow res = null;
            foreach (ExcelRow row in Rows) {
                if (row.RowId.Equals(rowId)) {
                    res = row;
                    break;
                }
            }
            return res;
        }

        public int DayOfWeek { get; set; }
    }
}