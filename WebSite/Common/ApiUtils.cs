using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using FoodApp.Client;
using FoodApp.Common;

namespace FoodApp.Controllers {
    public class ApiUtils {
        //public const string c_sExcelFileName = "mykhaylo_test";
        public const string c_sExcelFileName = "Меню на тиждень";
        
#if DEBUG
        public const string REDIRECT_URL = "http://localhost:15845/";
#else
        public const string REDIRECT_URL = "http://www.gam-gam.lviv.ua/";
#endif

        public const string CLIENT_ID = "668583993597.apps.googleusercontent.com";
        public const string CLIENT_SECRET = "70LRXGzVw-G1t5bzRmdUmcoj";
        public const string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

        public static string USER_INFO_SCOPE =
            "https://www.googleapis.com/auth/userinfo.profile  https://www.googleapis.com/auth/userinfo.email";

        public static string GetSessionUserId() {
            string res = null;
            if (null != HttpContext.Current.Session) {
                res = HttpContext.Current.Session["userId"] as string;
            }
            if (string.IsNullOrEmpty(res)) {
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

        public static ngUserModel GetUser()
        {
            ngUserModel res = null;
            string id = GetSessionUserId();
            if (!string.IsNullOrEmpty(id)) {
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
            List<ngOrderEntry> res = new List<ngOrderEntry>();

            List<ngFoodItem> foods = OrderManager.Inst.GetOrderedFoods(user, day);
            int smallContainersCount = 0;
            int bigContainersCount = 0;

            int meathOrFish = 0;
            int garnirOrSalat = 0;
            foreach (ngFoodItem food in foods) {
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
            if ((garnirOrSalat == 1 && meathOrFish == 0) || (garnirOrSalat == 0 && meathOrFish == 1)) {
                smallContainersCount++;
            }
            else if ((garnirOrSalat != 0) || (meathOrFish != 0)) {
                bigContainersCount = Convert.ToInt16(Math.Round(garnirOrSalat/(decimal) 2, MidpointRounding.ToEven));
                if (garnirOrSalat%2 > 0) {
                    smallContainersCount++;
                }
            }

            // remove containers
            foreach (ngOrderEntry order in orders) {
                ngFoodItem ngFoodItem = FoodManager.Inst.GetFoodById(day, order.FoodId);
                if (!ngFoodItem.isContainer) {
                    res.Add(order);
                }
            }

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


        internal static bool TryDecimalParse(string str, out decimal lPrice) {
            bool res = false;
            if (!string.IsNullOrEmpty(str)) {
                str = str.Replace("грн.", "");
                str = str.Replace("грн ", "");
                str = str.Replace(",", ".");

                res = decimal.TryParse(str, out lPrice);
            }
            else {
                lPrice = 0;
            }
            return res;
        }

        internal static bool EqualDate(DateTime dt1, DateTime dt2) {
            return dt1.Year.Equals(dt2.Year) && dt1.Month.Equals(dt2.Month) && dt1.Day.Equals(dt2.Day);
        }



        
    }
}