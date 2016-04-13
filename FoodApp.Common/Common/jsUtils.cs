using SharpKit.Html;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class jsUtils {
        public static jsUtils inst = new jsUtils();

        private jsUtils() {
        }

        public string getLocation() {
            string name = HtmlContext.document.location.protocol + "//" + HtmlContext.document.location.host;
            return name;
        }

        public void showLoading() {
            HtmlElement loadingEl = HtmlContext.document.getElementById("loadingDiv") as HtmlElement;
            loadingEl.style.display = "block";
        }

        public void hideLoading() {
            HtmlContext.window.setTimeout(delegate {
                HtmlElement loadingEl = HtmlContext.document.getElementById("loadingDiv") as HtmlElement;
                loadingEl.style.display = "none";
            }, 200);
        }

        public string getSelectedText(Window wnd) {
            string text = "";
            if (wnd.As<JsObject>()["getSelection"] != null) {
                text = wnd.getSelection().toString();
            }
            else {
                JsObject selection = wnd.document.As<JsObject>()["selection"].As<JsObject>();
                if (selection != null && (selection["type"] != "Control")) {
                    JsFunction fn = selection["createRange"].As<JsFunction>();
                    text = fn.call().As<JsObject>()["text"].As<JsString>();
                }
            }
            return text;
        }


        public bool isEmpty(object str) {
            return (null == str) || (JsContext.undefined == str) || ("" == str) || ("null" == str);
        }

        public bool Contains(JsArray<string> ngCategories, string p) {
            bool res = false;
            foreach (string str in ngCategories) {
                if (str == p) {
                    res = true;
                    break;
                }
            }
            return res;
        }

        public decimal fixNumber(decimal p) {
            JsNumber jsNumber = JsContext.parseFloat(p.As<JsString>()).As<JsNumber>();
            string tmp = jsNumber.toPrecision(5).As<string>();
            return JsContext.parseFloat(tmp).As<decimal>();
        }
    }
}