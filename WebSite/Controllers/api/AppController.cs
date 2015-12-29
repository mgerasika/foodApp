using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using FoodApp.Client;
using FoodApp.Common;
using GoogleAppsConsoleApplication;

namespace FoodApp.Controllers.api
{
    public class FoodAppController : System.Web.Http.ApiController
    {
        [HttpPost]
        [Route("foodApp/login")]
        public bool Login(string email) {
            ngUserModel userByEmail = UsersManager.Inst.GetUserByEmail(email);
            Debug.Assert(null != userByEmail);
            ApiUtils.SetSessionUserId(userByEmail.Id);
            return true;
        }

        private string GetUserLogin()
        {
            string res = null;
            if (null != HttpContext.Current.User && null != HttpContext.Current.User.Identity)
            {
                return HttpContext.Current.User.Identity.Name;
            }
            return res;
        }
       
    }
}