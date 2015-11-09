using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Services.Discovery;
using FoodApp.Client;
using FoodApp.Common;
using Google.API.Search;
using GoogleAppsConsoleApplication;

namespace FoodApp.Controllers
{
    public class HomeController : Controller
    {
        /*
        public string Test() {
            var sb = new StringBuilder();
            var ngFoodItems = FoodManager.Inst.GetAllFoods();
            foreach (var item in ngFoodItems) {
                sb.AppendFormat("<div>{0} - {1} - {2}</div>", item.Category, item.Name, item.FoodId);
            }
            return sb.ToString();
        }

        public bool Generate() {
            var ngFoodItems = FoodManager.Inst.GetAllFoods();

            var folder = HostingEnvironment.MapPath("~/img");
            DirectoryInfo dir = new DirectoryInfo(folder);
            foreach (FileInfo file in dir.GetFiles()) {
                string newFileName = ApiUtils.GetLatinCodeFromCyrillic(file.Name);
                try {
                    file.MoveTo(dir.FullName + "/" + newFileName);
                }
                catch (Exception ex) {
                    
                }
            }

            foreach (var item in ngFoodItems) {
                DownloadImage(item);
            }
            return true;
        }

        private void DownloadImage(ngFoodItem item) {
            var searchText = "Кулинария " +item.Category + " " + item.Name;

            var downloadCount = 7;
            var exist = false;
            for (var i = 0; i < downloadCount; ++i) {
                var imgPath = GetImagePath(item, 0);
                if (System.IO.File.Exists(imgPath)) {
                    exist = true;
                }
                else {
                    exist = false;
                    break;
                }
            }

            if (!exist) {
                var client = new GimageSearchClient("http://www.google.com.ua");
                var result = client.Search(searchText, downloadCount);
                for (var i = 0; i < result.Count; ++i) {
                    var image = result[i];
                    var url = image.Url;
                    var download = new WebClient();

                    var fileName = GetImagePath(item, i);
                    try {
                        download.DownloadFile(url, fileName);
                    }
                    catch (Exception ex) {
                    }
                }
            }
        }

        private static string GetImagePath(ngFoodItem item, int i) {
            var folder = HostingEnvironment.MapPath("~/img");
            var fileName = folder + "/" + item.FoodId + "_" + i + ".jpg";
            return fileName;
        }
         */

        public ActionResult Index() {
            if (Request.QueryString["login"] != null) {
                ApiUtils.SetUserLogin(Request.QueryString["login"]);
            }
            if (string.IsNullOrEmpty(ApiUtils.GetUserLogin())) {
                return RedirectToAction("Login");
            }
            ExcelParser.Inst.Init();
            return View();
        }

        public ActionResult Login() {
            return View();
            //return LoginTool.Inst.StartLogin();
        }
    }
}