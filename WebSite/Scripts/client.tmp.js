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
    var loadingEl = document.getElementById("loadingDiv") instanceof HTMLElement ? document.getElementById("loadingDiv") : null;
    loadingEl.style.display = "block";
};
FoodApp.Client.clientUtils.prototype.hideLoading = function (){
    window.setTimeout($CreateAnonymousDelegate(this, function (){
        var loadingEl = document.getElementById("loadingDiv") instanceof HTMLElement ? document.getElementById("loadingDiv") : null;
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
FoodApp.Client.clientUtils.prototype.Contains = function (ngCategories, p){
    var res = false;
    for (var $i2 = 0,$l2 = ngCategories.length,str = ngCategories[$i2]; $i2 < $l2; $i2++, str = ngCategories[$i2]){
        if (str == p){
            res = true;
            break;
        }
    }
    return res;
};
FoodApp.Client.clientUtils.prototype.fixNumber = function (p){
    var jsNumber = parseFloat(p);
    var tmp = jsNumber.toPrecision(5);
    return parseFloat(tmp);
};
FoodApp.Client.ngFoodFilter = function (){
};
FoodApp.Client.ngFoodFilter.prototype.get_className = function (){
    return "ngFoodFilter";
};
FoodApp.Client.ngFoodFilter.prototype.get_namespace = function (){
    return "FoodApp.Client";
};
FoodApp.Client.ngFoodFilter.prototype.get_filterName = function (){
    return "foodFilter";
};
angularUtils.inst.registerFilterType(new FoodApp.Client.ngFoodFilter());
FoodApp.Client.ngFoodFilter.prototype.filter = function (obj, arg){
    var res =  [];
    var category = arg["category"];
    var day = arg["day"];
    var allFoods = obj;
    var items = allFoods[day];
    if (null != items){
        for (var $i3 = 0,$l3 = items.length,item = items[$i3]; $i3 < $l3; $i3++, item = items[$i3]){
            if ((item.Category == category) && !item.isContainer){
                res.push(item);
            }
        }
    }
    return res;
};
FoodApp.Client.ngControllerBase = function (){
    angularjs.angularController.call(this);
};
FoodApp.Client.ngControllerBase.prototype.get_className = function (){
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
    FoodApp.Client.clientUtils.Inst.hideLoading();
};
$Inherit(FoodApp.Client.ngControllerBase, angularjs.angularController);
FoodApp.Client.ngPropousalContoller = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngPropousalContoller.inst = new FoodApp.Client.ngPropousalContoller();
FoodApp.Client.ngPropousalContoller.prototype.get_className = function (){
    return "ngPropousalContoller";
};
FoodApp.Client.ngPropousalContoller.prototype.get_ngItems = function (){
    return this._scope["ngItems"];
};
FoodApp.Client.ngPropousalContoller.prototype.set_ngItems = function (value){
    this._scope["ngItems"] = value;
};
FoodApp.Client.ngPropousalContoller.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngItems( []);
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refreshPropousals();
    }));
};
FoodApp.Client.ngPropousalContoller.prototype.getFoodItem = function (id){
    var item = FoodApp.Client.ngFoodController.inst.findFoodById(id);
    return item;
};
FoodApp.Client.ngPropousalContoller.prototype.refreshPropousals = function (){
    FoodApp.Client.serviceHlp.inst.SendGet("json", "api/propousal/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/", $CreateAnonymousDelegate(this, function (o, s, arg3){
        this.set_ngItems(o);
        this._scope.$apply();
    }), $CreateDelegate(this, this.onRequestFailed));
};
$Inherit(FoodApp.Client.ngPropousalContoller, FoodApp.Client.ngControllerBase);
FoodApp.Client.ngPropousalFilter = function (){
};
FoodApp.Client.ngPropousalFilter.prototype.get_className = function (){
    return "ngPropousalFilter";
};
FoodApp.Client.ngPropousalFilter.prototype.get_namespace = function (){
    return "FoodApp.Client";
};
FoodApp.Client.ngPropousalFilter.prototype.get_filterName = function (){
    return "propousalFilter";
};
angularUtils.inst.registerFilterType(new FoodApp.Client.ngPropousalFilter());
FoodApp.Client.ngPropousalFilter.prototype.filter = function (obj, arg){
    debugger;
    var day = arg["day"];
    var allPropousals = obj;
    var res = allPropousals[day];
    return res;
};
FoodApp.Client.ngHistoryController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngHistoryController.inst = new FoodApp.Client.ngHistoryController();
FoodApp.Client.ngHistoryController.prototype.get_className = function (){
    return "ngHistoryController";
};
FoodApp.Client.ngHistoryController.prototype.get_ngHistoryItems = function (){
    return this._scope["ngHistoryItems"];
};
FoodApp.Client.ngHistoryController.prototype.set_ngHistoryItems = function (value){
    this._scope["ngHistoryItems"] = value;
};
FoodApp.Client.ngHistoryController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngHistoryItems( []);
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refreshHistory();
    }));
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.orderCompleted, $CreateAnonymousDelegate(this, function (n){
        this.refreshHistory();
    }));
};
FoodApp.Client.ngHistoryController.prototype.getFoodItem = function (id){
    var item = FoodApp.Client.ngFoodController.inst.findFoodById(id);
    return item;
};
FoodApp.Client.ngHistoryController.prototype.refreshHistory = function (){
    FoodApp.Client.serviceHlp.inst.SendGet("json", "api/history/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/", $CreateAnonymousDelegate(this, function (o, s, arg3){
        this.set_ngHistoryItems(o);
        this._scope.$apply();
    }), $CreateDelegate(this, this.onRequestFailed));
};
$Inherit(FoodApp.Client.ngHistoryController, FoodApp.Client.ngControllerBase);
FoodApp.Client.ngFavoriteController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngFavoriteController.inst = new FoodApp.Client.ngFavoriteController();
FoodApp.Client.ngFavoriteController.prototype.get_className = function (){
    return "ngFavoriteController";
};
FoodApp.Client.ngFavoriteController.prototype.get_ngFavoriteItems = function (){
    return this._scope["ngFavoriteItems"];
};
FoodApp.Client.ngFavoriteController.prototype.set_ngFavoriteItems = function (value){
    this._scope["ngFavoriteItems"] = value;
};
FoodApp.Client.ngFavoriteController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngFavoriteItems( []);
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refreshFavorite();
    }));
};
FoodApp.Client.ngFavoriteController.prototype.getFoodItem = function (id){
    var item = FoodApp.Client.ngFoodController.inst.findFoodById(id);
    return item;
};
FoodApp.Client.ngFavoriteController.prototype.refreshFavorite = function (){
    FoodApp.Client.serviceHlp.inst.SendGet("json", "api/favorite/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/", $CreateAnonymousDelegate(this, function (o, s, arg3){
        this.set_ngFavoriteItems(o);
        this._scope.$apply();
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngFavoriteController.prototype.rateChanged = function (rate, newRate){
    rate.Rate = newRate;
    FoodApp.Client.serviceHlp.inst.SendPost("json", "api/favorite/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + rate.FoodId + "/" + rate.Rate + "/", new Object(), $CreateAnonymousDelegate(this, function (){
    }), $CreateDelegate(this, this.onRequestFailed));
};
$Inherit(FoodApp.Client.ngFavoriteController, FoodApp.Client.ngControllerBase);
FoodApp.Client.eventManager = function (){
    this._dict = new Object();
};
FoodApp.Client.eventManager.settingsLoaded = "settingsLoaded";
FoodApp.Client.eventManager.orderCompleted = "orderCompleted";
FoodApp.Client.eventManager.orderListChanged = "orderListChanged";
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
    for (var $i4 = 0,$l4 = array.length,obj = array[$i4]; $i4 < $l4; $i4++, obj = array[$i4]){
        var action = obj;
        action(arg);
    }
};
FoodApp.Client.ngAppController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngAppController.inst = new FoodApp.Client.ngAppController();
FoodApp.Client.ngAppController.prototype.get_className = function (){
    return "ngAppController";
};
FoodApp.Client.ngAppController.prototype.get_namespace = function (){
    return "FoodApp.Client";
};
FoodApp.Client.ngAppController.prototype.get_ngUserId = function (){
    return this._scope["ngUserId"];
};
FoodApp.Client.ngAppController.prototype.set_ngUserId = function (value){
    this._scope["ngUserId"] = value;
};
FoodApp.Client.ngAppController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngUserId(document.getElementById("userId").value);
    console.log("email=" + this.get_ngUserId());
    window.setTimeout($CreateAnonymousDelegate(this, function (){
        FoodApp.Client.eventManager.inst.fire(FoodApp.Client.eventManager.settingsLoaded, "");
    }), 1);
};
$Inherit(FoodApp.Client.ngAppController, FoodApp.Client.ngControllerBase);
FoodApp.Client.ngOrderFilter = function (){
};
FoodApp.Client.ngOrderFilter.prototype.get_className = function (){
    return "ngOrderFilter";
};
FoodApp.Client.ngOrderFilter.prototype.get_namespace = function (){
    return "FoodApp.Client";
};
FoodApp.Client.ngOrderFilter.prototype.get_filterName = function (){
    return "orderFilter";
};
angularUtils.inst.registerFilterType(new FoodApp.Client.ngOrderFilter());
FoodApp.Client.ngOrderFilter.prototype.filter = function (obj, arg){
    var res =  [];
    var day = arg["day"];
    var allOrders = obj;
    var tmp = allOrders[day];
    if (tmp != null && tmp.length > 0){
        for (var $i5 = 0,$l5 = tmp.length,order = tmp[$i5]; $i5 < $l5; $i5++, order = tmp[$i5]){
            var foodItem = FoodApp.Client.ngFoodController.inst.findFoodById(order.FoodId);
            if (null != foodItem && !foodItem.isContainer){
                res.push(order);
            }
        }
    }
    return res;
};
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
FoodApp.Common.ngHistoryEntryByDate = function (){
    this.Date = System.DateTime.MinValue;
    this.Entries = null;
    this.Entries = new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngHistoryEntry");
};
$Inherit(FoodApp.Common.ngHistoryEntryByDate, FoodApp.Client.ngModelBase);
FoodApp.Common.ngMoneyModel = function (){
    this.UserId = null;
    this.Total = 0;
    this.MoneyOrders = null;
    this.MoneyOrders = new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngMoneyOrderModel");
};
$Inherit(FoodApp.Common.ngMoneyModel, FoodApp.Client.ngModelBase);
FoodApp.Common.ngMoneyOrderModel = function (){
    this.Operation = 0;
    this.Id = null;
    this.DateTime = System.DateTime.MinValue;
    this.Value = 0;
};
$Inherit(FoodApp.Common.ngMoneyOrderModel, FoodApp.Client.ngModelBase);
FoodApp.Common.ngFoodRate = function (){
    this.FoodId = null;
    this.Rate = 0;
};
$Inherit(FoodApp.Common.ngFoodRate, FoodApp.Client.ngModelBase);
FoodApp.Common.ngHistoryEntry = function (){
    this.FoodId = null;
    this.Date = System.DateTime.MinValue;
    this.Count = 0;
    this.FoodPrice = 0;
};
$Inherit(FoodApp.Common.ngHistoryEntry, FoodApp.Client.ngModelBase);
FoodApp.Common.ngHistoryModel = function (){
    this.Email = null;
    this.UserId = null;
    this.Entries = null;
    this.Entries = new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngHistoryEntry");
};
FoodApp.Common.ngHistoryModel.prototype.GetEntriesByDate = function (dt){
    var res = new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngHistoryEntry");
    var $it5 = this.Entries.GetEnumerator();
    while ($it5.MoveNext()){
        var entry = $it5.get_Current();
        if (FoodApp.Controllers.ApiUtils.EqualDate(entry.Date, dt)){
            res.Add(entry);
        }
    }
    return res;
};
FoodApp.Common.ngHistoryModel.prototype.GroupByDate = function (){
    var res = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor, System.Collections.Generic.List$1.ctor);
    var $it6 = this.Entries.GetEnumerator();
    while ($it6.MoveNext()){
        var entry = $it6.get_Current();
        var food = FoodApp.Common.FoodManager.Inst.GetFoodById$$String(entry.FoodId);
        if (null != food && !food.isContainer){
            var dateTime = entry.Date.ToShortDateString();
            if (!res.ContainsKey(dateTime)){
                res.set_Item$$TKey(dateTime, new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngHistoryEntry"));
            }
            res.get_Item$$TKey(dateTime).Add(entry);
        }
    }
    return res;
};
FoodApp.Common.ngHistoryModel.prototype.GroupByFoodId = function (){
    var res = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor, System.Collections.Generic.List$1.ctor);
    var $it7 = this.Entries.GetEnumerator();
    while ($it7.MoveNext()){
        var entry = $it7.get_Current();
        var food = FoodApp.Common.FoodManager.Inst.GetFoodById$$String(entry.FoodId);
        if (null != food && !food.isContainer){
            if (!res.ContainsKey(entry.FoodId)){
                res.set_Item$$TKey(entry.FoodId, new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngHistoryEntry"));
            }
            res.get_Item$$TKey(entry.FoodId).Add(entry);
        }
    }
    return res;
};
$Inherit(FoodApp.Common.ngHistoryModel, FoodApp.Client.ngModelBase);
FoodApp.Common.ngMoneyLoggerModel = function (){
    this.UserId = null;
    this.DateTime = System.DateTime.MinValue;
    this.OrderId = null;
    this.Operation = 0;
    this.Value = 0;
};
$Inherit(FoodApp.Common.ngMoneyLoggerModel, FoodApp.Client.ngModelBase);
FoodApp.Client.ngOrderEntry = function (){
    this.FoodId = null;
    this.Count = 0;
    this.FoodPrice = 0;
};
$Inherit(FoodApp.Client.ngOrderEntry, FoodApp.Client.ngModelBase);
FoodApp.Common.ngUsersSettingsModel = function (){
    this.FoodRates = null;
    this.Email = null;
    this.UserId = null;
    this.FoodRates = new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngFoodRate");
};
FoodApp.Common.ngUsersSettingsModel.prototype.GetFoodRateById = function (foodId){
    var res = null;
    var $it8 = this.FoodRates.GetEnumerator();
    while ($it8.MoveNext()){
        var r = $it8.get_Current();
        if (r.FoodId.Equals$$String(foodId)){
            res = r;
            break;
        }
    }
    return res;
};
FoodApp.Common.ngUsersSettingsModel.prototype.EnsureFoodRate = function (foodId){
    var res = this.GetFoodRateById(foodId);
    if (null == res){
        res = {};
        res.FoodId = foodId;
        this.FoodRates.Add(res);
    }
    return res;
};
FoodApp.Common.ngUsersSettingsModel.prototype.Fix = function (){
    var ratesThatNeedRemove = new System.Collections.Generic.List$1.ctor("FoodApp.Common.ngFoodRate");
    var $it9 = this.FoodRates.GetEnumerator();
    while ($it9.MoveNext()){
        var foodRate = $it9.get_Current();
        var food = FoodApp.Common.FoodManager.Inst.GetFoodById$$String(foodRate.FoodId);
        if (null == food){
            ratesThatNeedRemove.Add(foodRate);
        }
        else if (food.isContainer){
            ratesThatNeedRemove.Add(foodRate);
        }
    }
    var $it10 = ratesThatNeedRemove.GetEnumerator();
    while ($it10.MoveNext()){
        var rateToRemove = $it10.get_Current();
        this.FoodRates.Remove(rateToRemove);
    }
};
$Inherit(FoodApp.Common.ngUsersSettingsModel, FoodApp.Client.ngModelBase);
FoodApp.Common.ngUserModel = function (){
    this.Column = 0;
    this.Email = null;
    this.Name = null;
    this.Id = null;
    this.Picture = null;
    this.GoogleName = null;
    this.IsAdmin = false;
    this.GoogleFirstName = null;
};
$Inherit(FoodApp.Common.ngUserModel, FoodApp.Client.ngModelBase);
FoodApp.Client.ngFoodItem = function (){
    this.Name = null;
    this.Description = null;
    this.Category = null;
    this.Price = 0;
    this.FoodId = null;
    this.IsByWeightItem = false;
    this.isContainer = false;
    this.isSmallContainer = false;
    this.isBigContainer = false;
    this.isSalat = false;
    this.isGarnir = false;
    this.isMeatOrFish = false;
    this.isFirst = false;
    this.isKvasoleva = false;
};
$Inherit(FoodApp.Client.ngFoodItem, FoodApp.Client.ngModelBase);
FoodApp.Client.ngFoodController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngFoodController.inst = new FoodApp.Client.ngFoodController();
FoodApp.Client.ngFoodController.prototype.get_className = function (){
    return "ngFoodController";
};
FoodApp.Client.ngFoodController.prototype.get_namespace = function (){
    return "FoodApp.Client";
};
FoodApp.Client.ngFoodController.prototype.get_ngFoods = function (){
    return this._scope["ngFoods"];
};
FoodApp.Client.ngFoodController.prototype.set_ngFoods = function (value){
    this._scope["ngFoods"] = value;
};
FoodApp.Client.ngFoodController.prototype.get_ngCategories = function (){
    return this._scope["ngCategories"];
};
FoodApp.Client.ngFoodController.prototype.set_ngCategories = function (value){
    this._scope["ngCategories"] = value;
};
FoodApp.Client.ngFoodController.prototype.buyClick = function (day, foodId, value){
    FoodApp.Client.clientUtils.Inst.showLoading();
    FoodApp.Client.serviceHlp.inst.SendPost("json", "api/foods/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + day + "/" + foodId + "/" + value + "/", new Object(), $CreateAnonymousDelegate(this, function (){
        FoodApp.Client.clientUtils.Inst.hideLoading();
        FoodApp.Client.ngOrderController.inst.refreshOrders();
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngFoodController.prototype.hasOrder = function (day, foodId){
    var res = false;
    var order = FoodApp.Client.ngOrderController.inst.getOrderByFoodId(day, foodId);
    if (null != order){
        res = true;
    }
    return res;
};
FoodApp.Client.ngFoodController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngFoods( []);
    this.set_ngCategories( []);
    this.get_ngCategories().push("Салати");
    this.get_ngCategories().push("Перші страви");
    this.get_ngCategories().push("Гарніри");
    this.get_ngCategories().push("Мясо/Риба");
    this.get_ngCategories().push("Комплексний");
    this.get_ngCategories().push("Хліб");
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refreshFoods($CreateAnonymousDelegate(this, function (){
            var fn = window["initMenu"];
            fn.call();
        }));
    }));
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.orderListChanged, $CreateAnonymousDelegate(this, function (n){
        this._scope.$apply();
    }));
};
FoodApp.Client.ngFoodController.prototype.getPrefix = function (price){
    var res = price;
    if (res.indexOf(".") > 0){
        res = res.substr(0, res.indexOf("."));
    }
    return res;
};
FoodApp.Client.ngFoodController.prototype.getSuffix = function (price){
    var res = price;
    if (res.indexOf(".") > 0){
        res = res.substr(res.indexOf(".") + 1);
    }
    else {
        res = "";
    }
    return res;
};
FoodApp.Client.ngFoodController.prototype.refreshFoods = function (complete){
    FoodApp.Client.serviceHlp.inst.SendGet("json", "api/foods/", $CreateAnonymousDelegate(this, function (o, s, arg3){
        this.set_ngFoods(o);
        this._scope.$apply();
        if (null != complete){
            complete();
        }
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngFoodController.prototype.change = function (){
    this.refreshFoods(null);
};
FoodApp.Client.ngFoodController.prototype.findFoodById = function (id){
    var res = null;
    for (var $i12 = 0,$t12 = this.get_ngFoods(),$l12 = $t12.length,dayItems = $t12[$i12]; $i12 < $l12; $i12++, dayItems = $t12[$i12]){
        for (var $i13 = 0,$l13 = dayItems.length,item = dayItems[$i13]; $i13 < $l13; $i13++, item = dayItems[$i13]){
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
FoodApp.Client.ngFoodController.prototype.getOrderCount = function (day, foodId){
    var res = 0;
    var order = FoodApp.Client.ngOrderController.inst.getOrderByFoodId(day, foodId);
    if (null != order){
        res = order.Count;
    }
    return res;
};
$Inherit(FoodApp.Client.ngFoodController, FoodApp.Client.ngControllerBase);
FoodApp.Client.ngOrderController = function (){
    FoodApp.Client.ngControllerBase.call(this);
};
FoodApp.Client.ngOrderController.inst = new FoodApp.Client.ngOrderController();
FoodApp.Client.ngOrderController.prototype.get_className = function (){
    return "ngOrderController";
};
FoodApp.Client.ngOrderController.prototype.get_ngOrderEntries = function (){
    return this._scope["ngOrderEntries"];
};
FoodApp.Client.ngOrderController.prototype.set_ngOrderEntries = function (value){
    this._scope["ngOrderEntries"] = value;
};
FoodApp.Client.ngOrderController.prototype.getFoodItem = function (id){
    var item = FoodApp.Client.ngFoodController.inst.findFoodById(id);
    return item;
};
FoodApp.Client.ngOrderController.prototype.formatCount = function (order){
    var res = order.Count + "";
    var food = this.getFoodItem(order.FoodId);
    if (food.IsByWeightItem){
        res = parseInt((order.Count * 100), 10) + "";
    }
    return res;
};
FoodApp.Client.ngOrderController.prototype.getTotalPrice = function (day){
    var res = 0;
    var ngOrderModels = this.get_ngOrderEntries()[day];
    if (ngOrderModels != null){
        for (var $i14 = 0,$l14 = ngOrderModels.length,item = ngOrderModels[$i14]; $i14 < $l14; $i14++, item = ngOrderModels[$i14]){
            var food = this.getFoodItem(item.FoodId);
            if (null != food){
                res += FoodApp.Client.clientUtils.Inst.fixNumber(food.Price * item.Count);
                res = FoodApp.Client.clientUtils.Inst.fixNumber(res);
            }
        }
    }
    res = FoodApp.Client.clientUtils.Inst.fixNumber(res);
    return res;
};
FoodApp.Client.ngOrderController.prototype.init = function (scope, http, loc, filter){
    FoodApp.Client.ngControllerBase.prototype.init.call(this, scope, http, loc, filter);
    this.set_ngOrderEntries( []);
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.settingsLoaded, $CreateAnonymousDelegate(this, function (n){
        this.refreshOrders();
    }));
    FoodApp.Client.eventManager.inst.subscribe(FoodApp.Client.eventManager.orderCompleted, $CreateAnonymousDelegate(this, function (n){
        this.refreshOrders();
    }));
};
FoodApp.Client.ngOrderController.prototype.deleteOrder = function (day, row){
    FoodApp.Client.clientUtils.Inst.showLoading();
    FoodApp.Client.serviceHlp.inst.SendDelete("json", "api/orders/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/" + day + "/" + row + "/", new Object(), $CreateAnonymousDelegate(this, function (){
        FoodApp.Client.clientUtils.Inst.hideLoading();
        this.refreshOrders();
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngOrderController.prototype.refreshOrders = function (){
    FoodApp.Client.serviceHlp.inst.SendGet("json", "api/orders/" + FoodApp.Client.ngAppController.inst.get_ngUserId() + "/", $CreateAnonymousDelegate(this, function (o, s, arg3){
        var tmp = o;
        while (this.get_ngOrderEntries().length > 0){
            this.get_ngOrderEntries().pop();
        }
        for (var $i15 = 0,$l15 = tmp.length,obj = tmp[$i15]; $i15 < $l15; $i15++, obj = tmp[$i15]){
            this.get_ngOrderEntries().push(obj);
        }
        FoodApp.Client.eventManager.inst.fire(FoodApp.Client.eventManager.orderListChanged, this.get_ngOrderEntries());
        this._scope.$apply();
    }), $CreateDelegate(this, this.onRequestFailed));
};
FoodApp.Client.ngOrderController.prototype.getOrders = function (day){
    return this.get_ngOrderEntries()[day];
};
FoodApp.Client.ngOrderController.prototype.getOrderByFoodId = function (day, foodId){
    var res = null;
    var orders = FoodApp.Client.ngOrderController.inst.getOrders(day);
    for (var $i16 = 0,$l16 = orders.length,order = orders[$i16]; $i16 < $l16; $i16++, order = orders[$i16]){
        if (order.FoodId == foodId){
            res = order;
            break;
        }
    }
    return res;
};
$Inherit(FoodApp.Client.ngOrderController, FoodApp.Client.ngControllerBase);

