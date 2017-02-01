using MovieStore.BLL.BusinessServiceInterfaces;
using MovieStore.Service.Helpers;
using MovieStore.Service.Interfaces;
using MovieStore.Models.Models;
using MovieStore.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MovieStore.BLL.AutofacDI;
using Autofac;

namespace MovieStore.BLL.Repositories
{
   public class MovieRepository:IMovieRepository
    {
        IMovieService _service;

        public MovieRepository()
        {
            _service = ContainerConfig.BuildContainer().Resolve<IMovieService>();
        }

        public void SetMovieDb(string _moviedb)
        {
            _service.SetMovieDb(_moviedb);
        }

        public async Task<MovieResult<MovieGetResponse>> GetMovies()
        {
            var result = await _service.GetMovies().ConfigureAwait(false);
            return result;
        }

        public async Task<MovieResult<MovieBooking>> GetMovieById(string id)
        {
             var result = await _service.GetMovieById(id).ConfigureAwait(false);
            return result;
        }

    }
}
