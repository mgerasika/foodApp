using System;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using GoogleAppsConsoleApplication;

namespace FoodApp.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult Index() {
            if (Request.QueryString["login"] != null) {
                ApiUtils.SetUserLogin(Request.QueryString["login"]);
            }
            if (string.IsNullOrEmpty(ApiUtils.GetUserLogin()))
            {
                return RedirectToAction("Login");
            }
            ExcelParser.Inst.Init();
            return View();
        }

        public ActionResult Login() {
            return View();
            //return LoginTool.Inst.StartLogin();
        }

        /*
        public ActionResult Test2() {
            if (string.IsNullOrEmpty(GetUserLogin())) {
                string res = LoginTool.Inst.EndLogin();
                SetUserLogin(res);
                return View();
            }
            else {
                return RedirectToAction("/");
            }
        }
         * */
    }
}