//1.
describe('Test Movie service', function () {
    var MovieService, httpBackend;
    //2.
    beforeEach(function () {
        //3. load the module.
        module('movieApp');

        // 4. get your service, also get $httpBackend
        // $httpBackend will be a mock.
        inject(function ($httpBackend, _MovieService_) {
            MovieService = _MovieService_;
            httpBackend = $httpBackend;
        });
    });

    // 5. make sure no expectations were missed in your tests.
    afterEach(function () {
        httpBackend.verifyNoOutstandingExpectation();
        httpBackend.verifyNoOutstandingRequest();
    });

    //6.
    it('Service get All Movies', function () {

        var returnData = {};

        //7. expectGET to make sure this is called once.
        httpBackend.expectGET("/Movie/GetMovies").respond(returnData);

        //8. make the call.
        var returnedPromise = MovieService.getAll();

        //9. set up a handler for the response, that will put the result
        // into a variable in this scope for you to test.
        var result;
        returnedPromise.then(function (response) {
            result = response.data;
        });

        //10. flush the backend to "execute" the request to do the expectedGET assertion.
        httpBackend.flush();

        //11. check the result. 

        expect(result).toEqual(returnData);

    });


});