using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class serviceHlp
    {
        public static serviceHlp inst = new serviceHlp();

        private serviceHlp()
        {
        }

       
        

        public void SendGet(string type, JsString url, JsAction<object, JsString, jqXHR> success,
            JsAction<JsError, JsString, jqXHR> failed)
        {
            url = addTimeToUrl(url);

            JsObject headers = new JsObject();
            jQuery.ajax(new AjaxSettings
            {
                type = "GET",
                dataType = type,
                url = jsUtils.inst.getLocation() + "/" + url,
                headers = headers,
                success = delegate(object o, JsString s, jqXHR arg3) { success(o, s, arg3); },
                error = delegate(jqXHR xhr, JsString s, JsError arg3) { failed(arg3, s, xhr); }
            });
        }

        public void SendPost(string dataType, JsString url, object data, JsAction<object, JsString, jqXHR> success,
            JsAction<JsError, JsString, jqXHR> failed)
        {
            SendInternal("post", dataType, url, data, success, failed);
        }

        public void SendPut(string dataType, JsString url, object data, JsAction<object, JsString, jqXHR> success,
            JsAction<JsError, JsString, jqXHR> failed)
        {
            SendInternal("put", dataType, url, data, success, failed);
        }

        public void SendDelete(string dataType, JsString url, object data, JsAction<object, JsString, jqXHR> success,
            JsAction<JsError, JsString, jqXHR> failed)
        {
            SendInternal("delete", dataType, url, data, success, failed);
        }

        private void SendInternal(string httpMethod, string type, JsString url, object data,
            JsAction<object, JsString, jqXHR> success,
            JsAction<JsError, JsString, jqXHR> failed)
        {
            url = addTimeToUrl(url);

            JsObject headers = new JsObject();
            AjaxSettings ajaxSettings = new AjaxSettings
            {
                type = httpMethod,
                dataType = type,
                data = data,
                url = jsUtils.inst.getLocation() + "/" + url,
                headers = headers,
                success = delegate(object o, JsString s, jqXHR arg3) { success(o, s, arg3); },
                error = delegate(jqXHR xhr, JsString s, JsError arg3) { failed(arg3, s, xhr); }
            };
            bool isString = data.As<JsObject>()["toLowerCase"] != null;
            if (isString)
            {
                ajaxSettings.processData = true;
                ajaxSettings.contentType = (type.As<JsString>().toLowerCase() == "xml")
                    ? "application/xml"
                    : "application/json";
            }

            jQuery.ajax(
                ajaxSettings);
        }

        public static JsString addTimeToUrl(JsString url)
        {
            if (-1 == url.indexOf("?"))
            {
                url += "?" + "time=" + (new JsDate()).getTime();
            }
            else
            {
                url += "&" +  "time=" + (new JsDate()).getTime();
            }
            return url;
        }
    }
}