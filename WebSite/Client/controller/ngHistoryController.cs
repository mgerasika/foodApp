using angularjs;
using FoodApp.Common;
using FoodApp.Properties;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpKit.JavaScript;

namespace FoodApp.Client {
    [JsType(JsMode.Prototype, Filename = WebApiResources._fileClientJs)]
    public class ngHistoryController : ngControllerBase {
        public static ngHistoryController inst = new ngHistoryController();

        public override string className {
            get { return "ngHistoryController"; }
        }

        public JsArray<ngHistoryGroupEntry> ngHistoryItems {
            get { return _scope["ngHistoryItems"].As<JsArray<ngHistoryGroupEntry>>(); }
            set { _scope["ngHistoryItems"] = value; }
        }

        public JsArray<ngUserModel> ngUsers {
            get { return _scope["ngUsers"].As<JsArray<ngUserModel>>(); }
            set { _scope["ngUsers"] = value; }
        }

        public string ngUserId {
            get { return _scope["ngUserId"].As<string>(); }
            set { _scope["ngUserId"] = value; }
        }

        public void checkMoneyClick(string userId) {
            ngUserId = userId;

            refreshHistory();
        }

        public override void init(angularScope scope, angularHttp http, angularLocation loc, angularFilter filter) {
            base.init(scope, http, loc, filter);
            ngHistoryItems = new JsArray<ngHistoryGroupEntry>();
            ngUserId = ngAppController.inst.ngUserId;

            ngUsers = new JsArray<ngUserModel>();
            eventManager.inst.subscribe(eventManager.settingsLoaded, delegate(int n) { refreshHistory(); });

            eventManager.inst.subscribe(eventManager.orderCompleted, delegate(int n) { refreshHistory(); });

            requestGetUsers(delegate { _scope.apply(); });
        }

        public ngFoodItem getFoodItem(string id) {
            ngFoodItem item = ngFoodController.inst.findFoodById(id);
            return item;
        }

        public void refreshHistory() {
            jsUtils.inst.showLoading();
            ajaxHlp.inst.SendGet("json",
                HistoryUrl.c_sHistoryPrefix + "/" + ngUserId + "/",
                delegate(object o, JsString s, jqXHR arg3) {
                    ngHistoryItems = o.As<JsArray<ngHistoryGroupEntry>>();
                    createLineChart();
                    createBarChart();
                    createTerribleLineChart();

                    _scope.apply();

                    jsUtils.inst.hideLoading();
                }, onRequestFailed);
        }

        private ngHistoryController() {
        }

        private void createLineChart() {
            Element ctx = HtmlContext.document.getElementById("lineChart");

            JsArray allDays = new JsArray();
            foreach (ngHistoryGroupEntry ngHistoryGroupEntry in ngHistoryItems) {
                JsDate date = new JsDate(ngHistoryGroupEntry.DateStr);
                decimal price = CalcTotalPrice(ngHistoryGroupEntry);
                var item = new {
                    x = date,
                    y = price
                };
                allDays.push(item);
            }
            var settings = new {
                type = "line",
                data = new {
                    datasets = new JsArray {
                        new {
                            label = "",
                            data = allDays
                        }
                    }
                },
                options = new {
                    scales = new {
                        xAxes = new JsArray {
                            new {
                                type = "time",
                                position = "bottom",
                                unit = "day",
                                unitStepSize = 1,
                                time = new {
                                    displayFormats = new {
                                        day = "YYYY MMM DD"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            JsContext.eval("createChart(ctx,settings)");
        }

        private void createTerribleLineChart() {
            Element ctx = HtmlContext.document.getElementById("line2Chart");

            JsArray simpleDays = new JsArray();

            JsArray fridayDays = new JsArray();
            foreach (ngHistoryGroupEntry ngHistoryGroupEntry in ngHistoryItems) {
                JsDate date = new JsDate(ngHistoryGroupEntry.DateStr);
                decimal price = CalcTotalPrice(ngHistoryGroupEntry);
                var item = new {
                    x = date,
                    y = price
                };
                if (date.getDay() == 5) {
                    fridayDays.push(item);
                }
                else {
                    simpleDays.push(item);
                }
            }
            var settings = new {
                type = "line",
                data = new {
                    datasets = new JsArray {
                        new {
                            label = "Other",
                            borderColor = "blue",
                            data = simpleDays
                        },
                        new {
                            borderColor = "red",
                            label = "Friday",
                            data = fridayDays
                        }
                    }
                },
                options = new {
                    scales = new {
                        xAxes = new JsArray {
                            new {
                                type = "time",
                                position = "bottom",
                                unit = "day",
                                unitStepSize = 1,
                                time = new {
                                    displayFormats = new {
                                        day = "YYYY MMM DD"
                                    }
                                }
                            }
                        }
                    }
                }
            };
            JsContext.eval("createChart(ctx,settings)");
        }

        private void createBarChart() {
            Element ctx = HtmlContext.document.getElementById("barChart");

            JsArray labels = new JsArray();
            JsArray colors = new JsArray();
            JsArray allDays = new JsArray();

            foreach (ngHistoryGroupEntry ngHistoryGroupEntry in ngHistoryItems) {
                JsDate date = new JsDate(ngHistoryGroupEntry.DateStr);
                decimal price = CalcTotalPrice(ngHistoryGroupEntry);

                labels.push(ngHistoryGroupEntry.DateStr);
                allDays.push(price);

                if (date.getDay() == 5) {
                    colors.push("blue");
                }
                else {
                    colors.push("blue");
                }
            }
            var settings = new {
                type = "bar",
                data = new {
                    labels,
                    datasets = new JsArray {
                        new {
                            label = "",
                            backgroundColor = colors,
                            data = allDays
                        }
                    }
                },
                options = new {
                    scales = new {
                        xAxes = new JsArray {
                            new {
                            }
                        },
                        yAxes = new JsArray {
                            new {
                            }
                        }
                    }
                }
            };
            JsContext.eval("createChart(ctx,settings)");
        }

        private decimal CalcTotalPrice(ngHistoryGroupEntry ngHistoryGroupEntry) {
            decimal price = 0;
            JsArray ngHistoryEntries = ngHistoryGroupEntry.Entries.As<JsArray>();
            foreach (ngHistoryEntry entry in ngHistoryEntries) {
                JsString str = (entry.Count*entry.FoodPrice).As<JsString>();
                decimal tmp = JsContext.parseFloat(str).As<decimal>();
                price += tmp;
            }
            return price.As<decimal>();
        }

        private void requestGetUsers(JsAction complete) {
            JsService.Inst.UsersApi.GetUsers(delegate(JsArray<ngUserModel> res) {
                JsArray<ngUserModel> tmp = new JsArray<ngUserModel>();
                foreach (ngUserModel user in res) {
                    JsString email = user.Email.As<JsString>();
                    if (-1 != email.indexOf("darwins") || -1 != email.indexOf("stiystil")) {
                        tmp.Add(user);
                    }
                }
                ngUsers = tmp;

                if (null != complete) {
                    complete();
                }
            });
        }
    }
}