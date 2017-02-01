using System;
using System.Web.Mvc;

namespace MovieStore.Web.Controllers
{
    public class JasmineController : Controller
    {
        public ViewResult Run()
        {
            return View("SpecRunner");
        }
    }
}
