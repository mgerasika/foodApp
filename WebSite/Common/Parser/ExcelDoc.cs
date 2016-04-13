using System.Collections.Generic;
using Google.GData.Spreadsheets;

namespace FoodApp.Common.Parser
{
    public class ExcelDoc
    {
        private readonly SpreadsheetEntry _entry;
        public List<ExcelTable> Tables = new List<ExcelTable>();
        private SpreadsheetsService _service;

        public ExcelDoc(SpreadsheetEntry entry,SpreadsheetsService service) {
            _entry = entry;
            _service = service;
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

                ExcelTable table = new ExcelTable(i,this, worksheetEntry,_service);
                table.Title = worksheetEntry.Title.Text;
                this.Tables.Add(table);
                table.Parse();
            }
        }
    }
}