using System;
using System.Diagnostics;
using FoodApp.Controllers;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication {
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

        public ExcelRow GetRow() {
            return _row;
        }

        /*
        internal void UpdateRow() {
            ExcelParser.Inst.RefreshAccessToken();

            //CellEntry atomEntry = _entry.Update() as CellEntry;
            //Debug.Assert(null != atomEntry);
            //_entry = atomEntry;
        }
         * */

        private bool requestAddCell(ExcelCell obj) {
            WorksheetEntry worksheetEntry = GetRow().GetTable().GetEntry();
            CellQuery cellQuery = new CellQuery(worksheetEntry.CellFeedLink);
            CellFeed cellFeed = GetRow().GetTable().GetCellFeed();
            CellEntry batchEntry = GetCellEntryMap(ExcelParser.Inst.SpreadsheetsService, cellFeed, obj);

            CellFeed batchRequest = new CellFeed(cellQuery.Uri, ExcelParser.Inst.SpreadsheetsService);
            string value = obj.Value.ToString().Replace(".", ",");
            batchEntry.InputValue = value;
            batchEntry.BatchData = new GDataBatchEntryData(value, GDataBatchOperationType.update);
            batchRequest.Entries.Add(batchEntry);

            CellFeed batchResponse =
                (CellFeed) ExcelParser.Inst.SpreadsheetsService.Batch(batchRequest, new Uri(cellFeed.Batch));

            bool res = true;
            foreach (CellEntry entry in batchResponse.Entries) {
                _entry = entry;
                if (entry.BatchData.Status.Code != 200) {
                    res = false;
                }
            }
            return res;
        }

        private static CellEntry GetCellEntryMap(SpreadsheetsService service, CellFeed cellFeed, ExcelCell obj) {
            CellFeed batchRequest = new CellFeed(new Uri(cellFeed.Self), service);
            string id = string.Format("R{0}C{1}", obj.Row, obj.Column);
            CellEntry batchEntry = new CellEntry(obj.Row, obj.Column, id);
            batchEntry.Id = new AtomId(string.Format("{0}/{1}", cellFeed.Self, id));
            batchEntry.BatchData = new GDataBatchEntryData(id, GDataBatchOperationType.query);
            batchRequest.Entries.Add(batchEntry);

            CellFeed queryBatchResponse = (CellFeed) service.Batch(batchRequest, new Uri(cellFeed.Batch));
            return queryBatchResponse.Entries[0] as CellEntry;
        }

        public void Update() {
            ExcelParser.Inst.RefreshAccessToken();

            if (_entry != null) {
                try {
                    _entry.InputValue = Value.ToString().Replace(".", ",");
                    CellEntry newEntry = _entry.Update() as CellEntry;
                    _entry = newEntry;
                }
                catch (Exception ex) {
                }
            }
            else {
                requestAddCell(this);
            }
            _row.Merge(_entry);
        }

        public void Parse() {
            decimal lPrice = 0;
            if (ApiUtils.TryDecimalParse(_entry.Value, out lPrice)) {
                Value = lPrice;
            }
        }
    }
}