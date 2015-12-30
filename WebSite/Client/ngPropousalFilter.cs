using angularjs;
using FoodApp.Common;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngFoodFilter : angularFilter {
        protected ngFoodFilter() {
        }

        public override string className {
            get { return "ngPropousalFilter"; }
        }

        public override string @namespace {
            get { return WebApiResources.@namespace; }
        }

        public override string filterName {
            get { return "foodFilter"; }
        }

        [JsMethod(GlobalCode = true)]
        private static void register() {
            angularUtils.inst.registerFilterType(new ngFoodFilter());
        }

        public override object filter(JsObject obj, JsObject arg) {
            JsArray<ngFoodItem> res = new JsArray<ngFoodItem>();

            string category = arg["category"].As<JsString>();
            int day = arg["day"].As<int>();
            JsArray<JsArray<ngFoodItem>> allFoods = obj.As<JsArray<JsArray<ngFoodItem>>>();
            JsArray<ngFoodItem> items = allFoods[day];
            if (null != items) {
                foreach (ngFoodItem item in items) {
                    if ((item.Category == category) && !item.isContainer) {
                        res.Add(item);
                    }
                }
            }
            return res;
        }
    }

    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngPropousalFilter : angularFilter
    {
        protected ngPropousalFilter()
        {
        }

        public override string className
        {
            get { return "ngPropousalFilter"; }
        }

        public override string @namespace
        {
            get { return WebApiResources.@namespace; }
        }

        public override string filterName
        {
            get { return "propousalFilter"; }
        }

        [JsMethod(GlobalCode = true)]
        private static void register()
        {
            angularUtils.inst.registerFilterType(new ngPropousalFilter());
        }

        public override object filter(JsObject obj, JsObject arg)
        {
            JsContext.debugger();

            int day = arg["day"].As<int>();

            JsArray<JsArray<ngHistoryEntry>> allPropousals = obj.As<JsArray<JsArray<ngHistoryEntry>>>();
            JsArray<ngHistoryEntry> res = allPropousals[day];
            return res;
        }
    }
}