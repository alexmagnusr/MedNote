'use strict';
Globalsys.factory('gerenciaService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var gerenciaServiceFactory = {};

    gerenciaServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Gerencia/', data).success(function (response) {
            return response;
        });
    };

    gerenciaServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Gerencia').success(function (response) {
            return response;
        });
    };

    gerenciaServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Gerencia?id=' + data.Codigo);
    };

    gerenciaServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/Gerencia/' + data.Codigo, data)
    };

    gerenciaServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/Gerencia/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    return gerenciaServiceFactory;

}]);