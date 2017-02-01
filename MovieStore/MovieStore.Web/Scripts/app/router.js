
app.config(['$httpProvider', '$routeProvider', '$locationProvider', function ($httpProvider, $routeProvider, $locationProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];

    $routeProvider
      .when('/home', {
          templateUrl: 'MovieList.html'
      })
      .when('/MovieDetail', {
          templateUrl: '/Templates/Movies/MovieDetail.html'
      });
    $routeProvider.otherwise({ redirectTo: '/home' });
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
}]);
