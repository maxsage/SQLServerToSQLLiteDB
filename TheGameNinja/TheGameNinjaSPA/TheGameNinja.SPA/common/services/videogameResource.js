/**
 * Created by Deb on 8/21/2014.
 */
(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("videogameResource",
                ["$resource",
                 videogameResource]);

    function videogameResource($resource) {
        return $resource("/api/videogames/:Id")
    }

}());
