﻿using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FoodApp.Common;

namespace FoodApp {
    public class WebApiApplication : HttpApplication {
        protected void Application_Start() {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.NumberDecimalSeparator = ",";
            BackupHistoryManager backupHistoryManager = BackupHistoryManager.Inst;
            ClearOrdersManager clearOrdersManage = ClearOrdersManager.Inst;

            UsersManager.Inst.Fix();
            UserSettingsManager.Inst.Fix();
            HistoryManager.Inst.Fix();

            /*
            ngUserModel user = UsersManager.Inst.GetUserByEmail("mgerasika@gmail.com");
            string orderId = OrderManager.Inst.GetOrderId(user,3);
            if (!MoneyManager.Inst.CanBuy(user, 100,orderId)) {
                MoneyManager.Inst.Add(user,200);
            }
            if (MoneyManager.Inst.CanBuy(user, 10,orderId)) {
                MoneyManager.Inst.Buy(user, 10, orderId);
            }

            if (MoneyManager.Inst.CanBuy(user, 10,orderId))
            {
                MoneyManager.Inst.Buy(user, 10, orderId);
            }

            if (MoneyManager.Inst.CanRefund(user, orderId))
            {
                MoneyManager.Inst.Refund(user,orderId);
            }
            if (MoneyManager.Inst.CanRefund(user, orderId))
            {
                MoneyManager.Inst.Refund(user, orderId);
            }
             * */

            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}