(function () {
    "use strict";

    angular.module("app-trips")
           .controller("tripsController", tripsController);

    function tripsController($http) {
        var viewModel = this;
        viewModel.trips = [];
        viewModel.newTrip = {};
        viewModel.errorMessage = "";
        viewModel.isBusy = true;

        $http.get("/api/trips")
            .then(function (response) {
                angular.copy(response.data, viewModel.trips);
            }, function (error) {
                viewModel.errorMessage = "Unable to load data: " + error;
            })
            .finally(function () {
                viewModel.isBusy = false;
            });

        viewModel.addTrip = function () {
            viewModel.isBusy = true;
            viewModel.errorMessage = "";

            $http.post("/api/trips", viewModel.newTrip)
                .then(function (response) {
                    viewModel.trips.push(response.data);
                    viewModel.newTrip = {};
                }, function () {
                    viewModel.errorMessage = "Unable to save trip";
                })
                .finally(function () {
                    viewModel.isBusy = false;
                });
        };
    }
})();
