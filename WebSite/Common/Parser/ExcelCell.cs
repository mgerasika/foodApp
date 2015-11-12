using System.Diagnostics;
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
            Debug.Assert(false);
            /*
            ListEntry.Custom entry = EnsureEntry(this.ColumnName);
            entry.Value = this.Value.ToString().Replace(".",",");
            _row.UpdateRow();
             * */
        }

        private ListEntry.Custom EnsureEntry(string columnName)
        {
            ListEntry.Custom cell = GetEntry(columnName);
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
            for (int j = 0; j < xmlRow.Elements.Count; ++j)
            {
                ListEntry.Custom xmlCell = xmlRow.Elements[j];
                string cellName = xmlCell.LocalName;
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