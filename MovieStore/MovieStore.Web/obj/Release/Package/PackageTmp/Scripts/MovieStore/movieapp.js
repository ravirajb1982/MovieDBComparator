var myApp = angular.module('myApp', ['ngRoute']);


myApp.config(['$httpProvider','$routeProvider', '$locationProvider', function ($httpProvider,$routeProvider, $locationProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];

    $routeProvider
      .when('/home', {
          templateUrl: 'home.html'
      })
      .when('/confirm', {
          templateUrl: 'confirm.html'
      })
      .when('/checkout', {
          templateUrl: 'checkout.html'
      })
      .when('/thankyou', {
          templateUrl: 'thankyou.html'
      });
    $routeProvider.otherwise({ redirectTo: '/home' });
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);


myApp.controller('DataController', ['$scope', '$http', '$location', function ($scope, $http, $location) {

    $scope.selectedMovie = "";
    $scope.loaded = false;
    $scope.shipping = 2;
    $http.get('/Home/GetMovies').success(function (data) {
        $scope.movies = data;
        $scope.loaded = true;
    });
    $scope.buy = function (movie) {
        $scope.status = "buy";
        $http.get('/Home/GetMovieDetails/', { params: movie }).success(function (data) {
            $scope.selectedMovie = data;
        });
        $location.path('/confirm');
    };
    $scope.rent = function (movie) {
        $scope.status = "rent"
        $scope.selectedMovie = movie;
        $location.path('/confirm');
    };
    $scope.checkout = function () {
        $location.path('/checkout');
    }
    $scope.pay = function () {
        $location.path('/thankyou')
    }
}]);