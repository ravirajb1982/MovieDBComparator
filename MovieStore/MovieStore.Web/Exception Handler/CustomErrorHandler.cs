using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieStore.Web.Exception_Handler
{
        public class CustomErrorHandler : HandleErrorAttribute
        {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnException(ExceptionContext filterContext)
            {
                if (!filterContext.ExceptionHandled)
                {
                    string controller = filterContext.RouteData.Values["controller"].ToString();
                    string action = filterContext.RouteData.Values["action"].ToString();
                    Exception ex = filterContext.Exception;
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "Error"
                    };
                    //Log error  
                    Log.Error("Error Message" +ex.Message);
                    Log.Error("source" + ex.Source);
                    Log.Error("stack trace" + ex.StackTrace);
                    Log.Error("TargetSite" + ex.TargetSite);
                    Log.Error("InnerException" + ex.InnerException);
                    Log.Error("HResult" + ex.HResult);
                    Log.Error("HelpLink" + ex.HelpLink);
                //RedirectToAction("Error", "Home");  
            }
            }
        }  
}