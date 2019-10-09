'use strict';
Globalsys.factory('tipoSetorClienteService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var tipoSetorClienteServiceFactory = {};

    tipoSetorClienteServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/tipoSetorCliente/Cadastro/', data).success(function (response) {
            return response;
        });
    };

    tipoSetorClienteServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/tipoSetorCliente/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    tipoSetorClienteServiceFactory.ConsultarTipoSetorPorCliente = function (data) {
        return $http.get(serviceBase + 'api/tipoSetorCliente/ConsultarTipoSetorPorCliente?codigo=' + data).success(function (response) {
            return response;
        });
    };

    return tipoSetorClienteServiceFactory;

}]);