using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FoodApp.Client;
using FoodApp.Common;
using FoodApp.Common.Managers;
using FoodApp.Common.Parser;
using Google.GData.Client;

namespace FoodApp.Controllers {
    public class HomeController : Controller {
        public const string UserIdQueryString = "email";
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

        [Route("trace")]
        public ViewResult Trace() {
            return View();
        }

        [Route("error")]
        public ViewResult Error() {
            return View();
        }

        public ActionResult MIndex() {
            ActionResult viewResult = null;
            if (!IsAuthAndLogined()) {
                viewResult = AuthinficateAndLogin();
            }
            if (null == viewResult) {
                viewResult = View();
            }
            return viewResult;
        }

        [Route("Review")]
        public ActionResult Review() {
            ActionResult viewResult = null;
            if (!IsAuthAndLogined()) {
                viewResult = AuthinficateAndLogin();
            }
            if (null == viewResult) {
                viewResult = View();
            }
            return viewResult;
        }


        public ActionResult Index() {
            ActionResult viewResult = null;

            bool isMobile = Request.Browser.IsMobileDevice;

            if (!IsAuthAndLogined()) {
                viewResult = AuthinficateAndLogin();
            }
            if (null == viewResult) {
                if (null == ApiUtils.GetLoggedInUser()) {
                    viewResult = RedirectToAction("Login");
                }
                else {
                    ExcelManager.Inst.Init();
                    HistoryManager.Inst.FixFoodIdsForLoogedUser();
                    UserSettingsManager.Inst.Init();
                    UsersManager.Inst.Init();
                    ApiTraceManager.Inst.LogTrace("User on index page ");

                    if (ApiUtils.GetLoggedInUser().IsReviewer) {
                        viewResult = View("Review");
                    }
                    else {
                        viewResult = isMobile ? View("MIndex") : View();
                    }
                }
            }
            return viewResult;
        }

        public ActionResult Login() {
            return View();
        }

        public ActionResult Admin() {
            return View();
        }

        public ActionResult LoginClick() {
            OAuth2Parameters lParams = CreateParameters();
            string lUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(lParams);
            return Redirect(lUrl);
        }

        private ActionResult AuthinficateAndLogin() {
            ActionResult res = null;
            bool isMobile = Request.Browser.IsMobileDevice;

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
                                    //create new user.
                                    //userModel = UsersManager.Inst.GetUserByEmail("unknown@gmail.com");
                                    ngUserModel tmp = new ngUserModel();
                                    tmp.Email = email;
                                    tmp.IsAdmin = false;
                                    tmp.IsReviewer = true;
                                    tmp.Id = Guid.NewGuid().ToString();
                                    UsersManager.Inst.AddItemAndSave(tmp);
                                    userModel = tmp;
                                }
                                if (string.IsNullOrEmpty(userModel.GoogleFirstName)) {
                                    userModel.Picture = Convert.ToString(data["picture"]);
                                    userModel.GoogleName = Convert.ToString(data["name"]);
                                    userModel.GoogleFirstName = Convert.ToString(data["given_name"]);
                                    UsersManager.Inst.Save();
                                }
                                if (email.Contains("mgerasika") || email.Contains("mherasika") || email.Contains("omartsinets")) {
                                    if (!userModel.IsAdmin) {
                                        userModel.IsAdmin = true;
                                        UsersManager.Inst.Save();
                                    }
                                }
                                ApiUtils.SetSessionUserId(userModel.Id);

                                if (userModel.IsReviewer) {
                                    res = RedirectToAction("Review");
                                }
                                else {
                                    res = isMobile ? RedirectToAction("MIndex") : RedirectToAction("Index");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
            }
            return res;
        }

        private bool IsAuthAndLogined() {
            bool res = Request.QueryString["code"] != null && null == ApiUtils.GetLoggedInUser();
            return !res;
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