using angularjs;
using FoodApp.Common;
using FoodApp.Common.Model;
using SharpKit.JavaScript;

namespace FoodApp.Client
{
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngOrderFilter : angularFilter
    {
        protected ngOrderFilter()
        {
        }

        public override string className
        {
            get { return "ngOrderFilter"; }
        }

        public override string @namespace
        {
            get { return WebApiResources.@namespace; }
        }

        public override string filterName
        {
            get { return "orderFilter"; }
        }

        [JsMethod(GlobalCode = true)]
        private static void register()
        {
            angularUtils.inst.registerFilterType(new ngOrderFilter());
        }

        public override object filter(JsObject obj, JsObject arg)
        {
            JsArray<ngOrderEntry> res = new JsArray<ngOrderEntry>();
            int day = arg["day"].As<int>();
            JsArray<JsArray<ngOrderEntry>> allOrders = obj.As<JsArray<JsArray<ngOrderEntry>>>();
            JsArray<ngOrderEntry> tmp = allOrders[day];
            if (tmp != null && tmp.length > 0) {
                foreach (ngOrderEntry order in tmp) {
                    ngFoodItem foodItem = ngFoodController.inst.findFoodById(order.FoodId);
                    if (null != foodItem && !foodItem.isContainer) {
                        res.Add(order);
                    }
                }
            }

            return res;
        }
    }
}