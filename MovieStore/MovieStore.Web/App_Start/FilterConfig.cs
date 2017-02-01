using MovieStore.Web.Exception_Handler;
using System.Web;
using System.Web.Mvc;

namespace MovieStore.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomErrorHandler());
        }
    }
}
