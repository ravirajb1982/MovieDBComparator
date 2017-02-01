using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Models.Models
{

    public class MovieResult<T>
    {
        public MovieResultErrorCode? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public enum MovieResultErrorCode
    {
        //general
        VALIDATION_FAILURE,
        LOGICAL_ERROR,


        UNKNOWN_ERROR,
        SERVER_ERROR
    }


    public class MovieDBInfo
    {
        public string ID { get; set; }
        public string MovieDB { get; set; }

    }

        public class MovieDetail
    {
        public MovieDBInfo[] movieDBInfo { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public string Rated { get; set; }

        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Metascore { get; set; }
        public string Rating { get; set; }
        public string Votes { get; set; }
        public string Price { get; set; }
    }


    public class MovieBooking
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public string Rated { get; set; }

        public string Released { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
        public string Metascore { get; set; }
        public string Rating { get; set; }
        public string Votes { get; set; }
        public double Price { get; set; }
        public string MovieDB { get; set; }
        public string RivalID { get; set; }
        public string RivalMovieDB { get; set; }
        public double RivalPrice { get; set; }
        public bool RivalPriceCheaper { get; set; }
    }



    public class Movie4xxResponse
    {
        public bool success { get; set; }
        public Movie4xxResponseError error { get; set; }
    }
    public class Movie4xxResponseError
    {
        public string message { get; set; }
        public int code { get; set; }
    }

    public class MovieUpdateResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public string booking_id { get; set; }
    }
    public class MovieGetResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<MovieBooking> movies { get; set; }
    }

    public class MovieGetByIdResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public MovieBooking movie { get; set; }
    }


    public class MovieUnAuthorisedException : Exception
    {

    }
}
