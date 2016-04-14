using SharpKit.Html;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Common {
    [JsType(JsMode.Prototype, Filename = CommonApiResources._fileClientJs)]
    public abstract class JsApiBase {
        protected string _userId;
        protected string _serverUrl;
        public JsApiBase(string serverUrl,string userId) {
            _serverUrl = serverUrl;
            _userId = userId;
        }

        public string getUserId() {
            return _userId;
        }

        public object Deserealize(object obj) {
            object res = obj;

            if (jsCommonUtils.inst.IsArray(obj)) {
                jsCommonUtils.inst.assert(null != res);
            }
            else {
                jsCommonUtils.inst.assert(null != res);
            }

            return res;
        }


        protected string GetPrefixFromArray(JsArray args) {
            JsString prefix = null;
            foreach (JsObject arg in args) {
                prefix = GetPrefixFromObject(arg);
            }
            return prefix;
        }

        protected string GetPrefixFromObject(JsObject arg) {
            JsString prefix = null;
            foreach (JsString prop in arg) {
                if (jsCommonUtils.inst.HasPrefix(prop)) {
                    prefix = jsCommonUtils.inst.GetPrefix(prop);
                    break;
                }
            }
            return prefix;
        }

        protected void SendGet<T>(string url, JsHandler<T> handler, params object[] args) {
            string serverUrl = _serverUrl;
            url = jsCommonUtils.inst.appendQueryToUrl(serverUrl + url, "time=" + new JsDate().getTime());

            JsObject headers = new JsObject();
            jQuery.ajax(new AjaxSettings {
                type = "GET",
                dataType = "json",
                url = url,
                async = true,
                headers = headers,
                success = delegate(object o, JsString s, jqXHR arg3) { handler(o.As<T>()); },
                error = delegate(jqXHR xhr, JsString s, JsError arg3) {
                    HtmlContext.alert(arg3);
                }
            });
        }

        protected void SendPost<T>( JsString url, object data, JsHandler<T> handler)
        {
            SendInternal("post",  url, data, handler);
        }

        protected void SendDelete<T>(JsString url, JsHandler<T> handler)
        {
            SendInternal("delete", url, null, handler);
        }

        private void SendInternal<T>(string httpMethod,  JsString url, object data, JsHandler<T> handler)
        {
            string serverUrl = _serverUrl;
            url = jsCommonUtils.inst.appendQueryToUrl(serverUrl + url, "time=" + new JsDate().getTime());

            JsObject headers = new JsObject();
            AjaxSettings ajaxSettings = new AjaxSettings
            {
                type = httpMethod,
                dataType = "json",
                data = data,
                url = url,
                headers = headers,
                success = delegate (object o, JsString s, jqXHR arg3) { handler(o.As<T>()); },
                error = delegate (jqXHR xhr, JsString s, JsError arg3) {
                    HtmlContext.alert(arg3);
                }
            };
           

            jQuery.ajax(
                ajaxSettings);
        }

       
    }
}