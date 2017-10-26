angular.module('theGameNinja', ["customFilters", "ngRoute", "ui.bootstrap"])
    .config(function ($routeProvider) {
        $routeProvider.when("/videogames", {
            templateUrl: "/views/videogameList.html"
        });
        $routeProvider.when("/newVideogameForm", {
            templateUrl: "/views/vgfTemplate.html"
        });
        $routeProvider.when("/updateVideogameForm/:id", {
            templateUrl: "/views/vgfTemplate.html"
        });
        $routeProvider.otherwise({
                redirectTo: "/videogames"
        });
    })
    .constant("dataUrl", "http://localhost:50555/api/")
    .controller("theGameNinjaCtrl", function ($scope, $http, dataUrl) {

        $scope.data = {};

        $http.get(dataUrl + "Videogames/")
            .success(function (data) {
                $scope.data.videogames = data;
            })
            .error(function (error) {
                $scope.data.error = error;
            });

        $http.get(dataUrl + "Platforms/")
            .success(function (data) {
                $scope.data.platforms = data;
            })
            .error(function (error) {
                $scope.data.error = error;
            });
    })
    .factory('videogameService',

    function ($http) {
        
        

        var getVideogame = function (id) {
            return $http.get("http://localhost:50555/api/Videogames/" + id);
        };

        var insertVideogame = function (videogame) {
            return $http.post("http://localhost:50555/api/Videogames/", videogame);
        };

        var updateVideogame = function (videogame) {
            return $http.put("http://localhost:50555/api/Videogames/" + videogame.Id, videogame);
        };

        var deleteVideogame = function (videogameId) {
            return $http.delete("http://localhost:50555/api/Videogames/" + videogameId);
        }

        var getPlatforms = function () {
            return $http.get("http://localhost:50555/api/Platforms/");
        }

        var getGenres = function () {
            return $http.get("http://localhost:50555/api/Genres/");
        }

        var getMediaTypes = function () {
            return $http.get("http://localhost:50555/api/MediaTypes/");
        }

        return {

            getVideogame: getVideogame,
            insertVideogame: insertVideogame,
            updateVideogame: updateVideogame,
            deleteVideogame: deleteVideogame,

            getPlatforms: getPlatforms,

            getGenres: getGenres,

            getMediaTypes: getMediaTypes
        };
    });
