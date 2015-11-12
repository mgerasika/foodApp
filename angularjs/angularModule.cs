using SharpKit.Html;
using SharpKit.JavaScript;

namespace angularjs
{
    [JsDelegate(NativeDelegates = true)]
    [JsType(JsMode.Json, Export = false, OmitCasts = true)]
    public delegate object JsActionWithResult();

    [JsType(Export = false)]
    public class angularModule : JsObject
    {
        /*
         
         */
        public void controller(string addordercontroller,JsAction<angularScope,angularHttp,angularLocation,angularFilter> action) {
        }

        public void directive(string name, JsActionWithResult action)
        {
        }

        public void filter(string name, JsActionWithResult action)
        {
        }
    }
}