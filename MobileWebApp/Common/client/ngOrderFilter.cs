using angularjs;
using FoodApp.Common;
using SharpKit.JavaScript;

namespace MobileWebApp.Common.client
{
    [JsType(JsMode.Prototype, Filename = MobileApiResources._fileClientTmpJs)]
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
            get { return MobileApiResources.@namespace; }
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
                    ngFoodItem foodItem = ngMobileFoodController.inst.findFoodById(order.FoodId);
                    if (null != foodItem && !foodItem.isContainer) {
                        res.Add(order);
                    }
                }
            }

            return res;
        }
    }
}