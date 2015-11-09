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

        public static string GetLatinCodeFromCyrillic(string str)
        {

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
    }
}