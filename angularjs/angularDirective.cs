using angularjs.Properties;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace angularjs
{
    [JsType(JsMode.Prototype, Filename = angularjsResources._fileClientBeforeJs)]
    public abstract class angularDirective
    {
        protected JsObject _attrs;
        protected HtmlElement _element;
        protected angularScope _scope;

        protected virtual string restrict {
            get { return "A"; }
        }

        protected virtual object getScope {
            get { return true; }
        }

        public string attrName {
            get {
                string res;
                if (className.As<JsString>() != null) {
                    res = className.As<JsString>().toLowerCase();
                }
                else {
                    res = className.ToLower();
                }
                return res;
            }
        }

        public abstract string className { get; }
        public abstract string @namespace { get; }
        protected virtual object template(HtmlElement element, JsObject attrs)
        {
            object res = new jQuery("#" + getTemplateId()).html();
            return res;
        }

        public string getTemplateId() {
            return "_id" + this.className;
        }

        public void initDirectiveInternal() {
            JsObject self = this.As<JsObject>();
            self["restrict"] = this.restrict;
            self["scope"] = this.getScope;
        }

        protected virtual void init(angularScope s, HtmlElement element, JsObject attrs) {
            _scope = s;
            _element = element;
            _attrs = attrs;
        }

        private void link(angularScope s, HtmlElement element, JsObject attrs) {
            
            string cstype = attrs["cstype"].As<string>();
            angularDirective obj = angularUtils.inst.createInstance(cstype).As<angularDirective>();
            obj.init(s,element,attrs);
        }

        
    }
}