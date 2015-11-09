using System;
using System.Web;
using Google.GData.Client;
using Google.GData.Spreadsheets;

namespace GoogleAppsConsoleApplication
{
    public class LoginTool
    {
        public static LoginTool Inst = new LoginTool();
        private OAuth2Parameters _parameters;
        private SpreadsheetsService _SpreadsheetsService = null;
        private readonly string CLIENT_ID = "668583993597.apps.googleusercontent.com";
        private readonly string CLIENT_SECRET = "70LRXGzVw-G1t5bzRmdUmcoj";
        private readonly string REDIRECT_URI = "http://localhost:15845/Home/Test2";
        private readonly string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

        public string StartLogin() {
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

        public string EndLogin() {
            _parameters.AccessCode = HttpContext.Current.Request.QueryString["code"];
            OAuthUtil.GetAccessToken(_parameters);
            var accessToken = _parameters.AccessToken;


            return accessToken;
        }
    }
}