using System;
using FoodApp.Common;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class clientUtils
    {
        public static clientUtils Inst = new clientUtils();

        private clientUtils()
        {
        }

        public string getLocation()
        {
            string name = HtmlContext.document.location.protocol + "//" + HtmlContext.document.location.host;
            return name;
        }

        public void showLoading()
        {
            HtmlElement loadingEl = HtmlContext.document.getElementById("loadingDiv") as HtmlElement;
            loadingEl.style.display = "block";
        }

        public void hideLoading()
        {
            HtmlContext.window.setTimeout(delegate
            {
                HtmlElement loadingEl = HtmlContext.document.getElementById("loadingDiv") as HtmlElement;
                loadingEl.style.display = "none";
            }, 200);
        }

        public string getSelectedText(Window wnd)
        {
            string text = "";
            if (wnd.As<JsObject>()["getSelection"] != null)
            {
                text = wnd.getSelection().toString();
            }
            else
            {
                JsObject selection = wnd.document.As<JsObject>()["selection"].As<JsObject>();
                if (selection != null && (selection["type"] != "Control"))
                {
                    JsFunction fn = selection["createRange"].As<JsFunction>();
                    text = fn.call().As<JsObject>()["text"].As<JsString>();
                }
            }
            return text;
        }

      

        public bool isEmpty(object str)
        {
            return (null == str) || (JsContext.undefined == str) || ("" == str) || ("null" == str);
        }

        internal bool Contains(JsArray<string> ngCategories, string p) {
            bool res = false;
            foreach (string str in ngCategories) {
                if (str == p) {
                    res = true;
                    break;
                }
            }
            return res;
        }

        internal decimal fixNumber(decimal p) {
            JsNumber jsNumber = JsContext.parseFloat(p.As<JsString>()).As<JsNumber>();
            string tmp = jsNumber.toPrecision(5).As<string>();
            return JsContext.parseFloat(tmp).As<decimal>();
        }

        public bool compareFoodIds(JsString foodId1, JsString foodid2) {
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
                for (int i = 0; i < symbCount; i ++) {
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
    }
}