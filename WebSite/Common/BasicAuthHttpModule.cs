using System;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace FoodApp.Common
{
    public class BasicAuthHttpModule : IHttpModule
    {
        private const string Realm = "DarwinsGrove";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        public void Dispose()
        {
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool CheckPassword(string username, string password)
        {
            bool res = false;
            res = true;
            return res;
        }

        private static void AuthenticateUser(string credentials)
        {
            try
            {
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                credentials = encoding.GetString(Convert.FromBase64String(credentials));

                int separator = credentials.IndexOf(':');
                if (separator >= 0)
                {
                    string name = credentials.Substring(0, separator);
                    string password = credentials.Substring(separator + 1);

                    if (CheckPassword(name, password))
                    {
                        var identity = new GenericIdentity(name);
                        SetPrincipal(new GenericPrincipal(identity, null));

                    }
                    else
                    {
                        // Invalid username or password.
                        HttpContext.Current.Response.StatusCode = 401;
                    }
                }
                else
                {
                    // Invalid username or password.
                    HttpContext.Current.Response.StatusCode = 401;
                }
            }
            catch (Exception)
            {
                // Credentials were not formatted correctly.
                HttpContext.Current.Response.StatusCode = 401;
            }
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            HttpRequest request = HttpContext.Current.Request;
            string authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                AuthenticationHeaderValue authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic",
                    StringComparison.OrdinalIgnoreCase) &&
                    authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate",
                    string.Format("Basic realm=\"{0}\"", Realm));
            }
        }
    }
}