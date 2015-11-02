using angularjs.Properties;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace angularjs
{
    [JsType(JsMode.Prototype, Filename = angularjsResources._fileClientBeforeJs, OrderInFile = -10)]
    public class clientAppHelper
    {
        public static readonly clientAppHelper inst = new clientAppHelper();
        public JsArray<angularController> _controllers = new JsArray<angularController>();
        public const string c_sModuleName = "StudyLogApiApp";
        public bool isDebug = true;

        private clientAppHelper() {
        }

        public angularModule module { get; set; }

        public void registerController(angularController angularController) {
            _controllers.Add(angularController);
        }

        private void linkWithAngularJs(angularController angularController) {
            var func1 = new JsFunction("$scope", "$http","$location","$filter",
                angularController.@namespace + "." + angularController.name + ".inst.init($scope,$http,$location,$filter)");
            string str = c_sModuleName + ".controller('" + angularController.name + "',func1);";
            JsContext.eval(str);
        }

        public void before() {
            this.module = angular.module(c_sModuleName, new JsArray());
            HtmlContext.window[c_sModuleName] = this.module;
        }

        public void after() {
            for (int i = 0; i < _controllers.length; ++i) {
                linkWithAngularJs(_controllers[i]);
            }

            addFilter();
        }

        private static void addFilter() {
            HtmlContext.eval(
                c_sModuleName +".filter('rawHtml', ['$sce', function($sce){return function(val) {return $sce.trustAsHtml(val);};}]);");
        }
    }

    [JsType(JsMode.Prototype, Filename = angularjsResources._fileClientBeforeJs, OrderInFile = -9)]
    public class ProgramBefore
    {
        [JsMethod(GlobalCode = true)]
        public static void Main()
        {
            clientAppHelper.inst.before();
        }
    }

    [JsType(JsMode.Prototype, Filename = angularjsResources._fileClientAfterJs, OrderInFile = int.MaxValue)]
    public class ProgramAfter
    {
        [JsMethod(GlobalCode = true)]
        public static void Main() {
            clientAppHelper.inst.after();
        }
    }
}