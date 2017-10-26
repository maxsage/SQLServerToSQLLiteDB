theGameNinjaApp.controller("HomeController",
    function ($scope, $location, $modal, theGameNinjaApp) {

        $scope.showCreateVideogameForm = function () {

            var videogame = { Id: 0 };

            //$modal.open({
            //    templateUrl: 'controllers/VideogameForm/vgfTemplate.html',
            //    controller: 'vgfController',
            //    resolve: {
            //    videogame: function () {
            //        return videogame;
            //    }
            //}
            //});
            //$location.path('/newVideogameForm');
        };

        $scope.showUpdateVideogameForm = function (id) {

            //videogameService.getVideogame(5).then(
            //    function (results) {
            //        // on success
            //        var videogame = results.data;

            //        $modal.open({
            //            templateUrl: 'controllers/VideogameForm/vgfTemplate.html',
            //            controller: 'vgfController',
            //            resolve: {
            //                videogame: function () {
            //                    return videogame;
            //                }
            //            }
            //        });
            //        //$location.path('/updateVideogameForm/' + id);
            //    }, 
            //    function (results) {
            //        // on error
            //        var data = results.data;
            //    });
        };

        $scope.deleteVideogame = function () {      
            videogameService.deleteVideogame($scope.VideogameId).then(
                function (results) {
                    alert('Videogame ' + $scope.VideogameId
                        + ' deleted successfully.');
                },
                function (results) {
                    alert('Videogame ' + $scope.VideogameId
                        + ' was NOT deleted successfully.');
                });
        };
    });