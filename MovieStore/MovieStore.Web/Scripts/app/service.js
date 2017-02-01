//app.factory('MovieService', function ($http) {
//    crudMovieObj = {};

//    crudMovieObj.getAll = function () {
//        var movies;

//        movies = $http({ method: 'get', url: '/Movie/GetMovies' })
//        .then(function (response) {
//            return response;
//        });
//        return movies;
//    };

//    crudMovieObj.getMovieDetail = function (movie) {
//        var mov;

//        mov = $http({ method: 'get', url: '/Movie/GetMovieDetail/', params: movie })
//        .then(function (response) {
//            return response;
//        });
//        return mov;
//    };

//    return crudMovieObj;
//});

app.service('MovieService', function ($http) {

    this.getAll = function () {
        var movies;
        movies = $http({ method: 'get', url: '/Movie/GetMovies' });
        return movies;
    };

    this.getMovieDetail = function (movie) {
        var mov;
        mov = $http({ method: 'get', url: '/Movie/GetMovieDetail/', params: movie });
        return mov;
    };

});