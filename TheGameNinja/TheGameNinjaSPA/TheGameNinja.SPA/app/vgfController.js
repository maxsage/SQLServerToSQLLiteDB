
angular.module("theGameNinja")
    .controller('vgfController',
    function vgfController($scope, $window, $routeParams, $modalInstance, videogameService, videogame) {
        //if ($routeParams.id) {
        //    $scope.pageTitle = "Update Videogame";
        //    //$scope.videogame = $scope.data.videogames[87];
        //    $scope.editableVideogame = angular.copy($scope.data.videogames[87]);
        //    //videogameService.getVideogame($routeParams.id).then(
        //    //    function (results) {
        //    //        $scope.videogame = results.data;
        //    //        $scope.editableVideogame = angular.copy($scope.videogame);
        //    //    },
        //    //    function (results) {
        //    //        var data = results.data;
        //    //    });
        //} else {
        //    $scope.pageTitle = "Create Videogame";
        //    videogame = { id: 0 };
        //    $scope.editableVideogame = angular.copy($scope.videogame);
        //}

        if (videogame.Id == 0) {
            pageTitle = "Create Videogame";
            $scope.editableVideogame = angular.copy(videogame);
        }
        else {
            pageTitle = "Update Videogame";
            $scope.editableVideogame = angular.copy(videogame);
        }
        
        
        videogameService.getPlatforms().then(
            function (results) {
                // on success
                $scope.Platforms = results.data;
            },
            function (results) {
                // on error
                $scope.Platforms = results.data;
            }
        );

        videogameService.getGenres().then(
            function (results) {
                // on success
                $scope.Genres = results.data;
            },
            function (results) {
                // on error
                $scope.Genres = results.data;
            }
        );

        videogameService.getMediaTypes().then(
            function (results) {
                // on success
                $scope.MediaTypes = results.data;
            },
            function (results) {
                // on error
                $scope.MediaTypes = results.data;
            }
        );

        // Datepicker setup
        // Is Datepicker open
        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        // Rating setup
        $scope.hoveringOver = function (value) {
            $scope.overStar = value;
            $scope.percent = 100 * (value / 10);
        }

        $scope.submitForm = function () {

            if ($scope.editableVideogame.Id == 0) {
                // insert the videogame
                videogameService.insertVideogame($scope.editableVideogame);
            } else {
                // update the videogame
                videogameService.updateVideogame($scope.editableVideogame);
            }

            videogame = angular.copy($scope.editableVideogame);

            $modalInstance.close(videogame);
        }

        $scope.cancelForm = function () {
            //$window.history.back();

            $modalInstance.dismiss();

        };
    });