'use strict';
Globalsys.factory('logErroService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var logErroFactory = {};

    logErroFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/LogErro').success(function (response) {
            return response;
        });
    };

    return logErroFactory;

}]);