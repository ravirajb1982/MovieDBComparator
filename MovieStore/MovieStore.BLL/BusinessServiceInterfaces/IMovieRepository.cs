using MovieStore.Service.Interfaces;
using MovieStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.BLL.BusinessServiceInterfaces
{
    public interface IMovieRepository
    {
        Task<MovieResult<MovieGetResponse>> GetMovies();
        Task<MovieResult<MovieBooking>> GetMovieById(string id);
        void SetMovieDb(string moviedb);
    }
}
