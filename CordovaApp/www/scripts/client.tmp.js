/* Generated by SharpKit 5 v5.5.0 */
if (typeof ($Inherit) == 'undefined') {
	var $Inherit = function (ce, ce2) {

		if (typeof (Object.getOwnPropertyNames) == 'undefined') {

			for (var p in ce2.prototype)
				if (typeof (ce.prototype[p]) == 'undefined' || ce.prototype[p] == Object.prototype[p])
					ce.prototype[p] = ce2.prototype[p];
			for (var p in ce2)
				if (typeof (ce[p]) == 'undefined')
					ce[p] = ce2[p];
			ce.$baseCtor = ce2;

		} else {

			var props = Object.getOwnPropertyNames(ce2.prototype);
			for (var i = 0; i < props.length; i++)
				if (typeof (Object.getOwnPropertyDescriptor(ce.prototype, props[i])) == 'undefined')
					Object.defineProperty(ce.prototype, props[i], Object.getOwnPropertyDescriptor(ce2.prototype, props[i]));

			for (var p in ce2)
				if (typeof (ce[p]) == 'undefined')
					ce[p] = ce2[p];
			ce.$baseCtor = ce2;

		}

	}
};

if (typeof ($CreateAnonymousDelegate) == 'undefined') {
    var $CreateAnonymousDelegate = function (target, func) {
        if (target == null || func == null)
            return func;
        var delegate = function () {
            return func.apply(target, arguments);
        };
        delegate.func = func;
        delegate.target = target;
        delegate.isDelegate = true;
        return delegate;
    }
}


if (typeof(MobileWebApp) == "undefined")
    var MobileWebApp = {};
if (typeof(MobileWebApp.Common) == "undefined")
    MobileWebApp.Common = {};
if (typeof(MobileWebApp.Common.client) == "undefined")
    MobileWebApp.Common.client = {};
MobileWebApp.Common.client.ngMobileControllerBase = function (){
    angularjs.angularController.call(this);
};
MobileWebApp.Common.client.ngMobileControllerBase.prototype.get_className = function (){
    return "ngMobileControllerBase";
};
MobileWebApp.Common.client.ngMobileControllerBase.prototype.get_namespace = function (){
    return "MobileWebApp.Common.client";
};
MobileWebApp.Common.client.ngMobileControllerBase.prototype.init = function (scope, http, loc, filter){
    angularjs.angularController.prototype.init.call(this, scope, http, loc, filter);
};
MobileWebApp.Common.client.ngMobileControllerBase.prototype.onRequestSuccess = function (o, s, arg3){
};
MobileWebApp.Common.client.ngMobileControllerBase.prototype.onRequestFailed = function (jsError, jsString, arg3){
};
$Inherit(MobileWebApp.Common.client.ngMobileControllerBase, angularjs.angularController);
MobileWebApp.Common.client.ngMobileAppController = function (){
    MobileWebApp.Common.client.ngMobileControllerBase.call(this);
};
MobileWebApp.Common.client.ngMobileAppController.inst = new MobileWebApp.Common.client.ngMobileAppController();
MobileWebApp.Common.client.ngMobileAppController.prototype.get_className = function (){
    return "ngMobileAppController";
};
MobileWebApp.Common.client.ngMobileAppController.prototype.get_ngUserId = function (){
    return this._scope["ngUserId"];
};
MobileWebApp.Common.client.ngMobileAppController.prototype.set_ngUserId = function (value){
    this._scope["ngUserId"] = value;
};
MobileWebApp.Common.client.ngMobileAppController.prototype.init = function (scope, http, loc, filter){
    MobileWebApp.Common.client.ngMobileControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngUserId(document.getElementById("userId").value);
    FoodApp.Common.JsService.Inst.Init("http://www.gam-gam.lviv.ua/", this.get_ngUserId());
    window.setTimeout($CreateAnonymousDelegate(this, function (){
        FoodApp.Common.eventManager.inst.fire(FoodApp.Common.eventManager.settingsLoaded, "");
    }), 1);
};
$Inherit(MobileWebApp.Common.client.ngMobileAppController, MobileWebApp.Common.client.ngMobileControllerBase);
MobileWebApp.Common.client.ngMobileFoodController = function (){
    MobileWebApp.Common.client.ngMobileControllerBase.call(this);
};
MobileWebApp.Common.client.ngMobileFoodController.inst = new MobileWebApp.Common.client.ngMobileFoodController();
MobileWebApp.Common.client.ngMobileFoodController.prototype.get_className = function (){
    return "ngMobileFoodController";
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.get_ngFoods = function (){
    return this._scope["ngFoods"];
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.set_ngFoods = function (value){
    this._scope["ngFoods"] = value;
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.get_ngCategories = function (){
    return this._scope["ngCategories"];
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.set_ngCategories = function (value){
    this._scope["ngCategories"] = value;
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.buyClick = function (day, foodId, value){
    FoodApp.Common.jsUtils.inst.showLoading();
    FoodApp.Common.JsService.Inst.FoodApi.Buy(day, foodId, value, $CreateAnonymousDelegate(this, function (){
        FoodApp.Common.jsUtils.inst.hideLoading();
        MobileWebApp.Common.client.ngMobileOrderController.inst.refreshOrders();
    }));
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.hasOrder = function (day, foodId){
    var res = false;
    var order = MobileWebApp.Common.client.ngMobileOrderController.inst.getOrderByFoodId(day, foodId);
    if (null != order){
        res = true;
    }
    return res;
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.init = function (scope, http, loc, filter){
    MobileWebApp.Common.client.ngMobileControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngFoods( []);
    this.set_ngCategories( []);
    this.get_ngCategories().push("Салати");
    this.get_ngCategories().push("Перші страви");
    this.get_ngCategories().push("Гарніри");
    this.get_ngCategories().push("Мясо/Риба");
    this.get_ngCategories().push("Комплексний");
    this.get_ngCategories().push("Хліб");
    FoodApp.Common.eventManager.inst.subscribe(FoodApp.Common.eventManager.deviceReady, $CreateAnonymousDelegate(this, function (n){
        this.refreshFoods($CreateAnonymousDelegate(this, function (){
        }));
    }));
    FoodApp.Common.eventManager.inst.subscribe(FoodApp.Common.eventManager.orderListChanged, $CreateAnonymousDelegate(this, function (n){
        this._scope.$apply();
    }));
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.getPrefix = function (price){
    var res = price;
    if (res.indexOf(".") > 0){
        res = res.substr(0, res.indexOf("."));
    }
    return res;
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.getSuffix = function (price){
    var res = price;
    if (res.indexOf(".") > 0){
        res = res.substr(res.indexOf(".") + 1);
    }
    else {
        res = "";
    }
    return res;
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.refreshFoods = function (complete){
    FoodApp.Common.JsService.Inst.FoodApi.GetAllFoods($CreateAnonymousDelegate(this, function (data){
        this.set_ngFoods(data);
        this._scope.$apply();
        if (null != complete){
            complete();
        }
    }));
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.findFoodById = function (id){
    var res = null;
    for (var $i2 = 0,$t2 = this.get_ngFoods(),$l2 = $t2.length,dayItems = $t2[$i2]; $i2 < $l2; $i2++, dayItems = $t2[$i2]){
        for (var $i3 = 0,$l3 = dayItems.length,item = dayItems[$i3]; $i3 < $l3; $i3++, item = dayItems[$i3]){
            if (item.FoodId == id){
                res = item;
                break;
            }
        }
        if (null != res){
            break;
        }
    }
    return res;
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.getOrderCount = function (day, foodId){
    var res = 0;
    var order = MobileWebApp.Common.client.ngMobileOrderController.inst.getOrderByFoodId(day, foodId);
    if (null != order){
        res = order.Count;
    }
    return res;
};
MobileWebApp.Common.client.ngMobileFoodController.prototype.changePrice = function (day, ngFoodItem){
    FoodApp.Common.jsUtils.inst.showLoading();
    FoodApp.Common.JsService.Inst.FoodApi.ChangePrice(day, ngFoodItem.FoodId, ngFoodItem.Price, $CreateAnonymousDelegate(this, function (data){
        FoodApp.Common.jsUtils.inst.hideLoading();
        this.refreshFoods($CreateAnonymousDelegate(this, function (){
        }));
    }));
};
$Inherit(MobileWebApp.Common.client.ngMobileFoodController, MobileWebApp.Common.client.ngMobileControllerBase);
MobileWebApp.Common.client.ngMobileOrderController = function (){
    MobileWebApp.Common.client.ngMobileControllerBase.call(this);
};
MobileWebApp.Common.client.ngMobileOrderController.inst = new MobileWebApp.Common.client.ngMobileOrderController();
MobileWebApp.Common.client.ngMobileOrderController.prototype.get_className = function (){
    return "ngMobileOrderController";
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.get_ngOrderEntries = function (){
    return this._scope["ngOrderEntries"];
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.set_ngOrderEntries = function (value){
    this._scope["ngOrderEntries"] = value;
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.getFoodItem = function (id){
    var item = MobileWebApp.Common.client.ngMobileFoodController.inst.findFoodById(id);
    return item;
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.formatCount = function (order){
    var res = order.Count + "";
    var food = this.getFoodItem(order.FoodId);
    if (food.IsByWeightItem){
        res = parseInt((order.Count * 100), 10) + "";
    }
    return res;
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.getTotalPrice = function (day){
    var res = 0;
    var ngOrderModels = this.get_ngOrderEntries()[day];
    if (ngOrderModels != null){
        for (var $i4 = 0,$l4 = ngOrderModels.length,item = ngOrderModels[$i4]; $i4 < $l4; $i4++, item = ngOrderModels[$i4]){
            var food = this.getFoodItem(item.FoodId);
            if (null != food){
                res += FoodApp.Common.jsUtils.inst.fixNumber(food.Price * item.Count);
                res = FoodApp.Common.jsUtils.inst.fixNumber(res);
            }
        }
    }
    res = FoodApp.Common.jsUtils.inst.fixNumber(res);
    return res;
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.init = function (scope, http, loc, filter){
    MobileWebApp.Common.client.ngMobileControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngOrderEntries( []);
    FoodApp.Common.eventManager.inst.subscribe(FoodApp.Common.eventManager.deviceReady, $CreateAnonymousDelegate(this, function (n){
        this.refreshOrders();
    }));
    FoodApp.Common.eventManager.inst.subscribe(FoodApp.Common.eventManager.orderCompleted, $CreateAnonymousDelegate(this, function (n){
        this.refreshOrders();
    }));
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.deleteOrder = function (day, foodId){
    FoodApp.Common.jsUtils.inst.showLoading();
    FoodApp.Common.JsService.Inst.OrderApi.DeleteOrder(day, foodId, $CreateAnonymousDelegate(this, function (s){
        FoodApp.Common.jsUtils.inst.hideLoading();
        this.refreshOrders();
    }));
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.refreshOrders = function (){
    FoodApp.Common.JsService.Inst.OrderApi.GetOrders($CreateAnonymousDelegate(this, function (tmp){
        while (this.get_ngOrderEntries().length > 0){
            this.get_ngOrderEntries().pop();
        }
        for (var $i5 = 0,$l5 = tmp.length,obj = tmp[$i5]; $i5 < $l5; $i5++, obj = tmp[$i5]){
            this.get_ngOrderEntries().push(obj);
        }
        FoodApp.Common.eventManager.inst.fire(FoodApp.Common.eventManager.orderListChanged, this.get_ngOrderEntries());
        this._scope.$apply();
    }));
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.getOrders = function (day){
    return this.get_ngOrderEntries()[day];
};
MobileWebApp.Common.client.ngMobileOrderController.prototype.getOrderByFoodId = function (day, foodId){
    var res = null;
    var orders = MobileWebApp.Common.client.ngMobileOrderController.inst.getOrders(day);
    for (var $i6 = 0,$l6 = orders.length,order = orders[$i6]; $i6 < $l6; $i6++, order = orders[$i6]){
        if (order.FoodId == foodId){
            res = order;
            break;
        }
    }
    return res;
};
$Inherit(MobileWebApp.Common.client.ngMobileOrderController, MobileWebApp.Common.client.ngMobileControllerBase);
function Main(){
    window.onerror = function (evt){
        alert("error:" + evt);
    };
    window.setTimeout(function (){
        FoodApp.Common.eventManager.inst.fire(FoodApp.Common.eventManager.deviceReady, "");
    }, 1);
};
var isSharp = -1 != window.location.href.indexOf("cshtml");
if (isSharp){
    Main();
}
MobileWebApp.Common.client.ngOrderFilter = function (){
};
MobileWebApp.Common.client.ngOrderFilter.prototype.get_className = function (){
    return "ngOrderFilter";
};
MobileWebApp.Common.client.ngOrderFilter.prototype.get_namespace = function (){
    return "MobileWebApp.Common.client";
};
MobileWebApp.Common.client.ngOrderFilter.prototype.get_filterName = function (){
    return "orderFilter";
};
angularUtils.inst.registerFilterType(new MobileWebApp.Common.client.ngOrderFilter());
MobileWebApp.Common.client.ngOrderFilter.prototype.filter = function (obj, arg){
    var res =  [];
    var day = arg["day"];
    var allOrders = obj;
    var tmp = allOrders[day];
    if (tmp != null && tmp.length > 0){
        for (var $i7 = 0,$l7 = tmp.length,order = tmp[$i7]; $i7 < $l7; $i7++, order = tmp[$i7]){
            var foodItem = MobileWebApp.Common.client.ngMobileFoodController.inst.findFoodById(order.FoodId);
            if (null != foodItem && !foodItem.isContainer){
                res.push(order);
            }
        }
    }
    return res;
};

