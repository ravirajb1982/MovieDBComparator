using log4net;
using MovieStore.BLL.BusinessService;
using MovieStore.BLL.BusinessServiceInterfaces;
using MovieStore.BLL.Repositories;
using MovieStore.Models.Models;
using MovieStore.Service.Interfaces;
using MovieStore.Service.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;
using MovieStore.Web.AutofacDI;

namespace MovieStore.Web.Controllers
{
    public class MovieController : Controller
    {
        private static string SourceMovieDb = ConfigurationManager.AppSettings["SourceMovieDb"];
        private static string RivalMovieDb = ConfigurationManager.AppSettings["RivalMovieDb"];
        IMovieBS _service;

        public MovieController()
        {
            _service = ContainerConfig.BuildContainer().Resolve<IMovieBS>();
        }

        public async Task<ActionResult> GetMovies()
        {
            var dataMovieList = await _service.GetMovieList(SourceMovieDb, RivalMovieDb).ConfigureAwait(false);

                if (dataMovieList.ErrorCode != null){
                    return Json(new { success = false, errorMessage = dataMovieList.ErrorMessage }, JsonRequestBehavior.AllowGet);
                }

            return Json(new { success = true, data = dataMovieList.Data }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> GetMovieDetail(MovieBooking movie)
        {
           var dataMovieDetail = await _service.GetMovieDetail(movie).ConfigureAwait(false);

                if (dataMovieDetail.ErrorCode != null)
                {
                    return Json(new { success = false, errorMessage = dataMovieDetail.ErrorMessage }, JsonRequestBehavior.AllowGet);
                }
           return Json(new { success = true, data = dataMovieDetail.Data }, JsonRequestBehavior.AllowGet);

        }
    }
}