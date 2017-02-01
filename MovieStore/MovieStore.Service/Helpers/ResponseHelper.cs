using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using MovieStore.Models.Models;
using System.Net.Http.Headers;

namespace MovieStore.Service.Helpers
{
    public static class ResponseHelper
    {
        public static async Task<MovieResult<T>> ConvertToResult<T>(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new MovieUnAuthorisedException();
            else if (response.StatusCode == System.Net.HttpStatusCode.OK
                || response.StatusCode == System.Net.HttpStatusCode.Created ||
                response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                if (typeof(T) == typeof(bool))
                    return new MovieResult<bool> { Data = true } as MovieResult<T>;

                var responseObj = await response.Content.ReadAsAsync<T>();
                return new MovieResult<T> { Data = responseObj };
            }
            else if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 500)
            {
                var responseObj = await response.Content.ReadAsAsync<Movie4xxResponse>();
                if (responseObj != null && responseObj.error != null)
                {
                    return new MovieResult<T>
                    {
                        ErrorCode = MovieResultErrorCode.LOGICAL_ERROR,
                        ErrorMessage = responseObj.error.message + " (Code=" + responseObj.error.code + ") . Please try again"
                    };
                }
                else
                    return new MovieResult<T>
                    {
                        ErrorCode = MovieResultErrorCode.UNKNOWN_ERROR,
                        ErrorMessage = response.ReasonPhrase + " (Code=" + (int)response.StatusCode+ ") . Please try again"
                    };
            }
            else
            {

                return new MovieResult<T>
                {
                    ErrorCode = MovieResultErrorCode.UNKNOWN_ERROR,
                    ErrorMessage = "Unknow error. server returned status code " + response.StatusCode + ".Please try again "
                };
            }
        }

    }

    public static class RequestHelper
    {
        public static void SetHeaders(HttpClient client, string apiToken, Uri apiEndpoint)
        {
            client.DefaultRequestHeaders.Add("x-access-token", apiToken);
            client.BaseAddress = apiEndpoint;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
