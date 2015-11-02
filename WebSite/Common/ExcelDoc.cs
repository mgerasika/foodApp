using System.Collections.Generic;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelDoc
    {
        public List<ExcelTable> Tables = new List<ExcelTable>(); 
        private SpreadsheetEntry _entry;
        public ExcelDoc(SpreadsheetEntry entry) {
            _entry = entry;

            for (int i = 0; i < 5; ++i) {
                ExcelTable tbl = new ExcelTable(i+1, GetWorksheetEntry(i+1));
                Tables.Add(tbl);
            }
        }

        private WorksheetEntry GetWorksheetEntry(int day)
        {
            var wsFeed = _entry.Worksheets;
            WorksheetEntry res = null;
            res = wsFeed.Entries[day - 1] as WorksheetEntry;
            return res;
        }

        public ExcelTable GetExcelTable(int day) {
            ExcelTable res = null;
            foreach (ExcelTable tbl in Tables) {
                if (tbl.DayOfWeek.Equals(day)) {
                    res = tbl;
                    break;
                }
            }
            return res;
        }
    }
}