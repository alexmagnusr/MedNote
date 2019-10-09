'use strict';
Globalsys.factory('funcaoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;


    var funcaoServiceFactory = {};

    funcaoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Funcao/', data).success(function (response) {
            return response;
        });
    };


    funcaoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Funcao').success(function (response) {
            return response;
        });
    };


    funcaoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/Funcao/' + data.Codigo).success(function (response) {
            return response;
        });
    };


    funcaoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Funcao/' + data.Codigo).success(function (response) {
            return response;
        });
    };


    funcaoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/Funcao/' + data.Codigo, data)
    };


    return funcaoServiceFactory;
}]);
