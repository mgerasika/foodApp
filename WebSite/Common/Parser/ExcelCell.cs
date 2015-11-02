using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelCell
    {
        private ExcelRow _row;
        public string ColumnName { get; set; }

        public ExcelCell(ExcelRow row) {
            _row = row;
        }

        public void Update()
        {
            var entry = EnsureEntry(this.ColumnName);
            entry.Value = this.Value.ToString();
            _row.UpdateRow();
        }

        private ListEntry.Custom EnsureEntry(string columnName)
        {
            var cell = GetEntry(columnName);
            if (null == cell)
            {
                cell = new ListEntry.Custom();
                cell.LocalName = columnName;
                _row.GetEntry().Elements.Add(cell);
            }
            return cell;
        }

        private ListEntry.Custom GetEntry( string columnName) {
            ListEntry xmlRow = _row.GetEntry();
            ListEntry.Custom res = null;
            for (var j = 0; j < xmlRow.Elements.Count; ++j)
            {
                var xmlCell = xmlRow.Elements[j];
                var cellName = xmlCell.LocalName;
                if (cellName.Equals(columnName))
                {
                    res = xmlCell;
                    break;
                }
            }
            return res;
        }

        public decimal Value { get; set; }
    }
}