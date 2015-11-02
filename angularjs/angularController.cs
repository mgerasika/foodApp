using angularjs.Properties;
using SharpKit.JavaScript;

namespace angularjs
{
    [JsType(JsMode.Prototype, Filename = angularjsResources._fileClientBeforeJs)]
    public abstract class angularController
    {
        public angularScope _scope = null;
        public angularHttp _http = null;
        public angularLocation _location = null;
        public angularFilter Filter = null;

        protected angularController() {
            clientAppHelper.inst.registerController(this);
        }

        protected object _model {
            get {
                JsObject scope = _scope as JsObject;
                object obj = scope["model"];
                return obj;
            }
            set {
                JsObject scope = _scope as JsObject;
                scope["model"] = value;
            }
        }
        
        public virtual void init(angularScope scope, angularHttp http, angularLocation loc,angularFilter filter) {
            _scope = scope;
            _http = http;
            _location = loc;
            Filter = filter;

            JsObject copy = this.As<JsObject>();
            foreach (var key in copy)
            {
                scope[key] = copy[key];
            }

        }

        public abstract string name { get; }
        public abstract string @namespace { get; }
    }
}