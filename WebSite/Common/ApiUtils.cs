using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Web;
using FoodApp.Common.Managers;

namespace FoodApp.Common {
    public class ApiUtils {
#if DEBUG
        public const string REDIRECT_URL = "http://localhost:15845/";
        //public const string c_sExcelFileName = "mykhaylo_test";
        public const string c_sExcelFileName = "Меню на тиждень";
#else
        public const string REDIRECT_URL = "http://www.gam-gam.lviv.ua/";
        public const string c_sExcelFileName = "Меню на тиждень";
#endif

        public const string CLIENT_ID = "668583993597.apps.googleusercontent.com";
        public const string CLIENT_SECRET = "70LRXGzVw-G1t5bzRmdUmcoj";
        public const string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

        public static string USER_INFO_SCOPE =
            "https://www.googleapis.com/auth/userinfo.profile  https://www.googleapis.com/auth/userinfo.email";

        public static string GetSessionUserId() {
            string res = null;
            if (null != HttpContext.Current && null != HttpContext.Current.Session) {
                res = HttpContext.Current.Session["userId"] as string;
            }
            if (String.IsNullOrEmpty(res)) {
                HttpRequest request = GetHttpRequest();
                if (null != request) {
                    HttpCookie httpCookie = request.Cookies.Get("userId");
                    if (null != httpCookie) {
                        res = httpCookie.Value;
                    }
                }
            }
            return res;
        }

        public static ngUserModel GetLoggedInUser() {
            ngUserModel res = null;
            string id = GetSessionUserId();
            if (!String.IsNullOrEmpty(id)) {
                ngUserModel userById = UsersManager.Inst.GetUserById(id);
                Debug.Assert(null != userById);
                res = userById;
            }
            return res;
        }

        public static void SetSessionUserId(string val) {
            HttpContext.Current.Session["userId"] = val;
            HttpResponse response = GetHttpResponse();
            if (null != response) {
                HttpCookie httpCookie = new HttpCookie("userId", val);
                httpCookie.Expires = DateTime.MaxValue;
                response.Cookies.Add(httpCookie);
            }
        }

        private static HttpRequest GetHttpRequest() {
            HttpRequest res = null;
            try {
                res = HttpContext.Current.Request;
            }
            catch (Exception e) {
            }
            return res;
        }

        private static HttpResponse GetHttpResponse() {
            HttpResponse res = null;
            try {
                res = HttpContext.Current.Response;
            }
            catch (Exception e) {
            }
            return res;
        }

        public static string GetLatinCodeFromCyrillic(string str) {
            str = str.Replace("б", "b");
            str = str.Replace("Б", "B");

            str = str.Replace("в", "v");
            str = str.Replace("В", "V");

            str = str.Replace("г", "h");
            str = str.Replace("Г", "H");

            str = str.Replace("ґ", "g");
            str = str.Replace("Ґ", "G");

            str = str.Replace("д", "d");
            str = str.Replace("Д", "D");

            str = str.Replace("є", "ye");
            str = str.Replace("Э", "Ye");

            str = str.Replace("ж", "zh");
            str = str.Replace("Ж", "Zh");

            str = str.Replace("з", "z");
            str = str.Replace("З", "Z");

            str = str.Replace("и", "y");
            str = str.Replace("И", "Y");

            str = str.Replace("ї", "yi");
            str = str.Replace("Ї", "YI");

            str = str.Replace("й", "j");
            str = str.Replace("Й", "J");

            str = str.Replace("к", "k");
            str = str.Replace("К", "K");

            str = str.Replace("л", "l");
            str = str.Replace("Л", "L");

            str = str.Replace("м", "m");
            str = str.Replace("М", "M");

            str = str.Replace("н", "n");
            str = str.Replace("Н", "N");

            str = str.Replace("п", "p");
            str = str.Replace("П", "P");

            str = str.Replace("р", "r");
            str = str.Replace("Р", "R");

            str = str.Replace("с", "s");
            str = str.Replace("С", "S");

            str = str.Replace("ч", "ch");
            str = str.Replace("Ч", "CH");

            str = str.Replace("ш", "sh");
            str = str.Replace("Щ", "SHH");

            str = str.Replace("ю", "yu");
            str = str.Replace("Ю", "YU");

            str = str.Replace("Я", "YA");
            str = str.Replace("я", "ya");

            str = str.Replace('ь', '"');
            str = str.Replace("Ь", "");

            str = str.Replace('т', 't');
            str = str.Replace("Т", "T");

            str = str.Replace('ц', 'c');
            str = str.Replace("Ц", "C");

            str = str.Replace('о', 'o');
            str = str.Replace("О", "O");

            str = str.Replace('е', 'e');
            str = str.Replace("Е", "E");

            str = str.Replace('а', 'a');
            str = str.Replace("А", "A");

            str = str.Replace('ф', 'f');
            str = str.Replace("Ф", "F");

            str = str.Replace('і', 'i');
            str = str.Replace("І", "I");

            str = str.Replace('У', 'U');
            str = str.Replace("у", "u");

            str = str.Replace('х', 'x');
            str = str.Replace("Х", "X");
            str = str.Replace("'", "");
            return str;
        }

        public static List<ngOrderEntry> AddContainersToFood(ngUserModel user, int day, List<ngOrderEntry> orders) {
            List<ngOrderEntry> filteredOrders = RemoveContainers(orders, day);

            int smallContainersCount = 0;
            int bigContainersCount = 0;

            int meathOrFish = 0;
            int garnirOrSalat = 0;
            foreach (ngOrderEntry order in orders) {
                if (order.Count > 0) {
                    ngFoodItem food = FoodManager.Inst.GetFoodById(day, order.FoodId);
                    Debug.Assert(null != food);

                    if (food.isFirst) {
                        smallContainersCount++;
                        if (food.isKvasolevaOrChanachi) {
                            smallContainersCount++;
                        }
                    }
                    else if (food.isMeatOrFish) {
                        meathOrFish++;
                    }
                    else if (food.isSalat || food.isGarnir) {
                        garnirOrSalat++;
                    }
                }
            }
           
            if ((garnirOrSalat == 1 && meathOrFish == 0) || (garnirOrSalat == 0 && meathOrFish == 1)) {
                smallContainersCount++;
            }
            else if ((garnirOrSalat != 0) || (meathOrFish != 0)) {
                bigContainersCount = Convert.ToInt16(Math.Round(garnirOrSalat/(decimal) 2, MidpointRounding.ToEven));
                if (garnirOrSalat%2 > 0) {
                    smallContainersCount++;
                }
            }

            List<ngOrderEntry> res = new List<ngOrderEntry>();
            res.AddRange(filteredOrders);

            //update small containers cout
            ngFoodItem foodSmallContainer = FoodManager.Inst.GetSmallContainer(day);
            Debug.Assert(null != foodSmallContainer);
            ngOrderEntry smallContainerOrder = new ngOrderEntry();
            smallContainerOrder.FoodId = foodSmallContainer.FoodId;
            smallContainerOrder.Count = smallContainersCount;
            res.Add(smallContainerOrder);

            //update big containers count
            ngFoodItem foodBigContainer = FoodManager.Inst.GetBigContainer(day);
            Debug.Assert(null != foodBigContainer);
            ngOrderEntry bigContainerOrder = new ngOrderEntry();
            bigContainerOrder.FoodId = foodBigContainer.FoodId;
            bigContainerOrder.Count = bigContainersCount;
            res.Add(bigContainerOrder);

            return res;
        }

        private static List<ngOrderEntry> RemoveContainers(List<ngOrderEntry> orders, int day) {
            List<ngOrderEntry> res = new List<ngOrderEntry>();
            // remove containers
            foreach (ngOrderEntry order in orders) {
                ngFoodItem ngFoodItem = FoodManager.Inst.GetFoodById(day, order.FoodId);
                if (!ngFoodItem.isContainer) {
                    res.Add(order);
                }
            }
            return res;
        }


        internal static bool TryDecimalParse(string str, out decimal lPrice) {
            bool res = false;
            if (!String.IsNullOrEmpty(str)) {
                str = str.Replace("грн.", "");
                str = str.Replace("грн ", "");
                str = str.Replace(",", ".");
                str = str.Replace("-", ".");
                str = str.Replace(" ", "");

                res = Decimal.TryParse(str, out lPrice);
            }
            else {
                lPrice = 0;
            }
            return res;
        }

        internal static bool EqualDate(DateTime dt1, DateTime dt2) {
            return dt1.Year.Equals(dt2.Year) && dt1.Month.Equals(dt2.Month) && dt1.Day.Equals(dt2.Day);
        }

        public static bool Equals(string foodId1, string foodId2) {
            bool res = foodId1.Equals(foodId2, StringComparison.OrdinalIgnoreCase);
            return res;
        }

        public static bool IsSeamsFoodIds(string foodId1, string foodid2) {
            bool res = false;

            do {
                if (foodId1.Equals(foodid2)) {
                    res = true;
                    break;
                }

                int lenDiff = Math.Abs(foodId1.Length - foodid2.Length);
                const double coef = 0.1;
                const int compare = 3;
                if (foodId1.Length*coef < lenDiff || foodid2.Length*coef < lenDiff) {
                    break;
                }

                int equalsCount = 0;
                int symbCount = Math.Min(foodId1.Length, foodId1.Length);
                for (int i = 0; i < symbCount; i++) {
                    if (foodId1.Length > i + compare) {
                        string tmp = foodId1.Substring(i, compare);
                        if (foodid2.Contains(tmp)) {
                            equalsCount++;
                        }
                    }
                }

                int seamsDiff = Math.Abs(symbCount - equalsCount);
                if (equalsCount*coef < seamsDiff) {
                    break;
                }
                res = true;
            } while (false);
            return res;
        }

        public static ngFoodItem GetFoodById(List<ngFoodItem> items, string foodId) {
            ngFoodItem res = null;
            foreach (ngFoodItem item in items) {
                if (item.FoodId.Equals(foodId)) {
                    res = item;
                    break;
                }
            }
            return res;
        }

        public static void SendEmail(ngUserModel user, string subject,string msg)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587) { Credentials = new NetworkCredential("gamgamlviv@gmail.com", "nikita1984"), EnableSsl = true };
            client.Send("gamgamlviv@gmail.com", "gamgamlviv@gmail.com", subject, msg);
            client.Send("gamgamlviv@gmail.com", user.Email, subject, msg);
        }
    }
}