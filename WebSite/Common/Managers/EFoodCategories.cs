using System;
using GoogleAppsConsoleApplication;

namespace FoodApp.Common {
    public class EFoodCategories {
        public const string Salat = "Салати";
        public const string PershiStravy = "Перші страви";
        public const string Garniri = "Гарніри";
        public const string Konteinery = "Контейнери";
        public const string Kompleksniy = "Комплексний";
        public const string MjasoRiba = "Мясо/Риба";
        public const string Xlib = "Хліб";

        internal static string GetCategory(ExcelRow row) {
            string category = row.Category;
            string res = category;
            if (category.Equals("Салати", StringComparison.OrdinalIgnoreCase)) {
                res = Salat;
            }

            else if (category.Equals("Гарніри", StringComparison.OrdinalIgnoreCase)) {
                res = Garniri;
            }

            else if (category.Equals("Перші страви", StringComparison.OrdinalIgnoreCase)) {
                res = PershiStravy;
            }
            else if (row.Name.Contains("Контейнери")) {
                res = Konteinery;
            }
            else if (row.Name.Contains("батон")) {
                res = Xlib;
            }
            else if (category.Contains("Комплексний")) {
                res = Kompleksniy;
            }

            else {
                res = MjasoRiba;
            }

            return res;
        }
    }
}