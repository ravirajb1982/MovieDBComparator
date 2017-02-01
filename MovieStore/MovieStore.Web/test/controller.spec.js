describe("App", function () {

    beforeEach(module('movieApp'));

    describe("movieController", function () {

        var scope, httpBackend, http, controller;
        beforeEach(inject(function ($rootScope, $controller, $httpBackend, $http) {
            scope = $rootScope.$new();
            httpBackend = $httpBackend;
            http = $http;
            controller = $controller;
            httpBackend.when("GET", "/Movie/GetMovies").respond([{}, {}, {}, {}, {}, {}, {}, {}]);
        }));

        it('should GET movies', function () {
            httpBackend.expectGET('/Movie/GetMovies');
            controller('movieController', {
                $scope: scope,
                $http: http
            });
            httpBackend.flush();
        });
    });
});