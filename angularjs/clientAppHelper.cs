using System;
using angularjs.Properties;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace angularjs
{
    [JsType(JsMode.Prototype, Filename = angularjsResources._fileClientBeforeJs, OrderInFile = -10,
        Name = "angularUtils")]
    public class angularUtils
    {
        public const string c_sModuleName = "StudyLogApiApp";
        public static readonly angularUtils inst = new angularUtils();
        public JsArray<angularController> _controllers = new JsArray<angularController>();
        public JsArray<angularDirective> _directives = new JsArray<angularDirective>();
        public JsArray<angularFilter> _filters = new JsArray<angularFilter>();

        private angularUtils() {
        }

        public angularModule module { get; set; }

        public void registerControllerType(angularController controller) {
            _controllers.Add(controller);
        }

        public void registerDirectiveType(angularDirective directive) {
            _directives.Add(directive);
        }

        public void registerFilterType(angularFilter filter) {
            _filters.Add(filter);
        }

        private void linkControllerWithAngularJs(angularController angularController) {
            JsFunction func1 = new JsFunction("$scope", "$http", "$location", "$filter",
                "var cstype = '" + angularController.@namespace + "." + angularController.className + "';" +
                "angularUtils.inst.createController(cstype,$scope,$http,$location,$filter);");
            string str = c_sModuleName + ".controller('" + angularController.className + "',func1);";
            JsContext.eval(str);
        }

        private void createController(string jstype, angularScope scope, angularHttp http, angularLocation loc,
            angularFilter filter) {
            JsObject type = getJsType(jstype);
            object obj = null;
            if (type["inst"] != null) {
                obj = type["inst"];
            }
            else {
                obj = createInstance(jstype);
            }

            angularController ctrl = obj.As<angularController>();
            ctrl.init(scope, http, loc, filter);
        }

        private JsObject getJsType(JsString cstype) {
            JsArray<string> arr = cstype.split(".").As<JsArray<string>>();
            JsObject res = HtmlContext.window.As<JsObject>();
            for (int i = 0; i < arr.length; ++i) {
                string key = arr[i];
                JsObject tmpObj = res[key].As<JsObject>();
                res = tmpObj;
            }
            return res;
        }

        public void linkDirectiveWithAngularJs(angularDirective directive) {
            angularModule module = HtmlContext.window.As<JsObject>()[c_sModuleName].As<angularModule>();
            module.directive(directive.attrName, delegate {
                JsObject obj = createInstance(directive.@namespace + "." + directive.className);
                obj.As<angularDirective>().initDirectiveInternal();
                return obj;
            });
        }

        public void linkFilterWithAngularJs(angularFilter filter)
        {
            angularModule module = HtmlContext.window.As<JsObject>()[c_sModuleName].As<angularModule>();
            module.filter(filter.filterName, delegate
            {
                JsObject obj = createInstance(filter.@namespace + "." + filter.className);
                JsFunction fn = obj["filter"].As<JsFunction>();
                return fn;
            });
        }

        public JsObject createInstance(string cstype) {
            string obj = "var obj = new " + cstype + "()";
            try {
                JsContext.eval(obj);
            }
            catch (Exception ex) {
                obj = null;
            }
            return obj.As<JsObject>();
        }

        public void before() {
            module = angular.module(c_sModuleName, new JsArray());
            HtmlContext.window[c_sModuleName] = module;
        }

        public void after() {
            for (int i = 0; i < _controllers.length; ++i) {
                linkControllerWithAngularJs(_controllers[i]);
            }

            for (int i = 0; i < _directives.length; ++i) {
                linkDirectiveWithAngularJs(_directives[i]);
            }

            for (int i = 0; i < _filters.length; ++i)
            {
                linkFilterWithAngularJs(_filters[i]);
            }

            addFilter();
        }

        private static void addFilter() {
            JsContext.eval(
                c_sModuleName +
                ".filter('rawHtml', ['$sce', function($sce){return function(val) {return $sce.trustAsHtml(val);};}]);");
        }
    }

    [JsType(JsMode.Prototype, Filename = angularjsResources._fileClientBeforeJs, OrderInFile = -9)]
    public class ProgramBefore
    {
        [JsMethod(GlobalCode = true)]
        public static void Main() {
            angularUtils.inst.before();
        }
    }

    [JsType(JsMode.Prototype, Filename = angularjsResources._fileClientAfterJs, OrderInFile = int.MaxValue)]
    public class ProgramAfter
    {
        [JsMethod(GlobalCode = true)]
        public static void Main() {
            angularUtils.inst.after();
        }
    }
}