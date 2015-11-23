using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FoodApp.Common;

namespace FoodApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.NumberDecimalSeparator = ",";
            BackupHistoryManager backupHistoryManager = BackupHistoryManager.Inst;
            ClearOrdersManager clearOrdersManage2R = ClearOrdersManager.Inst;
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
