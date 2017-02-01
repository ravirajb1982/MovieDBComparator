using MovieStore.BLL.BusinessService;
using MovieStore.BLL.BusinessServiceInterfaces;
using MovieStore.BLL.Repositories;
using MovieStore.Models.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace MovieStore.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();

        }     

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}