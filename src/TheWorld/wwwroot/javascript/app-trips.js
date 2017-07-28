//(function () {
//    "use strict";
//    angular.module("app-trips", ["simpleControls"]);
//})();


// copied code - I am using viewModel instead of vm
(function () {

    "use strict";

    // Creating the Module
    angular.module("app-trips", ["simpleControls", "ngRoute"])
      .config(function ($routeProvider) {

          $routeProvider.when("/", {
              controller: "tripsController",
              controllerAs: "viewModel",
              templateUrl: "/views/tripsView.html"
          });

          $routeProvider.when("/editor/:tripName", {
              controller: "tripEditorController",
              controllerAs: "viewModel",
              templateUrl: "/views/tripEditorView.html"
          });

          $routeProvider.otherwise({ redirectTo: "/" });

      });

})();

