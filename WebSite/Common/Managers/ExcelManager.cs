using System;
using System.Diagnostics;
using System.Text;
using System.Web;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelManager
    {
        public static ExcelManager Inst = new ExcelManager();
        private SpreadsheetsService _SpreadsheetsService = null;
        private readonly string CLIENT_ID = "668583993597.apps.googleusercontent.com";
        private readonly string CLIENT_SECRET = "70LRXGzVw-G1t5bzRmdUmcoj";
        private readonly string REDIRECT_URI = "http://localhost:15845/Home/Test2";
        private readonly string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
        private OAuth2Parameters _parameters;
        public ExcelDoc Doc { get; set; }
        public SpreadsheetsService SpreadsheetsService { get; set; }

        public string StartParseExcel() {
            _parameters = new OAuth2Parameters();
            _parameters.ClientId = CLIENT_ID;
            _parameters.ClientSecret = CLIENT_SECRET;
            _parameters.RedirectUri = REDIRECT_URI;
            _parameters.Scope = SCOPE;
            //parameters.ApprovalPrompt = "force";
            _parameters.TokenExpiry = DateTime.MaxValue;
            _parameters.AccessType = "offline";
            var authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(_parameters);
            HttpContext.Current.Response.Redirect(authorizationUrl);
            return authorizationUrl;
        }

        public void RefreshAccessToken() {
            OAuthUtil.RefreshAccessToken(_parameters);
        }

        public string EndParseExcel() {
            var sb = new StringBuilder();

            if (null == Inst.SpreadsheetsService) {
                _parameters.AccessCode = HttpContext.Current.Request.QueryString["code"];
                OAuthUtil.GetAccessToken(_parameters);
                var accessToken = _parameters.AccessToken;
                if (string.IsNullOrEmpty(_parameters.RefreshToken)) {
                    //changes every time!!!
                    _parameters.RefreshToken = "1/jWXrc6wJ4lUmWscyc3rozkWPKdFwvha43JfPCxMksus";
                }

                Console.WriteLine("OAuth Access Token: " + accessToken);
                var requestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", _parameters);
                Inst.SpreadsheetsService = new SpreadsheetsService("MySpreadsheetIntegration-v1");
                Inst.SpreadsheetsService.RequestFactory = requestFactory;
                sb.AppendFormat("<div>refresh token={0}</div>", _parameters.RefreshToken);
            }

            var service = Inst.SpreadsheetsService;
            var query = new SpreadsheetQuery();
            var feed = service.Query(query);

            var entry = GetSpreadsheetEntry(feed);
            Debug.Assert(null != entry);
            sb.Append("<h1>" + entry.Title.Text + "</h1>");
            RenderWeek(entry, sb);

            return sb.ToString();
        }

        private SpreadsheetEntry GetSpreadsheetEntry(SpreadsheetFeed feed) {
            var res = new SpreadsheetEntry();
            foreach (SpreadsheetEntry entry in feed.Entries) {
                if (entry.Title.Text.Equals("mymenutest")) {
                    res = entry;
                    break;
                }
            }
            return res;
        }

        private WorksheetEntry GetWorksheetEntry(SpreadsheetEntry entry, int day) {
            var wsFeed = entry.Worksheets;
            WorksheetEntry res = null;
            res = wsFeed.Entries[day - 1] as WorksheetEntry;
            return res;
        }

        private ListFeed GetListFeed(WorksheetEntry entry2) {
            // Define the URL to request the list feed of the worksheet.
            var listFeedLink = entry2.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            var listQuery = new ListQuery(listFeedLink.HRef.ToString());
            var listFeed = Inst.SpreadsheetsService.Query(listQuery);
            return listFeed;
        }

        private void RenderWeek(SpreadsheetEntry entry, StringBuilder sb) {
            Doc = new ExcelDoc(entry);

            for (var i = 0; i < 5; ++i) {
                WorksheetEntry worksheetEntry = GetWorksheetEntry(entry, i + 1);
                renderTable(worksheetEntry, sb, Inst.Doc.GetExcelTable(i + 1));
            }
        }

        private void renderTable(WorksheetEntry workEntry, StringBuilder sb, ExcelTable excelTable) {
            var listFeed = GetListFeed(workEntry);
            excelTable.Title = workEntry.Title.Text;

                if (listFeed.Entries.Count > 0) {
                sb.Append("<table border=1>");
                for (var i = 0; i < listFeed.Entries.Count; ++i) {
                    var row = listFeed.Entries[i] as ListEntry;
                    sb.Append("<tr>");
                    for (var j = 0; j < row.Elements.Count; ++j) {
                        var element = row.Elements[j];
                        sb.AppendFormat("<td><b>{0}</b></td>", element.Value);
                    }
                    sb.Append("</tr>");
                    break;
                }

                    string category = "";
                for (var i = 1; i < listFeed.Entries.Count; ++i) {
                    var xmlRow = listFeed.Entries[i] as ListEntry;

                    var excelRow = new ExcelRow(excelTable, xmlRow);
                    excelRow.RowId = Guid.NewGuid().ToString();
                    excelRow.InternalId = GetXmlRowId(xmlRow);
                    excelTable.Rows.Add(excelRow);

                    sb.Append("<tr>");


                    string tmpCategory = GetCategory(xmlRow);
                    if (!string.IsNullOrEmpty(tmpCategory)) {
                        category = tmpCategory;
                    }
                    excelRow.Category = category;

                    for (var j = 0; j < xmlRow.Elements.Count; ++j) {
                        var element = xmlRow.Elements[j];
                        var columnName = element.LocalName;
                        sb.AppendFormat("<td>{0}<b>{1}</b></td>", element.Value, columnName);

                        if (columnName.Equals(ColumnNames.Description)) {
                            excelRow.Description = element.Value;
                        }
                        else if (columnName.Equals(ColumnNames.Name)) {
                            excelRow.Name = element.Value;
                        }
                        else if (columnName.Equals(ColumnNames.Price)) {
                            var price = element.Value;
                            if (!string.IsNullOrEmpty(price)) {
                                var str = price;
                                str = str.Replace("грн.", "");
                                str = str.Replace("грн ", "");
                                decimal lPrice = 0;
                                if (decimal.TryParse(str, out lPrice)) {
                                    excelRow.Price = lPrice;
                                    excelRow.HasPrice = true;
                                }
                            }
                        }
                        else {
                            var cell = new ExcelCell(excelRow);
                            excelRow.Cells.Add(cell);
                            cell.ColumnName = columnName;
                            decimal lPrice = 0;
                            if (decimal.TryParse(element.Value, out lPrice)) {
                                cell.Value = lPrice;
                            }
                        }
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
            }
        }

        private static string GetCategory(ListEntry xmlRow) {
            string res = "";

            bool hasCaption = false;
            for (var j = 0; j < xmlRow.Elements.Count; ++j) {
                var element = xmlRow.Elements[j];
                var columnName = element.LocalName;
                if (columnName.Equals(ColumnNames.Price)) {
                    string val = element.Value;
                    if (!string.IsNullOrEmpty(val) && val.ToLower().Contains("ціна")) {
                        hasCaption = true;
                        break;
                    }
                }
            }

            if (hasCaption) {
                for (var j = 0; j < xmlRow.Elements.Count; ++j) {
                    var element = xmlRow.Elements[j];
                    var columnName = element.LocalName;
                    if (columnName.Equals(ColumnNames.Name)) {
                        res = element.Value;
                        break;
                    }
                }
            }
            return res;
        }

        public void AddCellValue(int dayOfWeek, string rowId, string columnName, object value) {
            var xmlRow = GetRow(dayOfWeek, rowId);
            Debug.Assert(null != xmlRow);
            var xmlCell = GetCell(xmlRow, columnName);
            if (null == xmlCell) {
                xmlCell = new ListEntry.Custom();
                xmlCell.LocalName = columnName;
                xmlRow.Elements.Add(xmlCell);
            }
            xmlCell.Value = value.ToString();

            xmlRow.Update();
        }

        public string GetCellValue(int dayOfWeek, string rowId, string columnName) {
            var res = "";
            var xmlRow = GetRow(dayOfWeek, rowId);
            Debug.Assert(null != xmlRow);
            var xmlCell = GetCell(xmlRow, columnName);
            if (null != xmlCell) {
                res = xmlCell.Value;
            }
            return res;
        }

        private ListEntry.Custom GetCell(ListEntry xmlRow, string columnName) {
            ListEntry.Custom res = null;
            for (var j = 0; j < xmlRow.Elements.Count; ++j) {
                var xmlCell = xmlRow.Elements[j];
                var cellName = xmlCell.LocalName;
                if (cellName.Equals(columnName)) {
                    res = xmlCell;
                    break;
                }
            }
            return res;
        }

        private string GetXmlRowId(ListEntry row) {
            return row.Id.AbsoluteUri;
        }

        private ListEntry GetRow(int dayOfWeek, string rowId) {
            ListEntry res = null;

            var excelTable = Doc.GetExcelTable(dayOfWeek);
            var excelRow = excelTable.GetRowById(rowId);
            var internalRowId = excelRow.InternalId;

            var service = Inst.SpreadsheetsService;
            var query = new SpreadsheetQuery();
            var feed = service.Query(query);

            var xmlDoc = GetSpreadsheetEntry(feed);
            var xmlPage = GetWorksheetEntry(xmlDoc, dayOfWeek);
            var xmlTable = GetListFeed(xmlPage);
            for (var i = 1; i < xmlTable.Entries.Count; ++i) {
                var xmlRow = xmlTable.Entries[i] as ListEntry;
                Debug.Assert(null != xmlRow);
                if (internalRowId.Equals(GetXmlRowId(xmlRow))) {
                    res = xmlRow;
                    break;
                }
            }

            return res;
        }
    }
}