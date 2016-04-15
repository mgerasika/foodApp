using System.Collections.Generic;
using System.Diagnostics;
using Google.GData.Spreadsheets;

namespace FoodApp.Common.Parser {
    public class ExcelDoc {
        private readonly SpreadsheetEntry _entry;
        private readonly SpreadsheetsService _service;
        public List<ExcelTable> Tables = new List<ExcelTable>();


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

        public void Parse() {
            for (int i = 0; i < 5; ++i) {
                WorksheetEntry worksheetEntry = GetWorksheetEntry(_entry, i);

                ExcelTable table = new ExcelTable(i, this, worksheetEntry, _service);
                table.Title = worksheetEntry.Title.Text;
                Tables.Add(table);
                table.Parse();
            }

            ChangeNames();
        }


        public ExcelDoc(SpreadsheetEntry entry, SpreadsheetsService service) {
            _entry = entry;
            _service = service;
        }

        private WorksheetEntry GetWorksheetEntry(SpreadsheetEntry entry, int day) {
            WorksheetFeed wsFeed = entry.Worksheets;
            WorksheetEntry res = null;
            res = wsFeed.Entries[day] as WorksheetEntry;
            return res;
        }

        private void ChangeNames() {
            for (int i = 0; i < 5; i++) {
                List<ExcelRow> rows1 = GetExcelTable(i).Rows;

                foreach (ExcelRow row1 in rows1) {
                    if (!string.IsNullOrEmpty(row1.Name) && row1.HasPrice) {
                        for (int j = i + 1; j < 5; j++) {
                            List<ExcelRow> rows2 = GetExcelTable(j).Rows;

                            foreach (ExcelRow row2 in rows2) {
                                if (!string.IsNullOrEmpty(row2.Name) && row2.HasPrice) {
                                    if (ApiUtils.IsSeamsFoodIds(row1.GetFoodId(), row2.GetFoodId()) && !row1.Name.Equals(row2.Name)) {
                                        row2.Name = row1.Name;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}