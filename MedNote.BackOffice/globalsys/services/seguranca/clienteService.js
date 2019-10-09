'use strict';
Globalsys.factory('clienteService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var clienteServiceFactory = {};

    clienteServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Cliente/Cadastro/', data).success(function (response) {
            return response;
        });
    };

    clienteServiceFactory.consultar = function () {
        
        return $http.get(serviceBase + 'api/Cliente/Consulta').success(function (response) {
            return response;
        });
    };

    clienteServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Cliente/Edita?id=' + data.Codigo);
    };

    clienteServiceFactory.atualizar = function (data) {
        return $http.post(serviceBase + 'api/Cliente/Atualiza?id=' + data.Codigo, data);
    };

    clienteServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/Cliente/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    return clienteServiceFactory;

}]);