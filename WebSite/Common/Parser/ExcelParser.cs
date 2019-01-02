using System;
using System.Diagnostics;
using System.Text;
using FoodApp.Client;
using FoodApp.Common.Managers;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace FoodApp.Common.Parser {
    public class ExcelParser
    {
        private static readonly object _lockObj = new object();
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


        public void RefreshAccessToken()
        {
            OAuthUtil.RefreshAccessToken(_parameters);
        }

        public string Init()
        {
            StringBuilder sb = new StringBuilder();

            lock (_lockObj)
            {
                if (null == SpreadsheetsService)
                {
                    //ApiTraceManager.Inst.LogTrace("Init excel parser");
                    _parameters = new OAuth2Parameters();
                    _parameters.ClientId = ApiUtils.CLIENT_ID;
                    _parameters.ClientSecret = ApiUtils.CLIENT_SECRET;
                    _parameters.Scope = ApiUtils.SCOPE;
                    _parameters.RedirectUri = ApiUtils.REDIRECT_URL;
                    _parameters.TokenExpiry = DateTime.MaxValue;
                    _parameters.AccessType = "offline";
                    _parameters.AccessToken =
                        "ya29.IAK0DqyyBCLy1lNBPHM4ON0m74HiPnXdJnizHC2a7w5CIDndeLPuFbJm0u34HfKpiXZ8UQ";
                    _parameters.RefreshToken = "1/jWXrc6wJ4lUmWscyc3rozkWPKdFwvha43JfPCxMksus";

                    GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1",
                        _parameters);
                    SpreadsheetsService = new SpreadsheetsService("MySpreadsheetIntegration-v1");
                    SpreadsheetsService.RequestFactory = requestFactory;
                    sb.AppendFormat("<div>access code={0}</div>", _parameters.AccessCode);
                    sb.AppendFormat("<div>access token={0}</div>", _parameters.AccessToken);
                    sb.AppendFormat("<div>refresh token={0}</div>", _parameters.RefreshToken);

                    SpreadsheetsService service = SpreadsheetsService;
                    SpreadsheetQuery query = new SpreadsheetQuery();
                    SpreadsheetFeed feed = service.Query(query);

                    SpreadsheetEntry entry = GetSpreadsheetEntry(feed);
                    Debug.Assert(null != entry);
                    sb.Append("<h1>" + entry.Title.Text + "</h1>");
                    RenderWeek(entry,service);

                   
                    this.IsInit = true;
                }
            }
            return sb.ToString();
        }


        private SpreadsheetEntry GetSpreadsheetEntry(SpreadsheetFeed feed)
        {
            SpreadsheetEntry res = new SpreadsheetEntry();
            foreach (SpreadsheetEntry entry in feed.Entries)
            {
                if (entry.Title.Text.Equals(ApiUtils.c_sExcelFileName))
                {
                    res = entry;
                    break;
                }
            }
            return res;
        }



        private void RenderWeek(SpreadsheetEntry entry,SpreadsheetsService service)
        {
            Doc = new ExcelDoc(entry,service);

            Doc.Parse();
        }

        public bool IsInit { get; set; }
    }
}