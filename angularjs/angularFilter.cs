using SharpKit.JavaScript;

namespace angularjs
{
    [JsType(Export = false)]
    public abstract class angularFilter : JsObject
    {
        public abstract string className { get; }
        public abstract string filterName { get; }
        public abstract string @namespace { get; }

        public virtual object filter(JsObject obj, JsObject arg)
        {
            return obj;
        }
    }
}