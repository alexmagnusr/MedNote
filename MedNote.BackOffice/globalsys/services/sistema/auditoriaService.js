'use strict';
Globalsys.factory('auditoriaService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var auditoriaFactory = {};

    auditoriaFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Auditoria?tipo=' + data).success(function (response) {
            return response;
        });
    };

    return auditoriaFactory;

}]);