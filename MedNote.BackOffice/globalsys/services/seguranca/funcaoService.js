'use strict';
Globalsys.factory('funcaoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var funcaoServiceFactory = {};

    funcaoServiceFactory.cadastrar = function (data) {
        
        return $http.post(serviceBase + 'api/Funcao/Cadastro/', data).success(function(response) {
            return response;
        });
    };

    funcaoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Funcao').success(function (response) {
            return response;
        });
    };

    funcaoServiceFactory.ConsultarPorTipo = function (data) {
        return $http.get(serviceBase + 'api/Funcao/ConsultarPorTipo/' + data).success(function (response) {
            return response;
        });
    };

    funcaoServiceFactory.ConsultarCanal = function () {
        return $http.get(serviceBase + 'api/Funcao/ConsultarCanal/').success(function (response) {
            return response;
        });
    };

    funcaoServiceFactory.ConsultarTipo = function () {
        return $http.get(serviceBase + 'api/Funcao/ConsultarTipo/').success(function (response) {
            return response;
        });
    };

    funcaoServiceFactory.ConsultarPaginasPorModulo = function (data) {
        return $http.get(serviceBase + 'api/Funcao/ConsultarPaginasPorModulo/' + data ).success(function (response) {
            return response;
        });
    };
    

    funcaoServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/Funcao/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    funcaoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Funcao?id=' + data.Codigo);
    };


    funcaoServiceFactory.atualizar = function (data) {
        return $http.post(serviceBase + 'api/Funcao/Atualiza?id=' + data.Codigo, data);
    };

    return funcaoServiceFactory;

}]);