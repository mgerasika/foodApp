using SharpKit.JavaScript;

namespace angularjs
{
    [JsType(Export = false)]
    public class angularScope : JsObject
    {

        [JsMethod(Name = "$apply")]
        public void apply()
        {
            throw new System.NotImplementedException();
        }
    }
}