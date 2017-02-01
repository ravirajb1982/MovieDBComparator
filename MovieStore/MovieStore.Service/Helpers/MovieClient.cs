using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Service.Helpers
{
    public class MovieClient
    {
        public string ApiMovieDB { get; set; }
        public string ApiToken { get; private set; }
        public Uri ApiEndpoint { get; private set; }
        public MovieConnector connector { private set; get; }
        public object DefaultRequestHeaders { get; internal set; }

        public MovieClient(string apiToken, string apiEndpoint,string apiMovieDB)
        {
            ApiEndpoint = new Uri(apiEndpoint);
            ApiToken = apiToken;
            ApiMovieDB = apiMovieDB;
            connector = new MovieConnector(this);
        }
    }
}
