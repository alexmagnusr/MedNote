'use strict';
Globalsys.factory('acaoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var acaoServiceFactory = {};

    acaoServiceFactory.cadastrar = function (data) {
        
        return $http.post(serviceBase + 'api/Acao/Cadastro/', data).success(function(response) {
            return response;
        });
    };

    acaoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Acao').success(function (response) {
            return response;
        });
    };

    acaoServiceFactory.ConsultarAcoesPorPagina = function (data) {
        return $http.get(serviceBase + 'api/Acao/ConsultarAcoesPorPagina/' + data).success(function (response) {
            return response;
        });
    };
    

    acaoServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/Acao/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    acaoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Acao/' + data.Codigo);
    };


    acaoServiceFactory.atualizar = function (data) {
        return $http.post(serviceBase + 'api/acao/Atualiza?id=' + data.Codigo, data);
    };

    return acaoServiceFactory;

}]);