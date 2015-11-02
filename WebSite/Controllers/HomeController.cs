using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using GoogleAppsConsoleApplication;

namespace FoodApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() {
            if (ExcelManager.Inst.SpreadsheetsService == null) {
                return RedirectToAction("Login");
            }
            return View();
        }

        public string Login() {
            return ExcelManager.Inst.StartParseExcel();
        }

        public string Test2()
        {
            string res = ExcelManager.Inst.EndParseExcel();
            // RedirectToAction("Index");
            return res;
        }
    }
}