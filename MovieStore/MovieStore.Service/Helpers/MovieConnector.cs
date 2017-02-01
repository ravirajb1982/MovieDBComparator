using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using MovieStore.Models.Models;
using MovieStore.Service.Interfaces;
using Newtonsoft.Json.Linq;
using MovieStore.Service.Handlers;

namespace MovieStore.Service.Helpers
{
    public class MovieConnector
    {
        private MovieClient MovieClient;

        public MovieConnector()
        {
         
        }

        public MovieConnector(MovieClient client)
        {
            MovieClient = client;
        }

        public async Task<MovieResult<MovieGetResponse>> GetAll()
        {
          var handler = new RetryDelegatingHandler();

            using (var client = new HttpClient(handler))
            {
                var result = new MovieResult<MovieGetResponse>();
                try
                {
                    RequestHelper.SetHeaders(client, MovieClient.ApiToken, MovieClient.ApiEndpoint);
                    var route = MovieClient.ApiMovieDB + "/movies/";
                    var response = await client.GetAsync(route).ConfigureAwait(false);
                    result = await ResponseHelper.ConvertToResult<MovieGetResponse>(response).ConfigureAwait(false);

                }catch(Exception ex)
                {
                    return new MovieResult<MovieGetResponse>
                    {
                        ErrorCode = MovieResultErrorCode.SERVER_ERROR,
                        ErrorMessage = ex.Message.ToString()
                    };
                }

                return result;
            }
        }

        public async Task<MovieResult<MovieBooking>> GetMovieById(string id)
        {
            var handler = new RetryDelegatingHandler();

            using (var client = new HttpClient(handler))
            {
                var result = new MovieResult<MovieBooking>();
                try
                {
                    RequestHelper.SetHeaders(client, MovieClient.ApiToken, MovieClient.ApiEndpoint);
                    var route = MovieClient.ApiMovieDB + "/movie/" + id;
                    var response = await client.GetAsync(route).ConfigureAwait(false);
                    result = await ResponseHelper.ConvertToResult<MovieBooking>(response).ConfigureAwait(false);

                }
                catch (Exception ex)
                {
                    return new MovieResult<MovieBooking>
                    {
                        ErrorCode = MovieResultErrorCode.SERVER_ERROR,
                        ErrorMessage = ex.Message.ToString()
                    };
                }
                return result;
            }
        }

    }
}
