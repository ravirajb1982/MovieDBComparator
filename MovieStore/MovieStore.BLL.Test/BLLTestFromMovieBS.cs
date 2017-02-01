using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieStore.BLL.BusinessService;
using MovieStore.BLL.BusinessServiceInterfaces;
using MovieStore.BLL.Repositories;
using MovieStore.BLL.Test.AutofacDI;
using MovieStore.BLL.Test.MockData;
using MovieStore.Models.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Moq;

namespace MovieStore.BLL.Test
{
    [TestClass]
    public class BLLTestFromMovieBS
    {
        IMovieBS _service;
        List<MovieBooking> cinemaworldMockMovies;
        List<MovieBooking> filmworldMockMovies;
        List<MovieBooking> dataSrcMovieDb = new List<MovieBooking>();
        List<MovieBooking> dataRivalMovieDb = new List<MovieBooking>();
        string SourceMovieDb;
        string RivalMovieDb;

     [TestInitialize]
        public void testInit()
        {
            SourceMovieDb = "cinemaworld";
            RivalMovieDb = "filmworld";
            _service = ContainerConfig.BuildContainer().Resolve<IMovieBS>();
            cinemaworldMockMovies =FakeData.GetCinemaWorldMockData();
            filmworldMockMovies = FakeData.GetFilmWorldMockData();

        }

        [TestCleanup]
        public void testClean()
        {
            _service = null;
        }

        [TestMethod]
        public async Task IMovieBS_Should_Give_Accurate_Movie_List_When_GetMovies_is_called_cinemaworld()
        {
            _service.SetMovieDb(SourceMovieDb);
           var listMoviesCinemaWorld = await _service.GetMovies();


            if (listMoviesCinemaWorld.ErrorCode == null)
            {
                List<MovieBooking> ExpectedCinemaWorldMovies = cinemaworldMockMovies;
                List<MovieBooking> ActualCinemaWorldMovies = listMoviesCinemaWorld.Data.movies;

                Assert.AreEqual(ActualCinemaWorldMovies.Count, 7);
                Assert.AreEqual(ActualCinemaWorldMovies[0].Title, "Star Wars: Episode IV - A New Hope");

                for(int i=0; i < ExpectedCinemaWorldMovies.Count;i++ )
                {
                    Assert.AreEqual(ActualCinemaWorldMovies[i].Title, ExpectedCinemaWorldMovies[i].Title);
                }
            }
        }

        [TestMethod]
        public async Task IMovieBS_Should_Give_ConsolidatedUniqueMovie_List_When_GetMovieList_is_called_for_both_cinemaworld_filmworld()
        {
            var taskmovieresult = await _service.GetMovieList(SourceMovieDb, RivalMovieDb);

            if (taskmovieresult.ErrorCode == null)
            {
                var result = taskmovieresult.Data.Count();
                List<MovieBooking> ActualConsolidatedUniqueMovieList = taskmovieresult.Data;
                Assert.AreEqual(result, 7);
                Assert.AreEqual(ActualConsolidatedUniqueMovieList[0].Title, "Star Wars: Episode IV - A New Hope");
                Assert.AreEqual(ActualConsolidatedUniqueMovieList[1].Title, "Star Wars: Episode V - The Empire Strikes Back");
                Assert.AreEqual(ActualConsolidatedUniqueMovieList[2].Title, "Star Wars: Episode VI - Return of the Jedi");
                Assert.AreEqual(ActualConsolidatedUniqueMovieList[3].Title, "Star Wars: Episode I - The Phantom Menace");
                Assert.AreEqual(ActualConsolidatedUniqueMovieList[4].Title, "Star Wars: Episode III - Revenge of the Sith");
                Assert.AreEqual(ActualConsolidatedUniqueMovieList[5].Title, "Star Wars: Episode II - Attack of the Clones");
                Assert.AreEqual(ActualConsolidatedUniqueMovieList[6].Title, "Star Wars: The Force Awakens");      
            
            }
        }

        [TestMethod]
        public async Task IMovieBS_Should_Give_CommonMovies_List_When_GetCommonMovies_is_called_for_both_cinemaworldData_filmworldData()
        {
            
            _service.SetMovieDb(SourceMovieDb);
            var ResponseSourceMovieDb = await _service.GetMovies();

            dataSrcMovieDb = _service.UpdateMovieDBField(ResponseSourceMovieDb, SourceMovieDb);

            _service.SetMovieDb(RivalMovieDb);
            var ResponseRivalMovieDb = await _service.GetMovies();

            dataRivalMovieDb = _service.UpdateMovieDBField(ResponseRivalMovieDb, RivalMovieDb);

            List <MovieBooking> ActualCommonMoviesList= _service.GetCommonMovies(dataSrcMovieDb, dataRivalMovieDb);

            var result = ActualCommonMoviesList.Count;

            Assert.AreEqual(result, 6);
            Assert.AreEqual(ActualCommonMoviesList[0].Title, "Star Wars: Episode IV - A New Hope");
            Assert.AreEqual(ActualCommonMoviesList[1].Title, "Star Wars: Episode V - The Empire Strikes Back");
            Assert.AreEqual(ActualCommonMoviesList[2].Title, "Star Wars: Episode VI - Return of the Jedi");
            Assert.AreEqual(ActualCommonMoviesList[3].Title, "Star Wars: Episode I - The Phantom Menace");
            Assert.AreEqual(ActualCommonMoviesList[4].Title, "Star Wars: Episode III - Revenge of the Sith");
            Assert.AreEqual(ActualCommonMoviesList[5].Title, "Star Wars: Episode II - Attack of the Clones");
        }


        [TestMethod]
        public void IMovieBS_UsingMockData_Should_Give_CommonMovies_List_When_GetCommonMovies_is_called_for_both_cinemaworldData_filmworldData()
        {

            var dataSrcMovieDb = cinemaworldMockMovies;
            dataSrcMovieDb.ForEach(c => c.MovieDB =SourceMovieDb);

            var dataRivalMovieDb = filmworldMockMovies;
            dataRivalMovieDb.ForEach(c => c.MovieDB = RivalMovieDb);

            List<MovieBooking> ActualCommonMoviesList = _service.GetCommonMovies(dataSrcMovieDb, dataRivalMovieDb);

            var result = ActualCommonMoviesList.Count;

            Assert.AreEqual(result, 6);
            Assert.AreEqual(ActualCommonMoviesList[0].Title, "Star Wars: Episode IV - A New Hope");
            Assert.AreEqual(ActualCommonMoviesList[1].Title, "Star Wars: Episode V - The Empire Strikes Back");
            Assert.AreEqual(ActualCommonMoviesList[2].Title, "Star Wars: Episode VI - Return of the Jedi");
            Assert.AreEqual(ActualCommonMoviesList[3].Title, "Star Wars: Episode I - The Phantom Menace");
            Assert.AreEqual(ActualCommonMoviesList[4].Title, "Star Wars: Episode III - Revenge of the Sith");
            Assert.AreEqual(ActualCommonMoviesList[5].Title, "Star Wars: Episode II - Attack of the Clones");
        }


        [TestMethod]
        public async Task IMovieBS_Should_Give_MoviesUniqueInRivalDb_List_When_GetMoviesUniqueInRivalDb_is_called_for_both_cinemaworldData_filmworldData()
        {

            var dataSrcMovieDb = new List<MovieBooking>();
            var dataRivalMovieDb = new List<MovieBooking>();

            _service.SetMovieDb(SourceMovieDb);
            var ResponseSourceMovieDb = await _service.GetMovies();

            dataSrcMovieDb = _service.UpdateMovieDBField(ResponseSourceMovieDb, SourceMovieDb);

            _service.SetMovieDb(RivalMovieDb);
            var ResponseRivalMovieDb = await _service.GetMovies();

            dataRivalMovieDb = _service.UpdateMovieDBField(ResponseRivalMovieDb, RivalMovieDb);


            List<MovieBooking> ListMoviesUniqueInRivalDbnMovies = _service.GetMoviesUniqueInRivalDb(dataSrcMovieDb, dataRivalMovieDb);

            var result = ListMoviesUniqueInRivalDbnMovies.Count;

            Assert.AreEqual(result, 0);
        }



        [TestMethod]
        public void IMovieBS_UsingMockData_Should_Give_MoviesUniqueInRivalDb_List_When_GetMoviesUniqueInRivalDb_is_called_for_both_cinemaworldData_filmworldData()
        {

            var dataSrcMovieDb = cinemaworldMockMovies;
            dataSrcMovieDb.ForEach(c => c.MovieDB = SourceMovieDb);

            var dataRivalMovieDb = filmworldMockMovies;
            dataRivalMovieDb.ForEach(c => c.MovieDB = RivalMovieDb);

            List<MovieBooking> ListMoviesUniqueInRivalDbnMovies = _service.GetMoviesUniqueInRivalDb(dataSrcMovieDb, dataRivalMovieDb);

            var result = ListMoviesUniqueInRivalDbnMovies.Count;

            Assert.AreEqual(result, 0);
        }


        [TestMethod]
        public async Task IMovieBS_Should_Give_MoviesUniqueInSourceDb_List_When_GetMoviesUniqueInSourceDb_is_called_for_both_cinemaworldData_filmworldData()
        {

            var dataSrcMovieDb = new List<MovieBooking>();
            var dataRivalMovieDb = new List<MovieBooking>();

            _service.SetMovieDb(SourceMovieDb);
            var ResponseSourceMovieDb = await _service.GetMovies();

            dataSrcMovieDb = _service.UpdateMovieDBField(ResponseSourceMovieDb, SourceMovieDb);

            _service.SetMovieDb(RivalMovieDb);
            var ResponseRivalMovieDb = await _service.GetMovies();

            dataRivalMovieDb = _service.UpdateMovieDBField(ResponseRivalMovieDb, RivalMovieDb);


            List<MovieBooking> ActualListMoviesUniqueInSourceDb = _service.GetMoviesUniqueInSourceDb(dataSrcMovieDb, dataRivalMovieDb);

            var result = ActualListMoviesUniqueInSourceDb.Count;

            Assert.AreEqual(result,1);
            Assert.AreEqual(ActualListMoviesUniqueInSourceDb[0].Title, "Star Wars: The Force Awakens");
        }


        [TestMethod]
        public void IMovieBS_UsingMockData_Should_Give_MoviesUniqueInSourceDb_List_When_GetMoviesUniqueInSourceDb_is_called_for_both_cinemaworldData_filmworldData()
        {

            var dataSrcMovieDb = cinemaworldMockMovies;
            dataSrcMovieDb.ForEach(c => c.MovieDB = SourceMovieDb);

            var dataRivalMovieDb = filmworldMockMovies;
            dataRivalMovieDb.ForEach(c => c.MovieDB = RivalMovieDb);

            List<MovieBooking> ActualListMoviesUniqueInSourceDb = _service.GetMoviesUniqueInSourceDb(dataSrcMovieDb, dataRivalMovieDb);

            var result = ActualListMoviesUniqueInSourceDb.Count;

            Assert.AreEqual(result, 1);
            Assert.AreEqual(ActualListMoviesUniqueInSourceDb[0].Title, "Star Wars: The Force Awakens");
        }


        [TestMethod]
        public async Task IMovieBS_Should_Give_Cheaper_MovieDB_When_GetMovieDetail_is_called_with_Selected_Movie()
        {
            MovieBooking selectedMovie = new MovieBooking()
            {
                Title = "Star Wars: Episode IV - A New Hope",
                Year = "1977",
                Rated = null,
                Released = null,
                Runtime = null,
                Genre = null,
                Director = null,
                Writer = null,
                Actors = null,
                Plot = null,
                Language = null,
                Country = null,
                Poster = "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg",
                Metascore = null,
                Rating = null,
                Votes = null,
                ID = "cw0076759",
                Type = "movie",
                Price = 0,
                MovieDB = "cinemaworld",
                RivalMovieDB = "filmworld",
                RivalID = "fw0076759",
                RivalPrice = 0,
                RivalPriceCheaper = false
            };

             var dataMovieDetail = await _service.GetMovieDetail(selectedMovie).ConfigureAwait(false);

            if (dataMovieDetail.ErrorCode == null)
            {
                Assert.AreEqual(dataMovieDetail.Data.RivalPrice, 29.5);
                Assert.AreEqual(dataMovieDetail.Data.Price, 123.5);
                Assert.AreEqual(dataMovieDetail.Data.RivalPriceCheaper, true);
            }
        }

        [TestMethod]
        public async Task IMovieBS_Should_Give_Accurate_Movie_List_When_GetMovies_is_called_filmworld()
        {
            _service.SetMovieDb(RivalMovieDb);
            var listMoviesFilmWorld = await _service.GetMovies();


            if (listMoviesFilmWorld.ErrorCode == null)
            {
                List<MovieBooking> ExpectedFilmWorldMovies = filmworldMockMovies;
                List<MovieBooking> ActualFilmWorldMovies = listMoviesFilmWorld.Data.movies;

                Assert.AreEqual(ActualFilmWorldMovies.Count, 6);
                Assert.AreEqual(ActualFilmWorldMovies[0].Title, "Star Wars: Episode IV - A New Hope");

                for (int i = 0; i < ExpectedFilmWorldMovies.Count; i++)
                {
                    Assert.AreEqual(ActualFilmWorldMovies[i].Title, ExpectedFilmWorldMovies[i].Title);
                }
            }
        }

        [TestMethod]
        public async Task IMovieBS_Should_Give_Movie_Detail_When_GetMovieById_is_called_cinemaworld()
        {
            _service.SetMovieDb(SourceMovieDb);
            var MovieCinemaWorld = await _service.GetMovieById("cw0076759");

           if (MovieCinemaWorld.ErrorCode == null)
            {
                MovieBooking actual = MovieCinemaWorld.Data;

                MovieBooking expected = new MovieBooking()
                {
                    Title = "Star Wars: Episode IV - A New Hope",
                    Year = "1977",
                    Rated = "PG",
                    Released = "25 May 1977",
                    Runtime = "121 min",
                    Genre = "Action, Adventure, Fantasy",
                    Director = "George Lucas",
                    Writer = "George Lucas",
                    Actors = "Mark Hamill, Harrison Ford, Carrie Fisher, Peter Cushing",
                    Plot = "Luke Skywalker joins forces with a Jedi Knight, a cocky pilot, a wookiee and two droids to save the galaxy from the Empire's world-destroying battle-station, while also attempting to rescue Princess Leia from the evil Darth Vader.",
                    Language = "English",
                    Country = "USA",
                    Poster = "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTcwOTk@._V1_SX300.jpg",
                    Metascore = "92",
                    Rating = "8.7",
                    Votes = "915,459",
                    ID = "cw0076759",
                    Type = "movie",
                    Price = 123.5,
                    MovieDB = null,
                    RivalMovieDB = null,
                    RivalID = null,
                    RivalPrice = 0,
                    RivalPriceCheaper = false
                };

                Assert.AreEqual(actual.Title, expected.Title);
                Assert.AreEqual(actual.Actors, expected.Actors);
                Assert.AreEqual(actual.Year, expected.Year);

                Assert.AreEqual(actual.Rated, expected.Rated);
                Assert.AreEqual(actual.Released, expected.Released);
                Assert.AreEqual(actual.Runtime, expected.Runtime);
                Assert.AreEqual(actual.Genre, expected.Genre);

                Assert.AreEqual(actual.Director, expected.Director);
                Assert.AreEqual(actual.Writer, expected.Writer);
                Assert.AreEqual(actual.Plot, expected.Plot);
                Assert.AreEqual(actual.Language, expected.Language);

                Assert.AreEqual(actual.Country, expected.Country);
                Assert.AreEqual(actual.Poster, expected.Poster);
                Assert.AreEqual(actual.Metascore, expected.Metascore);

                Assert.AreEqual(actual.Rating, expected.Rating);
                Assert.AreEqual(actual.Votes, expected.Votes);
                Assert.AreEqual(actual.ID, expected.ID);
                Assert.AreEqual(actual.Type, expected.Type);

                Assert.AreEqual(actual.Price, expected.Price);
                Assert.AreEqual(actual.MovieDB, expected.MovieDB);
                Assert.AreEqual(actual.RivalMovieDB, expected.RivalMovieDB);
                Assert.AreEqual(actual.RivalID, expected.RivalID);

                Assert.AreEqual(actual.RivalPrice, expected.RivalPrice);
                Assert.AreEqual(actual.RivalPriceCheaper, expected.RivalPriceCheaper);
            }
           
        }


        [TestMethod]
        public async Task IMovieBS_Should_Give_Movie_Detail_When_GetMovieById_is_called_filmworld()
        {
            _service.SetMovieDb(RivalMovieDb);
            var MovieFilmWorld = await _service.GetMovieById("fw0076759");

            if (MovieFilmWorld.ErrorCode == null)
            {
                MovieBooking actual = MovieFilmWorld.Data;

                MovieBooking expected = new MovieBooking()
                {
                    Title = "Star Wars: Episode IV - A New Hope",
                    Year = "1977",
                    Rated = "PG",
                    Released = "25 May 1977",
                    Runtime = "121 min",
                    Genre = "Action, Adventure, Fantasy",
                    Director = "George Lucas",
                    Writer = "George Lucas",
                    Actors = "Mark Hamill, Harrison Ford, Carrie Fisher, Peter Cushing",
                    Plot = "Luke Skywalker joins forces with a Jedi Knight, a cocky pilot, a wookiee and two droids to save the galaxy from the Empire's world-destroying battle-station, while also attempting to rescue Princess Leia from the evil Darth Vader.",
                    Language = "English",
                    Country = "USA",
                    Poster = "http://ia.media-imdb.com/images/M/MV5BOTIyMDY2NGQtOGJjNi00OTk4LWFhMDgtYmE3M2NiYzM0YTVmXkEyXkFqcGdeQXVyNTU1NTfwOTk@._V1_SX300.jpg",
                    Metascore = "92",
                    Rating = "8.7",
                    Votes = "915,459",
                    ID = "fw0076759",
                    Type = "movie",
                    Price = 29.5,
                    MovieDB = null,
                    RivalMovieDB = null,
                    RivalID = null,
                    RivalPrice = 0,
                    RivalPriceCheaper = false
                };

                Assert.AreEqual(actual.Title, expected.Title);
                Assert.AreEqual(actual.Actors, expected.Actors);
                Assert.AreEqual(actual.Year, expected.Year);

                Assert.AreEqual(actual.Rated, expected.Rated);
                Assert.AreEqual(actual.Released, expected.Released);
                Assert.AreEqual(actual.Runtime, expected.Runtime);
                Assert.AreEqual(actual.Genre, expected.Genre);

                Assert.AreEqual(actual.Director, expected.Director);
                Assert.AreEqual(actual.Writer, expected.Writer);
                Assert.AreEqual(actual.Plot, expected.Plot);
                Assert.AreEqual(actual.Language, expected.Language);

                Assert.AreEqual(actual.Country, expected.Country);
                Assert.AreEqual(actual.Poster, expected.Poster);
                Assert.AreEqual(actual.Metascore, expected.Metascore);

                Assert.AreEqual(actual.Rating, expected.Rating);
                Assert.AreEqual(actual.Votes, expected.Votes);
                Assert.AreEqual(actual.ID, expected.ID);
                Assert.AreEqual(actual.Type, expected.Type);

                Assert.AreEqual(actual.Price, expected.Price);
                Assert.AreEqual(actual.MovieDB, expected.MovieDB);
                Assert.AreEqual(actual.RivalMovieDB, expected.RivalMovieDB);
                Assert.AreEqual(actual.RivalID, expected.RivalID);

                Assert.AreEqual(actual.RivalPrice, expected.RivalPrice);
                Assert.AreEqual(actual.RivalPriceCheaper, expected.RivalPriceCheaper);
            }
        }

        [TestMethod]
        public async Task IMovieBS_Should_Give_UnknownError_When_GetMovies_is_called_forWrongMovieDb()
        {
            _service.SetMovieDb("cinemaworld1");
            var listMoviesCinemaWorld = await _service.GetMovies();

            var errorcode = listMoviesCinemaWorld.ErrorCode.Value.ToString();
            var errorMessage = listMoviesCinemaWorld.ErrorMessage.ToString();

            Assert.AreEqual(errorcode, "UNKNOWN_ERROR");
        }
    }
}
