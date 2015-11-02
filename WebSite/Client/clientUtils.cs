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
            var loadingEl = HtmlContext.document.getElementById("loadingIcon") as HtmlElement;
            loadingEl.style.display = "block";
        }

        public void prettyPrint(string txt, string id)
        {
            var fn = HtmlContext.window.As<JsObject>()["prettyPrintEx"].As<JsFunction>();
            fn.call(HtmlContext.window, txt, id);
        }

       


        public void hideLoading()
        {
            HtmlContext.window.setTimeout(delegate
            {
                var loadingEl = HtmlContext.document.getElementById("loadingIcon") as HtmlElement;
                loadingEl.style.display = "none";
            }, 200);
        }

        public string getSelectedText(Window wnd)
        {
            var text = "";
            if (wnd.As<JsObject>()["getSelection"] != null)
            {
                text = wnd.getSelection().toString();
            }
            else
            {
                var selection = wnd.document.As<JsObject>()["selection"].As<JsObject>();
                if (selection != null && (selection["type"] != "Control"))
                {
                    var fn = selection["createRange"].As<JsFunction>();
                    text = fn.call().As<JsObject>()["text"].As<JsString>();
                }
            }
            return text;
        }

      

        public bool isEmpty(object str)
        {
            return (null == str) || (JsContext.undefined == str) || ("" == str) || ("null" == str);
        }
    }
}