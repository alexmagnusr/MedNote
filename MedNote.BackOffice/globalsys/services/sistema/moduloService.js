'use strict';
Globalsys.factory('moduloService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var moduloServiceFactory = {};


    moduloServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Modulo').success(function (response) {
            return response;
        });
    };

   

    return moduloServiceFactory;

}]);