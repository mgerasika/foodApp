using System;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Text;
using System.Text.RegularExpressions;
using FoodApp.Controllers;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class ExcelParser
    {
        //private const string c_sExcelFileName = "mymenutest";
        private const string c_sExcelFileName = "ћеню на тиждень";

        private static object _lockObj = new object();
        public static ExcelParser Inst = new ExcelParser();
        private OAuth2Parameters _parameters;
        private SpreadsheetsService _SpreadsheetsService = null;
      
          public ExcelDoc Doc { get; set; }
        public SpreadsheetsService SpreadsheetsService { get; set; }

        /*
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
         * */

        /* public string EndParseExcel() {
            var sb = new StringBuilder();

            if (null == Inst.SpreadsheetsService) {
                _parameters.AccessCode = HttpContext.Current.Request.QueryString["code"];
                OAuthUtil.GetAccessToken(_parameters);
                var accessToken = _parameters.AccessToken;
                //		accessToken	"ya29.IAK0DqyyBCLy1lNBPHM4ON0m74HiPnXdJnizHC2a7w5CIDndeLPuFbJm0u34HfKpiXZ8UQ";

                if (string.IsNullOrEmpty(_parameters.RefreshToken)) {
                    //changes every time!!!
                    _parameters.RefreshToken = "1/jWXrc6wJ4lUmWscyc3rozkWPKdFwvha43JfPCxMksus";
                }

                var requestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", _parameters);
                Inst.SpreadsheetsService = new SpreadsheetsService("MySpreadsheetIntegration-v1");
                Inst.SpreadsheetsService.RequestFactory = requestFactory;
                sb.AppendFormat("<div>access code={0}</div>", _parameters.AccessCode);
                sb.AppendFormat("<div>access token={0}</div>", _parameters.AccessToken);
                sb.AppendFormat("<div>refresh token={0}</div>", _parameters.RefreshToken);
            }*/


        public void RefreshAccessToken() {
            OAuthUtil.RefreshAccessToken(_parameters);
        }

        public string Init() {
            StringBuilder sb = new StringBuilder();

            lock (_lockObj) {
                if (null == Inst.SpreadsheetsService) {
                    _parameters = new OAuth2Parameters();
                    _parameters.ClientId = ApiUtils.CLIENT_ID;
                    _parameters.ClientSecret = ApiUtils.CLIENT_SECRET;
                    _parameters.Scope = ApiUtils.SCOPE;
                    _parameters.RedirectUri = ApiUtils.REDIRECT_URL;
                    _parameters.TokenExpiry = DateTime.MaxValue;
                    _parameters.AccessType = "offline";
                    _parameters.AccessToken = "ya29.IAK0DqyyBCLy1lNBPHM4ON0m74HiPnXdJnizHC2a7w5CIDndeLPuFbJm0u34HfKpiXZ8UQ";
                    _parameters.RefreshToken = "1/jWXrc6wJ4lUmWscyc3rozkWPKdFwvha43JfPCxMksus";

                    GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", _parameters);
                    Inst.SpreadsheetsService = new SpreadsheetsService("MySpreadsheetIntegration-v1");
                    Inst.SpreadsheetsService.RequestFactory = requestFactory;
                    sb.AppendFormat("<div>access code={0}</div>", _parameters.AccessCode);
                    sb.AppendFormat("<div>access token={0}</div>", _parameters.AccessToken);
                    sb.AppendFormat("<div>refresh token={0}</div>", _parameters.RefreshToken);

                    SpreadsheetsService service = Inst.SpreadsheetsService;
                    SpreadsheetQuery query = new SpreadsheetQuery();
                    SpreadsheetFeed feed = service.Query(query);

                    SpreadsheetEntry entry = GetSpreadsheetEntry(feed);
                    Debug.Assert(null != entry);
                    sb.Append("<h1>" + entry.Title.Text + "</h1>");
                    RenderWeek(entry, sb);
                }
            }
            return sb.ToString();
        }
       

        private SpreadsheetEntry GetSpreadsheetEntry(SpreadsheetFeed feed) {
            SpreadsheetEntry res = new SpreadsheetEntry();
            foreach (SpreadsheetEntry entry in feed.Entries) {

                if (entry.Title.Text.Equals(c_sExcelFileName))
                {
                    res = entry;
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

        private ListFeed GetListFeed(WorksheetEntry entry2) {
            // Define the URL to request the list feed of the worksheet.
            AtomLink listFeedLink = entry2.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);

            // Fetch the list feed of the worksheet.
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            ListFeed listFeed = Inst.SpreadsheetsService.Query(listQuery);
            return listFeed;
        }

        private void RenderWeek(SpreadsheetEntry entry, StringBuilder sb) {
            Doc = new ExcelDoc(entry);

            for (int i = 0; i < 5; ++i) {
                WorksheetEntry worksheetEntry = GetWorksheetEntry(entry, i);
                renderTable(worksheetEntry, sb, Inst.Doc.GetExcelTable(i));
            }
        }

        private void renderTable(WorksheetEntry workEntry, StringBuilder sb, ExcelTable excelTable) {
            ListFeed listFeed = GetListFeed(workEntry);
            excelTable.Title = workEntry.Title.Text;

            if (listFeed.Entries.Count > 0) {
                sb.Append("<table border=1>");
                for (int i = 0; i < listFeed.Entries.Count; ++i) {
                    ListEntry row = listFeed.Entries[i] as ListEntry;
                    sb.Append("<tr>");
                    for (int j = 0; j < row.Elements.Count; ++j) {
                        ListEntry.Custom element = row.Elements[j];
                        sb.AppendFormat("<td><b>{0}</b></td>", element.Value);
                    }
                    sb.Append("</tr>");
                    break;
                }

                string category = "";
                for (int i = 1; i < listFeed.Entries.Count; ++i) {
                    ListEntry xmlRow = listFeed.Entries[i] as ListEntry;

                    ExcelRow excelRow = new ExcelRow(excelTable, xmlRow);
                    
                    excelRow.RowId = GetXmlRowId(xmlRow);
                    excelTable.Rows.Add(excelRow);

                    sb.Append("<tr>");


                    string tmpCategory = GetCategory(xmlRow);
                    if (!string.IsNullOrEmpty(tmpCategory)) {
                        category = tmpCategory.Replace(":","");
                    }
                    excelRow.Category = category;

                    for (int j = 0; j < xmlRow.Elements.Count; ++j) {
                        ListEntry.Custom element = xmlRow.Elements[j];
                        string columnName = element.LocalName;
                        sb.AppendFormat("<td>{0}<b>{1}</b></td>", element.Value, columnName);

                        if (columnName.Equals(ColumnNames.Description)) {
                            excelRow.Description = element.Value;
                        }
                        else if (columnName.Equals(ColumnNames.Name)) {
                            excelRow.Name = element.Value;
                        }
                        else if (columnName.Equals(ColumnNames.Price)) {
                            string price = element.Value;
                            if (!string.IsNullOrEmpty(price)) {
                                string str = price;
                              
                                decimal lPrice = 0;
                                if (ApiUtils.TryDecimalParse(str, out lPrice)) {
                                    excelRow.Price = lPrice;
                                    excelRow.HasPrice = true;
                                }
                            }
                        }
                        else {
                            ExcelCell cell = new ExcelCell(excelRow);
                            excelRow.Cells.Add(cell);
                            cell.ColumnName = columnName;
                            decimal lPrice = 0;
                            if (ApiUtils.TryDecimalParse(element.Value, out lPrice)) {
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
            for (int j = 0; j < xmlRow.Elements.Count; ++j) {
                ListEntry.Custom element = xmlRow.Elements[j];
                string columnName = element.LocalName;
                if (columnName.Equals(ColumnNames.Price)) {
                    string val = element.Value;
                    if (!string.IsNullOrEmpty(val) && val.ToLower().Contains("ц≥на")) {
                        hasCaption = true;
                        break;
                    }
                }
            }

            if (hasCaption) {
                for (int j = 0; j < xmlRow.Elements.Count; ++j) {
                    ListEntry.Custom element = xmlRow.Elements[j];
                    string columnName = element.LocalName;
                    if (columnName.Equals(ColumnNames.Name)) {
                        res = element.Value;
                        break;
                    }
                }
            }
            return res;
        }

        public void AddCellValue(int dayOfWeek, string rowId, string columnName, object value) {
            ListEntry xmlRow = GetRow(dayOfWeek, rowId);
            Debug.Assert(null != xmlRow);
            ListEntry.Custom xmlCell = GetCell(xmlRow, columnName);
            if (null == xmlCell) {
                xmlCell = new ListEntry.Custom();
                xmlCell.LocalName = columnName;
                xmlRow.Elements.Add(xmlCell);
            }
            xmlCell.Value = value.ToString();

            xmlRow.Update();
        }

        public string GetCellValue(int dayOfWeek, string rowId, string columnName) {
            string res = "";
            ListEntry xmlRow = GetRow(dayOfWeek, rowId);
            Debug.Assert(null != xmlRow);
            ListEntry.Custom xmlCell = GetCell(xmlRow, columnName);
            if (null != xmlCell) {
                res = xmlCell.Value;
            }
            return res;
        }

        private ListEntry.Custom GetCell(ListEntry xmlRow, string columnName) {
            ListEntry.Custom res = null;
            for (int j = 0; j < xmlRow.Elements.Count; ++j) {
                ListEntry.Custom xmlCell = xmlRow.Elements[j];
                string cellName = xmlCell.LocalName;
                if (cellName.Equals(columnName)) {
                    res = xmlCell;
                    break;
                }
            }
            return res;
        }

        private string GetXmlRowId(ListEntry row) {
            string res = row.Id.AbsoluteUri;
            res = new Regex("\\W").Replace(res, string.Empty);
            return res;
        }

        private ListEntry GetRow(int dayOfWeek, string rowId) {
            ListEntry res = null;

            ExcelTable excelTable = Doc.GetExcelTable(dayOfWeek);
            ExcelRow excelRow = excelTable.GetRowById(rowId);

            SpreadsheetsService service = Inst.SpreadsheetsService;
            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = service.Query(query);

            SpreadsheetEntry xmlDoc = GetSpreadsheetEntry(feed);
            WorksheetEntry xmlPage = GetWorksheetEntry(xmlDoc, dayOfWeek);
            ListFeed xmlTable = GetListFeed(xmlPage);
            for (int i = 1; i < xmlTable.Entries.Count; ++i) {
                ListEntry xmlRow = xmlTable.Entries[i] as ListEntry;
                Debug.Assert(null != xmlRow);
                if (excelRow.RowId.Equals(GetXmlRowId(xmlRow))) {
                    res = xmlRow;
                    break;
                }
            }
            return res;
        }
    }
}