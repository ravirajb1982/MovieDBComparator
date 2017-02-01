using MovieStore.Models.Models;
using MovieStore.Service.Helpers;
using MovieStore.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Service.Services
{
   public class MovieService:IMovieService
    {
        private static string ApiToken = ConfigurationManager.AppSettings["ApiToken"];
        private static string ApiEndpoint = ConfigurationManager.AppSettings["ApiEndpoint"];
        MovieClient client;

        public MovieService()
        {
        }

        public void SetMovieDb(string _moviedb)
        {
            client = new MovieClient(ApiToken, ApiEndpoint, _moviedb);
        }

        public async Task<MovieResult<MovieGetResponse>> GetMovies()
        {
            var result = await client.connector.GetAll().ConfigureAwait(false);
            return result;
        }


        public async Task<MovieResult<MovieBooking>> GetMovieById(string id)
        {
             var result = await client.connector.GetMovieById(id).ConfigureAwait(false);
            return result;
        }

    }
}
