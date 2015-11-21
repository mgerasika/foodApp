using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FoodApp.Common;
using Google.GData.Client;
using GoogleAppsConsoleApplication;

namespace FoodApp.Controllers {
    public class HomeController : Controller {
        public const string EmailQueryString = "email";
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
            if (Request.QueryString["code"] != null && string.IsNullOrEmpty(ApiUtils.GetSessionEmail())) {
                OAuth2Parameters lParams = CreateParameters();
                lParams.AccessType = "offline";
                lParams.AccessCode = HttpContext.Request.QueryString["code"];
                OAuthUtil.GetAccessToken(lParams);
                string lUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json";
                HttpWebRequest client = WebRequest.Create(lUrl) as HttpWebRequest;
                client.Method = "GET";
                client.Headers.Add("Authorization", "Bearer " + lParams.AccessToken);
                try {
                    using (HttpWebResponse response = (HttpWebResponse) client.GetResponse()) {
                        using (Stream dataStream = response.GetResponseStream()) {
                            using (StreamReader reader = new StreamReader(dataStream)) {
                                if (response.StatusCode == HttpStatusCode.OK) {
                                    JavaScriptSerializer json = new JavaScriptSerializer();
                                    IDictionary<string, object> data =
                                        json.Deserialize<IDictionary<string, object>>(reader.ReadToEnd());

                                    string email = Convert.ToString(data["email"]);
                                    ngUserModel userModel = UsersManager.Inst.GetUserByEmail(email);
                                    if (null == userModel) {
                                        throw new UnauthorizedAccessException("You don't have permission to access");
                                    }
                                    if (string.IsNullOrEmpty(userModel.GoogleName)) {
                                        userModel.Picture = Convert.ToString(data["picture"]);
                                        userModel.GoogleName = Convert.ToString(data["name"]);
                                        UsersManager.Inst.Save();
                                    }

                                    ApiUtils.SetSessionEmail(userModel.Email);
                                    return RedirectToAction("Index");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) {
                }
            }
            if (string.IsNullOrEmpty(ApiUtils.GetSessionEmail())) {
                return RedirectToAction("Login");
            }
            ExcelParser.Inst.Init();
            MakeFavoriteFromHistoryManager.Inst.ParseHistoryAndMakeRate();

            return View();
        }

        public ActionResult Login() {
            return View();
        }

        public ActionResult LoginClick() {
            OAuth2Parameters lParams = CreateParameters();
            string lUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(lParams);
            return Redirect(lUrl);
        }

        private static OAuth2Parameters CreateParameters() {
            OAuth2Parameters lParams = new OAuth2Parameters();
            lParams.ClientId = ApiUtils.CLIENT_ID;
            lParams.ClientSecret = ApiUtils.CLIENT_SECRET;
            lParams.RedirectUri = ApiUtils.REDIRECT_URL;
            lParams.Scope = ApiUtils.USER_INFO_SCOPE;
            //parameters.ApprovalPrompt = "force";
            lParams.TokenExpiry = DateTime.MaxValue;
            lParams.AccessType = "offline";
            return lParams;
        }
    }
}