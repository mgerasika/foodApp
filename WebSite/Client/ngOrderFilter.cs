using angularjs;
using FoodApp.Common;
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
            JsArray<ngOrderModel> res = new JsArray<ngOrderModel>();
            int day = arg["day"].As<int>();
            JsArray<JsArray<ngOrderModel>> allOrders = obj.As<JsArray<JsArray<ngOrderModel>>>();
            JsArray<ngOrderModel> tmp = allOrders[day];
            if (tmp != null && tmp.length > 0) {
                foreach (ngOrderModel order in tmp) {
                    ngFoodItem foodItem = ngFoodController.inst.findFoodById(order.FoodId);
                    if (!foodItem.isContainer) {
                        res.Add(order);
                    }
                }
            }

            return res;
        }
    }
}