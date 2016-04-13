using Google.GData.Spreadsheets;

namespace FoodApp.Common.Parser {
    public class ExcelCell {
        private readonly ExcelRow _row;
        private CellEntry _entry;

        public ExcelCell(ExcelRow row, CellEntry entry) {
            _entry = entry;
            _row = row;

            if (null != entry) {
                Column = entry.Column;
                Row = entry.Row;
                decimal val;
                if (ApiUtils.TryDecimalParse(entry.NumericValue, out val)) {
                    Value = val;
                }
            }
        }

        public uint Column { get; set; }
        public uint Row { get; set; }
        public decimal Value { get; set; }
        public decimal EditTmpValue { get; set; }

        public string GetBatchID() {
            return string.Format("R{0}C{1}", Row, Column);
        }

        public ExcelRow GetRow() {
            return _row;
        }


        public void Parse() {
            decimal lPrice = 0;
            if (ApiUtils.TryDecimalParse(_entry.Value, out lPrice)) {
                Value = lPrice;
            }
        }

        internal void SetEntry(CellEntry entry) {
            _entry = entry;
            _row.MergeEntry(entry);
        }

        internal CellEntry GetEntry()
        {
            return _entry;
        }
    }
}