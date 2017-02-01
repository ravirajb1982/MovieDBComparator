
app.controller('movieController', ['$scope', '$http', '$location', 'MovieService', function ($scope, $http, $location, MovieService) {

    $scope.selectedMovie = "";
    $scope.loaded = false;

    MovieService.getAll()
        .then(function (response) {
            $scope.loaded = true;
            if (response.data.success){
                $scope.movies = response.data.data;               
            } else {                
                $scope.Msg = "Error : " + response.data.errorMessage;
            }
        })
        .catch(function(response) {
            $scope.Msg = "Error : " + response.data.errorMessage;
        });

    $scope.viewDetail = function (movie) {
        $scope.loaded = false;
        MovieService.getMovieDetail(movie)
            .then(function (response) {
                $scope.loaded = true;
                if (response.data.success) {
                    $scope.selectedMovie = response.data.data;
                } else {
                    $scope.Msg = "Error : " + response.data.errorMessage;
                }
            })
        .catch(function (response) {
            $scope.Msg = "Error : " + response.data.errorMessage;
        });
        $location.path('/MovieDetail');
    };

}]);