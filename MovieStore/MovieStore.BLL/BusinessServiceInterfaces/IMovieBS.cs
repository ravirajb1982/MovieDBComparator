using MovieStore.Service.Interfaces;
using MovieStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.BLL.BusinessServiceInterfaces
{
    public interface IMovieBS
    {
        Task<MovieResult<MovieBooking>> GetMovieDetail(MovieBooking movie);
        Task<MovieResult<List<MovieBooking>>> GetMovieList(string SourceMovieDb, string RivalMovieDb);
        Task<MovieResult<MovieGetResponse>> GetMovies();
        Task<MovieResult<MovieBooking>> GetMovieById(string id);
         void SetMovieDb(string moviedb);
        List<MovieBooking> UpdateMovieDBField(MovieResult<MovieGetResponse> ResponseMovieData, string moviedb);
        List<MovieBooking> GetCommonMovies(List<MovieBooking> SourceDb, List<MovieBooking> RivalDb);
        List<MovieBooking> GetMoviesUniqueInRivalDb(List<MovieBooking> SourceDb, List<MovieBooking> RivalDb);
        List<MovieBooking> GetMoviesUniqueInSourceDb(List<MovieBooking> SourceDb, List<MovieBooking> RivalDb);
    }
}
