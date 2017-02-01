using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieStore.Service.Interfaces;
using MovieStore.Service.Services;
using MovieStore.Models.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using MovieStore.BLL.Test.MockData;
using MovieStore.Test.AutofacDI;
using Autofac;

namespace MovieStore.Test
{
    [TestClass]
    public class ServiceTest
    {
        IMovieService _service;
        List<MovieBooking> cinemaworldMockMovies;
        List<MovieBooking> filmworldMockMovies;
        string SourceMovieDb;
        string RivalMovieDb;

        [TestInitialize]
        public void testInit()
        {
            SourceMovieDb = "cinemaworld";
            RivalMovieDb = "filmworld";
            _service = ContainerConfig.BuildContainer().Resolve<IMovieService>();
            cinemaworldMockMovies = FakeData.GetCinemaWorldMockData();
            filmworldMockMovies = FakeData.GetFilmWorldMockData();
        }

        [TestCleanup]
        public void testClean()
        {
            _service = null;
        }

        [TestMethod]
        public async Task Service_Should_Give_Accurate_Movie_List_When_GetMovies_is_called_cinemaworld()
        {
            _service.SetMovieDb(SourceMovieDb);
            var listMoviesCinemaWorld = await _service.GetMovies();

            List<MovieBooking> ExpectedCinemaWorldMovies = cinemaworldMockMovies;
            List<MovieBooking> ActualCinemaWorldMovies = listMoviesCinemaWorld.Data.movies;

            Assert.AreEqual(ActualCinemaWorldMovies.Count, 7);
            Assert.AreEqual(ActualCinemaWorldMovies[0].Title, "Star Wars: Episode IV - A New Hope");

            for (int i = 0; i < ExpectedCinemaWorldMovies.Count; i++)
            {
                Assert.AreEqual(ActualCinemaWorldMovies[i].Title, ExpectedCinemaWorldMovies[i].Title);
            }

        }

            [TestMethod]
            public async Task Service_Should_Give_Accurate_Movie_List_When_GetMovies_is_called_filmworld()
            {
                _service.SetMovieDb(RivalMovieDb);
                 var  listMoviesFilmWorld = await _service.GetMovies();

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
            public async Task Service_Should_Give_Movie_Detail_When_GetMovieById_is_called_cinemaworld()
            {
                _service.SetMovieDb(SourceMovieDb);
                var MovieCinemaWorld = await _service.GetMovieById("cw0076759");

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
                    MovieDB =null,
                    RivalMovieDB=null,
                    RivalID =null,
                    RivalPrice =0,
                    RivalPriceCheaper =false
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


            [TestMethod]
            public async Task Service_Should_Give_Movie_Detail_When_GetMovieById_is_called_filmworld()
            {
                _service.SetMovieDb(RivalMovieDb);
                var MovieFilmWorld = await _service.GetMovieById("fw0076759");

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

            [TestMethod]
            public async Task Service_Should_Give_UnknownError_When_GetMovies_is_called_forWrongMovieDb()
            {
                _service.SetMovieDb("cinemaworld1");
                var  listMoviesCinemaWorld = await _service.GetMovies();

                var errorcode = listMoviesCinemaWorld.ErrorCode.Value.ToString();
                var errorMessage = listMoviesCinemaWorld.ErrorMessage.ToString();

                Assert.AreEqual(errorcode, "UNKNOWN_ERROR");
            }
    }


}
