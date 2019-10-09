'use strict';
Globalsys.factory('painelService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var painelServiceFactory = {};

    painelServiceFactory.consultar = function () {
        return $http.get(serviceBase + 'api/painel/Consultar').success(function (response) {
            return response;
        });
    };
    painelServiceFactory.abrirPainel = function (data) {
        return $http.get(serviceBase + 'api/painel/AbrirPainel?codigo=' + data).success(function (response) {
            return response;
        });
    };

    return painelServiceFactory;

}]);