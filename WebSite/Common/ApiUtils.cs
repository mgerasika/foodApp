using System.Security.Principal;
using System.Web;

namespace FoodApp.Controllers
{
    public class ApiUtils
    {
        public static string GetUserLogin() {
            string res = null;
            if (null != HttpContext.Current.User && null != HttpContext.Current.User.Identity) {
                return HttpContext.Current.User.Identity.Name;
            }
            return res;
        }

        public static void SetUserLogin(string val)
        {
            HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(val), null); 
        }
    }
}