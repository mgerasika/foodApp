using System.Collections.Generic;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelTable
    {
        private readonly WorksheetEntry _entry;
        public List<ExcelRow> Rows = new List<ExcelRow>();
        private ExcelDoc _doc;
        private CellFeed _cellFeed;


        public CellFeed GetCellFeed() {
            return _cellFeed;
        }

        public ExcelDoc GetParent() {
            return _doc;
        }

        public ExcelTable(int day,ExcelDoc doc, WorksheetEntry entry) {
            _entry = entry;
            _doc = doc;
            DayOfWeek = day;
        }

        public string Title { get; set; }
        public int DayOfWeek { get; set; }

        public WorksheetEntry GetEntry() {
            return _entry;
        }

        public ExcelRow GetRowByFoodId(string foodId) {
            ExcelRow res = null;
            foreach (ExcelRow row in Rows) {
                if (row.GetFoodId().Equals(foodId)) {
                    res = row;
                    break;
                }
            }
            return res;
        }


        public void Parse() {
            Title = _entry.Title.Text;

            CellQuery cellQuery = new CellQuery(_entry.CellFeedLink);
            CellFeed cellFeed = ExcelParser.Inst.SpreadsheetsService.Query(cellQuery);
            _cellFeed = cellFeed;

            Dictionary<uint, List<CellEntry>> groupRows = new Dictionary<uint, List<CellEntry>>();
            foreach (CellEntry cell in cellFeed.Entries) {
                if (!groupRows.ContainsKey(cell.Row)) {
                    groupRows[cell.Row] = new List<CellEntry>();
                }
                groupRows[cell.Row].Add(cell);
            }

            string category = "";
            foreach (KeyValuePair<uint, List<CellEntry>> row in groupRows) {
                ExcelRow excelRow = new ExcelRow(this,row.Key, row.Value);
                Rows.Add(excelRow);

                excelRow.Parse(ref category);
            }
        }
    }
}