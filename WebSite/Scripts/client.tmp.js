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

if (typeof($CreateDelegate)=='undefined'){
    if(typeof($iKey)=='undefined') var $iKey = 0;
    if(typeof($pKey)=='undefined') var $pKey = String.fromCharCode(1);
    var $CreateDelegate = function(target, func){
        if (target == null || func == null) 
            return func;
        if(func.target==target && func.func==func)
            return func;
        if (target.$delegateCache == null)
            target.$delegateCache = {};
        if (func.$key == null)
            func.$key = $pKey + String(++$iKey);
        var delegate;
        if(target.$delegateCache!=null)
            delegate = target.$delegateCache[func.$key];
        if (delegate == null){
            delegate = function(){
                return func.apply(target, arguments);
            };
            delegate.func = func;
            delegate.target = target;
            delegate.isDelegate = true;
            if(target.$delegateCache!=null)
                target.$delegateCache[func.$key] = delegate;
        }
        return delegate;
    }
}


if (typeof(FoodApp) == "undefined")
    var FoodApp = {};
if (typeof(FoodApp.Client) == "undefined")
    FoodApp.Client = {};
FoodApp.Client.clientUtils = function (){
};
FoodApp.Client.clientUtils.Inst = new FoodApp.Client.clientUtils();
FoodApp.Client.clientUtils.prototype.getLocation = function (){
    var name = document.location.protocol + "//" + document.location.host;
    return name;
};
FoodApp.Client.clientUtils.prototype.showLoading = function (){
    var loadingEl = document.getElementById("loadingIcon") instanceof HTMLElement ? document.getElementById("loadingIcon") : null;
    loadingEl.style.display = "block";
};
FoodApp.Client.clientUtils.prototype.prettyPrint = function (txt, id){
    var fn = window["prettyPrintEx"];
    fn.call(window, txt, id);
};
FoodApp.Client.clientUtils.prototype.hideLoading = function (){
    window.setTimeout($CreateAnonymousDelegate(this, function (){
        var loadingEl = document.getElementById("loadingIcon") instanceof HTMLElement ? document.getElementById("loadingIcon") : null;
        loadingEl.style.display = "none";
    }), 200);
};
FoodApp.Client.clientUtils.prototype.getSelectedText = function (wnd){
    var text = "";
    if (wnd["getSelection"] != null){
        text = wnd.getSelection().toString();
    }
    else {
        var selection = wnd.document["selection"];
        if (selection != null && (selection["type"] != "Control")){
            var fn = selection["createRange"];
            text = fn.call()["text"];
        }
    }
    return text;
};
FoodApp.Client.clientUtils.prototype.isEmpty = function (str){
    return (null == str) || (undefined == str) || ("" == str) || ("null" == str);
};
FoodApp.Client.ngControllerBase = function (){
    angularjs.angularController.call(this);
};
FoodApp.Client.ngControllerBase.prototype.get_name = function (){
    return "ngControllerBase";
};
FoodApp.Client.ngControllerBase.prototype.get_namespace = function (){
    return "FoodApp.Client";
};
FoodApp.Client.ngControllerBase.prototype.init = function (scope, http, loc, filter){
    angularjs.angularController.prototype.init.call(this, scope, http, loc, filter);
};
FoodApp.Client.ngControllerBase.prototype.onRequestSuccess = function (o, s, arg3){
};
FoodApp.Client.ngControllerBase.prototype.onRequestFailed = function (jsError, jsString, arg3){
};
$Inherit(FoodApp.Client.ngControllerBase, angularjs.angularController);
FoodApp.Client.ngHistoryController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngHistoryController.inst = new FoodApp.Client.ngHistoryController();
FoodApp.Client.ngHistoryController.prototype.get_name = function (){
    return "ngHistoryController";
};
FoodApp.Client.ngHistoryController.prototype.get_ngItems = function (){
    return this._scope["ngItems"];
};
FoodApp.Client.ngHistoryController.prototype.set_ngItems = function (value){
    this._scope["ngItems"] = value;
};
FoodApp.Client.ngHistoryController.prototype.getUrl = function (){
    return "api/history";
};
FoodApp.Client.ngHistoryController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngItems( []);
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refresh();
    }));
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.orderCompleted, $CreateAnonymousDelegate(this, function (n){
        this.refresh();
    }));
};
FoodApp.Client.ngHistoryController.prototype.getFoodItem = function (id){
    var item = FoodApp.Client.ngFoodController.inst.findItemById(id);
    return item;
};
FoodApp.Client.ngHistoryController.prototype.refresh = function (){
    FoodApp.Client.serviceHlp.inst.SendGet("json", this.getUrl() + "/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + FoodApp.Client.ngAppController.inst.get_ngDayOfWeek(), $CreateAnonymousDelegate(this, function (o, s, arg3){
        this.set_ngItems(o);
        this._scope.$apply();
    }), $CreateDelegate(this, this.onRequestFailed));
};
$Inherit(FoodApp.Client.ngHistoryController, FoodApp.Client.ngControllerBase);
FoodApp.Client.ngFavoriteController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngFavoriteController.inst = new FoodApp.Client.ngFavoriteController();
FoodApp.Client.ngFavoriteController.prototype.get_name = function (){
    return "ngFavoriteController";
};
FoodApp.Client.ngFavoriteController.prototype.get_ngItems = function (){
    return this._scope["ngItems"];
};
FoodApp.Client.ngFavoriteController.prototype.set_ngItems = function (value){
    this._scope["ngItems"] = value;
};
FoodApp.Client.ngFavoriteController.prototype.getUrl = function (){
    return "api/favorite";
};
FoodApp.Client.ngFavoriteController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngItems( []);
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refresh();
    }));
};
FoodApp.Client.ngFavoriteController.prototype.getFoodItem = function (id){
    var item = FoodApp.Client.ngFoodController.inst.findItemById(id);
    return item;
};
FoodApp.Client.ngFavoriteController.prototype.refresh = function (){
    FoodApp.Client.serviceHlp.inst.SendGet("json", this.getUrl() + "/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + FoodApp.Client.ngAppController.inst.get_ngDayOfWeek(), $CreateAnonymousDelegate(this, function (o, s, arg3){
        this.set_ngItems(o);
        this._scope.$apply();
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngFavoriteController.prototype.rateChanged = function (rate, newRate){
    rate.Rate = newRate;
    FoodApp.Client.serviceHlp.inst.SendPost("json", this.getUrl() + "/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + rate.FoodId + "/" + rate.Rate, new Object(), $CreateAnonymousDelegate(this, function (){
        FoodApp.Client.ngOrderController.inst.refresh();
    }), $CreateDelegate(this, this.onRequestFailed));
};
$Inherit(FoodApp.Client.ngFavoriteController, FoodApp.Client.ngControllerBase);
FoodApp.Client.eventManager = function (){
    this._dict = new Object();
};
FoodApp.Client.eventManager.dayOfWeekChanged = "dayOfWeekChanged";
FoodApp.Client.eventManager.userIdChanged = "userIdChanged";
FoodApp.Client.eventManager.settingsLoaded = "settingsLoaded";
FoodApp.Client.eventManager.orderCompleted = "orderCompleted";
FoodApp.Client.eventManager.inst = new FoodApp.Client.eventManager();
FoodApp.Client.eventManager.prototype.GetHandlersByName = function (name){
    if (this._dict[name] == null){
        this._dict[name] =  [];
    }
    return this._dict[name];
};
FoodApp.Client.eventManager.prototype.subscribe = function (eventName, action){
    var array = this.GetHandlersByName(eventName);
    array.push(action);
};
FoodApp.Client.eventManager.prototype.fire = function (name, arg){
    var array = this.GetHandlersByName(name);
    for (var $i2 = 0,$l2 = array.length,obj = array[$i2]; $i2 < $l2; $i2++, obj = array[$i2]){
        var action = obj;
        action(arg);
    }
};
FoodApp.Client.ngAppController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngAppController.inst = new FoodApp.Client.ngAppController();
FoodApp.Client.ngAppController.prototype.get_name = function (){
    return "ngAppController";
};
FoodApp.Client.ngAppController.prototype.get_namespace = function (){
    return "FoodApp.Client";
};
FoodApp.Client.ngAppController.prototype.get_ngDayOfWeek = function (){
    return this._scope["ngDayOfWeek"];
};
FoodApp.Client.ngAppController.prototype.set_ngDayOfWeek = function (value){
    this._scope["ngDayOfWeek"] = value;
};
FoodApp.Client.ngAppController.prototype.get_ngUserId = function (){
    return this._scope["ngUserId"];
};
FoodApp.Client.ngAppController.prototype.set_ngUserId = function (value){
    this._scope["ngUserId"] = value;
};
FoodApp.Client.ngAppController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngDayOfWeek(1);
    this.set_ngUserId("_cx0b9");
    window.setTimeout($CreateAnonymousDelegate(this, function (){
        FoodApp.Client.eventManager.inst.fire(FoodApp.Client.eventManager.settingsLoaded, this.get_ngDayOfWeek());
    }), 1);
};
FoodApp.Client.ngAppController.prototype.changeDayOfWeek = function (){
    FoodApp.Client.eventManager.inst.fire(FoodApp.Client.eventManager.dayOfWeekChanged, this.get_ngDayOfWeek());
};
FoodApp.Client.ngAppController.prototype.changeUserId = function (){
    FoodApp.Client.eventManager.inst.fire(FoodApp.Client.eventManager.dayOfWeekChanged, this.get_ngDayOfWeek());
};
$Inherit(FoodApp.Client.ngAppController, FoodApp.Client.ngControllerBase);
FoodApp.Client.serviceHlp = function (){
};
FoodApp.Client.serviceHlp.inst = new FoodApp.Client.serviceHlp();
FoodApp.Client.serviceHlp.prototype.SendGet = function (type, url, success, failed){
    url = FoodApp.Client.serviceHlp.addTimeToUrl(url);
    var headers = new Object();
    $.ajax({
        type: "GET",
        dataType: type,
        url: FoodApp.Client.clientUtils.Inst.getLocation() + "/" + url,
        headers: headers,
        success: $CreateAnonymousDelegate(this, function (o, s, arg3){
            success(o, s, arg3);
        }),
        error: $CreateAnonymousDelegate(this, function (xhr, s, arg3){
            failed(arg3, s, xhr);
        })
    });
};
FoodApp.Client.serviceHlp.prototype.SendPost = function (dataType, url, data, success, failed){
    this.SendInternal("post", dataType, url, data, success, failed);
};
FoodApp.Client.serviceHlp.prototype.SendPut = function (dataType, url, data, success, failed){
    this.SendInternal("put", dataType, url, data, success, failed);
};
FoodApp.Client.serviceHlp.prototype.SendDelete = function (dataType, url, data, success, failed){
    this.SendInternal("delete", dataType, url, data, success, failed);
};
FoodApp.Client.serviceHlp.prototype.SendInternal = function (httpMethod, type, url, data, success, failed){
    url = FoodApp.Client.serviceHlp.addTimeToUrl(url);
    var headers = new Object();
    var ajaxSettings = {
        type: httpMethod,
        dataType: type,
        data: data,
        url: FoodApp.Client.clientUtils.Inst.getLocation() + "/" + url,
        headers: headers,
        success: $CreateAnonymousDelegate(this, function (o, s, arg3){
            success(o, s, arg3);
        }),
        error: $CreateAnonymousDelegate(this, function (xhr, s, arg3){
            failed(arg3, s, xhr);
        })
    };
    var isString = data["toLowerCase"] != null;
    if (isString){
        ajaxSettings.processData = true;
        ajaxSettings.contentType = (type.toLowerCase() == "xml") ? "application/xml" : "application/json";
    }
    $.ajax(ajaxSettings);
};
FoodApp.Client.serviceHlp.addTimeToUrl = function (url){
    if (-1 == url.indexOf("?")){
        url += "?time=" + (new Date()).getTime();
    }
    else {
        url += "&time=" + (new Date()).getTime();
    }
    return url;
};
FoodApp.Client.ngModelBase = function (){
};
if (typeof(FoodApp.Common) == "undefined")
    FoodApp.Common = {};
FoodApp.Common.ngHistoryEntry = function (){
    this.FoodId = null;
    this.Date = System.DateTime.MinValue;
    this.Value = 0;
};
$Inherit(FoodApp.Common.ngHistoryEntry, FoodApp.Client.ngModelBase);
FoodApp.Common.ngFoodRate = function (){
    this.FoodId = null;
    this.Rate = 0;
};
$Inherit(FoodApp.Common.ngFoodRate, FoodApp.Client.ngModelBase);
FoodApp.Common.ngUsersSettingsModel = function (){
    this.FoodRates = null;
    this.UserId = null;
};
FoodApp.Common.ngUsersSettingsModel.prototype.CrateFakeFoodRate = function (){
    if (null == this.FoodRates){
        this.FoodRates = new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngFoodRate");
        var ngFoodItems = FoodApp.Common.FoodManager.Inst.GetFoods(1);
        var $it2 = ngFoodItems.GetEnumerator();
        while ($it2.MoveNext()){
            var foodItem = $it2.get_Current();
            var rate = {};
            rate.FoodId = foodItem.RowId;
            rate.Rate = 0.3;
            this.FoodRates.Add(rate);
        }
    }
};
FoodApp.Common.ngUsersSettingsModel.prototype.GetFoodRateById = function (foodId){
    var res = null;
    var $it3 = this.FoodRates.GetEnumerator();
    while ($it3.MoveNext()){
        var r = $it3.get_Current();
        if (r.FoodId.Equals$$String(foodId)){
            res = r;
            break;
        }
    }
    return res;
};
$Inherit(FoodApp.Common.ngUsersSettingsModel, FoodApp.Client.ngModelBase);
FoodApp.Client.ngFoodItem = function (){
    this.Name = null;
    this.Description = null;
    this.Category = null;
    this.Price = 0;
    this.RowId = null;
    this.FoodId = null;
};
$Inherit(FoodApp.Client.ngFoodItem, FoodApp.Client.ngModelBase);
FoodApp.Client.ngFoodController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngFoodController.inst = new FoodApp.Client.ngFoodController();
FoodApp.Client.ngFoodController.prototype.get_name = function (){
    return "ngFoodController";
};
FoodApp.Client.ngFoodController.prototype.get_namespace = function (){
    return "FoodApp.Client";
};
FoodApp.Client.ngFoodController.prototype.get_ngItems = function (){
    return this._scope["ngItems"];
};
FoodApp.Client.ngFoodController.prototype.set_ngItems = function (value){
    this._scope["ngItems"] = value;
};
FoodApp.Client.ngFoodController.prototype.buyClick = function (rowId){
    FoodApp.Client.serviceHlp.inst.SendPost("json", this.getUrl() + "/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + FoodApp.Client.ngAppController.inst.get_ngDayOfWeek() + "/" + rowId, new Object(), $CreateAnonymousDelegate(this, function (){
        FoodApp.Client.ngOrderController.inst.refresh();
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngFoodController.prototype.getUrl = function (){
    return "api/foods";
};
FoodApp.Client.ngFoodController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngItems( []);
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.dayOfWeekChanged, $CreateAnonymousDelegate(this, function (n){
        this.refresh(null);
    }));
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.userIdChanged, $CreateAnonymousDelegate(this, function (n){
        this.refresh(null);
    }));
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refresh($CreateAnonymousDelegate(this, function (){
            var fn = window["initMenu"];
            fn.call();
        }));
    }));
};
FoodApp.Client.ngFoodController.prototype.refresh = function (complete){
    FoodApp.Client.serviceHlp.inst.SendGet("json", this.getUrl() + "/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + FoodApp.Client.ngAppController.inst.get_ngDayOfWeek(), $CreateAnonymousDelegate(this, function (o, s, arg3){
        this.set_ngItems(o);
        this._scope.$apply();
        if (null != complete){
            complete();
        }
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngFoodController.prototype.change = function (){
    this.refresh(null);
};
FoodApp.Client.ngFoodController.prototype.findItemById = function (id){
    var res = null;
    for (var $i5 = 0,$t5 = this.get_ngItems(),$l5 = $t5.length,item = $t5[$i5]; $i5 < $l5; $i5++, item = $t5[$i5]){
        if (item.RowId == id){
            res = item;
            break;
        }
    }
    return res;
};
FoodApp.Client.ngFoodController.prototype.getFoodByDay = function (day){
    var res =  [];
    if (this.get_ngItems().length > 5){
        res.push(this.get_ngItems()[0]);
        res.push(this.get_ngItems()[1]);
        res.push(this.get_ngItems()[2]);
    }
    return res;
};
$Inherit(FoodApp.Client.ngFoodController, FoodApp.Client.ngControllerBase);
FoodApp.Client.ngOrderController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngOrderController.inst = new FoodApp.Client.ngOrderController();
FoodApp.Client.ngOrderController.prototype.get_name = function (){
    return "ngOrderController";
};
FoodApp.Client.ngOrderController.prototype.get_ngOrderModel = function (){
    return this._scope["ngOrderModel"];
};
FoodApp.Client.ngOrderController.prototype.set_ngOrderModel = function (value){
    this._scope["ngOrderModel"] = value;
};
FoodApp.Client.ngOrderController.prototype.get_ngItems = function (){
    return this._scope["ngItems"];
};
FoodApp.Client.ngOrderController.prototype.set_ngItems = function (value){
    this._scope["ngItems"] = value;
};
FoodApp.Client.ngOrderController.prototype.getUrl = function (){
    return "api/orders";
};
FoodApp.Client.ngOrderController.prototype.getFoodItem = function (id){
    var item = FoodApp.Client.ngFoodController.inst.findItemById(id);
    return item;
};
FoodApp.Client.ngOrderController.prototype.getTotalPrice = function (){
    var res = 0;
    for (var $i6 = 0,$t6 = this.get_ngItems(),$l6 = $t6.length,item = $t6[$i6]; $i6 < $l6; $i6++, item = $t6[$i6]){
        var food = this.getFoodItem(item.FoodId);
        if (null != food){
            res += (food.Price * item.Count);
        }
    }
    return res;
};
FoodApp.Client.ngOrderController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngOrderModel({});
    this.set_ngItems( []);
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.dayOfWeekChanged, $CreateAnonymousDelegate(this, function (n){
        this.refresh();
    }));
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refresh();
    }));
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.orderCompleted, $CreateAnonymousDelegate(this, function (n){
        this.refresh();
    }));
};
FoodApp.Client.ngOrderController.prototype.deleteOrder = function (id){
    FoodApp.Client.serviceHlp.inst.SendDelete("json", this.getUrl() + "/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + FoodApp.Client.ngAppController.inst.get_ngDayOfWeek() + "/" + id, new Object(), $CreateAnonymousDelegate(this, function (){
        this.refresh();
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngOrderController.prototype.completeOrder = function (){
    FoodApp.Client.serviceHlp.inst.SendPost("json", this.getUrl() + "/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + FoodApp.Client.ngAppController.inst.get_ngDayOfWeek(), new Object(), $CreateAnonymousDelegate(this, function (){
        FoodApp.Client.eventManager.inst.fire(FoodApp.Client.eventManager.orderCompleted, "");
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngOrderController.prototype.refresh = function (){
    FoodApp.Client.serviceHlp.inst.SendGet("json", this.getUrl() + "/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + FoodApp.Client.ngAppController.inst.get_ngDayOfWeek(), $CreateAnonymousDelegate(this, function (o, s, arg3){
        this.set_ngItems(o);
        this._scope.$apply();
    }), $CreateDelegate(this, this.onRequestFailed));
};
$Inherit(FoodApp.Client.ngOrderController, FoodApp.Client.ngControllerBase);
FoodApp.Client.ngOrderModel = function (){
    this.Count = 0;
    this.FoodId = null;
};
$Inherit(FoodApp.Client.ngOrderModel, FoodApp.Client.ngModelBase);

