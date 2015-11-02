using System.Security.Principal;
using System.Web;

namespace FoodApp.Controllers
{
    public class ApiUtils
    {
        public static string GetUserLogin() {
            string res = null;
            if (null != HttpContext.Current.Session) {
                return HttpContext.Current.Session["name"] as string;
            }
            return res;
        }

        public static void SetUserLogin(string val) {
            HttpContext.Current.Session["name"] = val;
        }
    }
}