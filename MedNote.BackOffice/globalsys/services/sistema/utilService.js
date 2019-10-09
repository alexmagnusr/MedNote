'use strict';
Globalsys.factory('utilService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var utilFactory = {};

    utilFactory.ConsultarCep = function (data) {
        return $http.get(serviceBase + 'api/Util/ConsultarCep/' + data).success(function (response) {
            return response;
        });
    };

    utilFactory.ConsultarEstado = function (data) {
        return $http.get(serviceBase + 'api/Util/ConsultarEstado/').success(function (response) {
            return response;
        });
    };

    utilFactory.ConsultarCidades = function (data) {
        return $http.get(serviceBase + 'api/Util/ConsultarCidades/' + data).success(function (response) {
            return response;
        });
    };


    return utilFactory;

}]);