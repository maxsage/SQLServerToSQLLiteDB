/**
 * Created by Deb on 8/21/2014.
 */
(function () {
    "use strict";

    var app = angular
                .module("videogameResourceMock",
                        ["ngMockE2E"]);

    app.run(function ($httpBackend) {
        var products = [
            {
                "Id": 1,
                "Name": "Halo",
                "Description": "Halo has Masterchief in it.",
                "GenreId": "1",
                "PlatformId": "1",
                "MediaTypeId": "2",
                "ImageUrl": "someimage.jpg",
                "DatePurchased": "March 19, 2009",
                "Notes": "Some Notes",
                "Rating": "4",
                "CurrentlyPlaying": "true",
                "Completed": "false"
                },
            {
                "Id": 2,
                "Name": "God of War",
                "Description": "Kratos.",
                "GenreId": "1",
                "PlatformId": "1",
                "MediaTypeId": "2",
                "ImageUrl": "someimage.jpg",
                "DatePurchased": "March 19, 2009",
                "Notes": "Some Notes",
                "Rating": "4",
                "CurrentlyPlaying": "true",
                "Completed": "false"
            },
            {
                "Id": 2,
                "Name": "Splatoon",
                "Description": "Paint ball game.",
                "GenreId": "1",
                "PlatformId": "1",
                "MediaTypeId": "2",
                "ImageUrl": "someimage.jpg",
                "DatePurchased": "March 19, 2009",
                "Notes": "Some Notes",
                "Rating": "4",
                "CurrentlyPlaying": "true",
                "Completed": "false"
            }
        ];

        var videogameUrl = "/api/videogames"

        $httpBackend.whenGET(videogameUrl).respond(products);

        var editingRegex = new RegExp(videogameUrl + "/[0-9][0-9]*", '');
        $httpBackend.whenGET(editingRegex).respond(function (method, url, data) {
            var videogame = { "Id": 0 };
            var parameters = url.split('/');
            var length = parameters.length;
            var id = parameters[length - 1];

            if (id > 0) {
                for (var i = 0; i < videogames.length; i++) {
                    if (videogames[i].Id == id) {
                        videogame = videogames[i];
                        break;
                    }
                };
            }
            return [200, videogame, {}];
        });

        $httpBackend.whenPOST(videogameUrl).respond(function (method, url, data) {
            var videogame = angular.fromJson(data);

            if (!videogame.Id) {
                videogame.Id = videogames[videogames.length - 1].Id + 1;
                videogames.push(videogame);
            }
            else {
                // Updated product
                for (var i = 0; i < videogames.length; i++) {
                    if (videogames[i].Id == videogame.Id) {
                        videogames[i] = videogame;
                        break;
                    }
                };
            }
            return [200, videogame, {}];
        });

        // Pass through any requests for application files
        $httpBackend.whenGET(/app/).passThrough();


    })
}());
