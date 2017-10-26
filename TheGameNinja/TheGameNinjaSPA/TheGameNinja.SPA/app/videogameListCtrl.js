angular.module("theGameNinja")
    .constant("videogameListActiveClass", "btn-primary")
    .constant("videogameListPageCount", 6)
    .controller("videogameListCtrl", function ($scope, $filter, $location, $modal,
        videogameListActiveClass, videogameListPageCount) {

        var selectedPlatform = null;

        $scope.selectPlatform = function (newPlatform) {
            selectedPlatform = newPlatform;
            $scope.selectedPage = 1;
        }

        $scope.platformFilterFn = function (videogame) {
            return selectedPlatform == null ||
                videogame.PlatformId == selectedPlatform;
        }

        $scope.getPlatformClass = function (platform) {
            return selectedPlatform == platform ? videogameListActiveClass : "";
        }

        $scope.editVideogame = function (videogame, index) {
            var modalInstance =
                $modal.open({
                    templateUrl: 'views/vgfTemplate.html',
                    controller: 'vgfController',
                    resolve: {
                        videogame: function () {
                            return videogame;
                        }
                    }
                });

            modalInstance.result.then(function (result) {

                $scope.$parent.data.videogames[index] = result;
                //$scope.$parent.data.videogames.splice(index, 1, result);
                //angular.extend($scope$scope.$parent.data.videogames[index], result);
                
            });
        }


        $scope.createVideogame = function () {
            var videogame = { Id: 0 };
            var modalInstance =
                $modal.open({
                    templateUrl: 'views/vgfTemplate.html',
                    controller: 'vgfController',
                    resolve: {
                        videogame: function () {
                            return videogame;
                        }
                    }
                });

            modalInstance.result.then(function (result) {
                $scope.$parent.data.videogames.push(result);
                //alert("Video game inserted.");
            });

        }
    });