using Autofac;
using MovieStore.BLL.AutofacDI;
using MovieStore.BLL.BusinessServiceInterfaces;
using MovieStore.BLL.Repositories;
using MovieStore.Models.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.BLL.BusinessService
{
    public class MovieBS:IMovieBS
    {
        public IMovieRepository _MovieRepository = null;
        private List<MovieBooking> listMovies = new List<MovieBooking>();

        public MovieBS()
        {
           _MovieRepository = ContainerConfig.BuildContainer().Resolve<IMovieRepository>();
        }

        public void SetMovieDb(string moviedb)
        {
            _MovieRepository.SetMovieDb(moviedb);
        }

        public Task<MovieResult<MovieGetResponse>> GetMovies()
        {
            return _MovieRepository.GetMovies();
        }

        public Task<MovieResult<MovieBooking>> GetMovieById(string id)
        {
            return _MovieRepository.GetMovieById(id);
        }

        public async Task<MovieResult<List<MovieBooking>>> GetMovieList(string SourceMovieDb, string RivalMovieDb)
        {
            var dataSrcMovieDb = new List<MovieBooking>();
            var dataRivalMovieDb = new List<MovieBooking>();

            this.SetMovieDb(SourceMovieDb);
            var ResponseSourceMovieDb = await this.GetMovies().ConfigureAwait(false);

            if (ResponseSourceMovieDb.ErrorCode.HasValue)
                return GetErrorResponseMovieList(ResponseSourceMovieDb);

            dataSrcMovieDb = UpdateMovieDBField(ResponseSourceMovieDb, SourceMovieDb);

            this.SetMovieDb(RivalMovieDb);
            var ResponseRivalMovieDb = await this.GetMovies().ConfigureAwait(false);

            if (ResponseRivalMovieDb.ErrorCode.HasValue)
                return GetErrorResponseMovieList(ResponseRivalMovieDb);       

            if (ResponseRivalMovieDb.Data.movies.Count>0){
                dataRivalMovieDb = UpdateMovieDBField(ResponseRivalMovieDb, RivalMovieDb);
                listMovies.AddRange(ProcessMovieList(dataSrcMovieDb, dataRivalMovieDb));
            }
            else{
                listMovies.AddRange(dataSrcMovieDb);
            }

            return new MovieResult<List<MovieBooking>> { Data = listMovies } as MovieResult<List<MovieBooking>>;

        }
        public async Task<MovieResult<MovieBooking>> GetMovieDetail(MovieBooking movie)
        {
            this.SetMovieDb(movie.MovieDB);
            var ResponseSourceMovieData = await this.GetMovieById(movie.ID).ConfigureAwait(false);

            if (ResponseSourceMovieData.ErrorCode.HasValue)
                return GetErrorResponseMovieDetail(ResponseSourceMovieData);

            var SourceMovieData = ResponseSourceMovieData.Data;
            SourceMovieData.MovieDB = movie.MovieDB;

            MovieBooking RivalMovieData = null;

            if (movie.RivalID != null)
            {
                this.SetMovieDb(movie.RivalMovieDB);
                var  ResponseRivalMovieData = await this.GetMovieById(movie.RivalID).ConfigureAwait(false);

                if (ResponseRivalMovieData.ErrorCode.HasValue)
                    return GetErrorResponseMovieDetail(ResponseRivalMovieData);

                RivalMovieData = ResponseRivalMovieData.Data;
            }

            if (RivalMovieData != null)
            {
                SourceMovieData.RivalID = RivalMovieData.ID;
                SourceMovieData.RivalMovieDB = movie.RivalMovieDB;
                SourceMovieData.RivalPrice = RivalMovieData.Price;

                if (SourceMovieData.Price >= RivalMovieData.Price)
                {
                    SourceMovieData.RivalPriceCheaper = true;
                }
            }
            return new MovieResult<MovieBooking> { Data = SourceMovieData } as MovieResult<MovieBooking>;

        }

        public List<MovieBooking> UpdateMovieDBField(MovieResult<MovieGetResponse> ResponseMovieData,string moviedb)
        {
            List<MovieBooking> dataMovieList= new List<MovieBooking>();

            if (ResponseMovieData.Data.movies.Count > 0)
            {
                dataMovieList = ResponseMovieData.Data.movies;
                dataMovieList.ForEach(c => c.MovieDB = moviedb);
            }
            return dataMovieList;
        }

        public List<MovieBooking> ProcessMovieList(List<MovieBooking> dataSrcMovieDb, List<MovieBooking> dataRivalMovieDb)
        {
            List<MovieBooking> MovieListProcessed = new List<MovieBooking>();

            var ListCommonMovies = this.GetCommonMovies(dataSrcMovieDb, dataRivalMovieDb);
            MovieListProcessed.AddRange(ListCommonMovies);

            var ListMoviesUniqueInRivalDb = this.GetMoviesUniqueInRivalDb(dataSrcMovieDb, dataRivalMovieDb);
            MovieListProcessed.AddRange(ListMoviesUniqueInRivalDb);

            var ListMoviesUniqueInSourceDb = this.GetMoviesUniqueInSourceDb(dataSrcMovieDb, dataRivalMovieDb);
            MovieListProcessed.AddRange(ListMoviesUniqueInSourceDb);

            return MovieListProcessed;
        }

        public MovieResult<MovieBooking> GetErrorResponseMovieDetail(MovieResult<MovieBooking> ResponseMovieDb)
        {
            return new MovieResult<MovieBooking>
            {
                ErrorCode = ResponseMovieDb.ErrorCode,
                ErrorMessage = ResponseMovieDb.ErrorMessage
            };
        }

        public MovieResult<List<MovieBooking>> GetErrorResponseMovieList(MovieResult<MovieGetResponse> ResponseMovieDb)
        {
            return new MovieResult<List<MovieBooking>>
            {
                ErrorCode = ResponseMovieDb.ErrorCode,
                ErrorMessage = ResponseMovieDb.ErrorMessage
            };
        }

        public  List<MovieBooking> GetCommonMovies(List<MovieBooking> SourceDb, List<MovieBooking> RivalDb)
        {
            var ListCommonMovies = SourceDb.Join(RivalDb,
                                        source => source.Title,
                                        Rival => Rival.Title,
                                        (source, Rival) => new MovieBooking()
                                        {
                                            ID = source.ID,
                                            Title = source.Title,
                                            Year = source.Year,
                                            Type = source.Type,
                                            Poster = source.Poster,
                                            MovieDB = source.MovieDB,
                                            RivalID = Rival.ID,
                                            RivalMovieDB = Rival.MovieDB
                                        }).ToList();

            return ListCommonMovies;
        }

        public  List<MovieBooking> GetMoviesUniqueInRivalDb(List<MovieBooking> SourceDb, List<MovieBooking> RivalDb)
        {
            var ListMoviesUniqueInRivalDb = RivalDb.Except(RivalDb.Where(o => SourceDb.Select(s => s.Title).ToList().Contains(o.Title)))
                                            .Select(entry => new MovieBooking()
                                            {
                                                ID = entry.ID,
                                                Title = entry.Title,
                                                Year = entry.Year,
                                                Type = entry.Type,
                                                Poster = entry.Poster,
                                                MovieDB = entry.MovieDB,
                                            }).ToList();


            return ListMoviesUniqueInRivalDb;
        }

        public  List<MovieBooking> GetMoviesUniqueInSourceDb(List<MovieBooking> SourceDb, List<MovieBooking> RivalDb)
        {
            var ListMoviesUniqueInSourceDb = SourceDb.Except(SourceDb.Where(o => RivalDb.Select(s => s.Title).ToList().Contains(o.Title)))
                                            .Select(entry => new MovieBooking()
                                            {
                                                ID = entry.ID,
                                                Title = entry.Title,
                                                Year = entry.Year,
                                                Type = entry.Type,
                                                Poster = entry.Poster,
                                                MovieDB = entry.MovieDB,
                                            }).ToList();

            return ListMoviesUniqueInSourceDb;
        }

    }
}
