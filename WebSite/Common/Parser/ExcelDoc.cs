using System.Collections.Generic;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelDoc
    {
        private readonly SpreadsheetEntry _entry;
        public List<ExcelTable> Tables = new List<ExcelTable>();

        public ExcelDoc(SpreadsheetEntry entry) {
            _entry = entry;
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

        private WorksheetEntry GetWorksheetEntry(SpreadsheetEntry entry, int day) {
            WorksheetFeed wsFeed = entry.Worksheets;
            WorksheetEntry res = null;
            res = wsFeed.Entries[day] as WorksheetEntry;
            return res;
        }

        public void Parse() {
            for (int i = 0; i < 5; ++i) {
                WorksheetEntry worksheetEntry = GetWorksheetEntry(_entry, i);

                ExcelTable table = new ExcelTable(i,this, worksheetEntry);
                table.Title = worksheetEntry.Title.Text;
                ExcelParser.Inst.Doc.Tables.Add(table);
                table.Parse();
            }
        }
    }
}