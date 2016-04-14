using SharpKit.Html;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public class jsCommonUtils
    {
        public static jsCommonUtils inst = new jsCommonUtils();
        private string _location;

        protected jsCommonUtils()
        {
        }

        public bool Equals(string prefix, string prefix2)
        {
            bool res = prefix.As<JsString>().toLowerCase() == prefix2.As<JsString>().toLowerCase();
            return res;
        }

        public void assert(bool p1)
        {
            assert(p1, "");
        }

        public void assert(bool p1, string msg)
        {
            if (!p1)
            {
                if (msg == null)
                {
                    msg = "";
                }
                if (HtmlContext.confirm(msg + "Debug?"))
                {
                    JsContext.debugger();
                }
            }
        }

        public bool isEmpty(string prefix)
        {
            bool res = prefix == null || prefix == "" && prefix == JsContext.undefined || prefix.As<int>() == 0;
            return res;
        }

        public string GetPrefix(JsString prop)
        {
            JsString prefix = "";
            int idx = prop.indexOf(".");
            if (-1 != idx)
            {
                prefix = prop.substr(0, idx + 1);
            }
            return prefix;
        }

        public bool HasPrefix(JsString prop)
        {
            bool res = false;
            int idx = prop.indexOf(".");
            if (-1 != idx)
            {
                res = true;
            }
            return res;
        }

        public bool IsArray(object obj)
        {
            bool res = false;
            JsArray arr = obj.As<JsArray>();
            if (null != arr && arr.As<JsObject>()["pop"] != null)
            {
                res = true;
            }
            return res;
        }

        public string appendQueryToUrl(string url, string queryStr)
        {
            string res = url;
            if (queryStr != "")
            {
                if (url.IndexOf("?") == -1)
                {
                    res = url + "?" + queryStr;
                }
                else {
                    res = url + "&" + queryStr;
                }
            }
            return res;
        }

        public bool isEmpty(object str)
        {
            return (null == str) || (JsContext.undefined == str) || ("" == str) || ("null" == str);
        }

        public bool toBool(object jsString)
        {
            bool res = false;
            if (!inst.isEmpty(jsString))
            {
                if (jsString.As<string>() == "true" || jsString.As<int>() == 1 || jsString.As<bool>())
                {
                    res = true;
                }
            }
            return res;
        }

        public bool contains(JsArray<string> prefixes, string prefix)
        {
            bool res = false;
            foreach (string pr in prefixes)
            {
                if (pr == prefix)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }

        public string getQueryParam(JsString jsString, string p)
        {
            string res = "";
            int idx = jsString.indexOf("?");
            if (idx > 0)
            {
                JsString tmp = jsString.substr(idx + 1);
                JsArray<JsString> args = tmp.split("&");
                if (args.length == 0)
                {
                    args.Add(tmp);
                }
                foreach (JsString arg in args)
                {
                    JsArray<JsString> keyVal = arg.split("=");
                    if (keyVal.length == 2)
                    {
                        if (keyVal[0] == p)
                        {
                            res = keyVal[1];
                            break;
                        }
                    }
                }
            }
            return res;
        }

        public string getLocation()
        {
            string res = _location;
            if (jsCommonUtils.inst.isEmpty(_location))
            {
                res = HtmlContext.document.location.protocol + "//" + HtmlContext.document.location.host + "/";
            }
            return res;
        }

        public string getPrefix(JsString tmp)
        {
            int idx = tmp.indexOf(".");
            string prefix = tmp.substr(0, idx + 1);
            return prefix;
        }

        public void removeFromArray(JsArray items, JsObject obj)
        {
            JsArray tmpArray = new JsArray();
            foreach (JsObject tmp in items)
            {
                if (tmp != obj)
                {
                    tmpArray.push(tmp);
                }
            }
            while (items.length > 0)
            {
                items.pop();
            }
            foreach (object tmp in tmpArray)
            {
                items.push(tmp);
            }
        }
    }
}