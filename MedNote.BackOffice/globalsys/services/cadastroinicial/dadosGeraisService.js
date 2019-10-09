'use strict';
Globalsys.factory('dadosGeraisService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var dadosGeraisServiceFactory = {};

    dadosGeraisServiceFactory.carregarCidades = function (data) {
        return $http.get(serviceBase + 'api/Cidade/CarregarCidades?codigoEstado=' + data).success(function (response) {
            return response;
        });
    };

    dadosGeraisServiceFactory.listarEstados = function (data) {
        return $http.get(serviceBase + 'api/Estado/ListarEstados/').success(function (response) {
            return response;
        });
    };

    return dadosGeraisServiceFactory;

}]);