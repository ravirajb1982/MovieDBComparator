using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace MovieStore.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        Exception exception = Server.GetLastError();
            if (exception is HttpUnhandledException)
            {
                exception = exception.InnerException;
            }
            if (exception != null)
            {
                Log.Error(exception.Message);
            }
        }
    }
}
