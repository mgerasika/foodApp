using System;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace FoodApp.Common.Url {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class UrlBase {
        protected string FormatUrl(string str, params string[] tmpArgs) {
            if (IsJavaScriptExecute()) {
                JsArray lArguments = JsContext.arguments.As<JsArray>();
                int len = lArguments.As<JsArray>().length;
                JsRegExp regExp = new JsRegExp(@"{\w+}");
                for (int i = 1; i < len; ++i) {
                    JsString arg = lArguments.As<JsArray>()[i].As<JsString>();
                    str = str.As<JsString>().replace(regExp, arg);
                }
            }
            else {
                str = FormatUrlInternal(str, tmpArgs);
            }
            return str;
        }

        [JsMethod(Export = false)]
        private static string FormatUrlInternal(string url, params string[] args) {
            string res = url;
            int idx = 0;
            string MASK = "[###{0}###]";
            while (true) {
                int startIdx = res.IndexOf("{");
                int endIdx = res.IndexOf("}");
                if (startIdx >= 0) {
                    string part = res.Substring(startIdx, endIdx - startIdx + 1);

                    res = res.Replace(part, string.Format(MASK, idx++));
                }
                else {
                    break;
                }
            }
            idx = 0;
            string find = string.Format(MASK, idx);
            while (res.Contains(find)) {
                res = res.Replace(find, "{" + idx++ + "}");
                find = string.Format(MASK, idx);
            }
            string[] newArgs = new string[args.Length];
            for (int i = 0; i < args.Length; ++i) {
                if (args[i].Contains("{") && args[i].Contains("}")) {
                    newArgs[i] = args[i];
                }
                else {
                    newArgs[i] = EscapeDataString(args[i]);
                }
            }

            res = string.Format(res, newArgs);
            return res;
        }

        [JsMethod(Export = false)]
        private static string EscapeDataString(string p) {
            return Uri.EscapeDataString(p).Replace("%2F", "_x002F_");
        }

        private static bool IsJavaScriptExecute() {
            return HtmlContext.window != null && HtmlContext.window.location != null && HtmlContext.window.location.href != null;
        }
    }

    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs, Export = true)]
    public class PropousalUrl {
        public const string c_sGetPropousal = "api/propousal";
        public const string c_sGetPropousalByDay = c_sGetPropousal + "/{userId}/{dayOfWeek}/";
        public const string c_sGetPropousals = c_sGetPropousal + "/{userId}/";
        public const string c_sBuy = c_sGetPropousal + "/{userId}/{dayOfWeek}/";
    }
}