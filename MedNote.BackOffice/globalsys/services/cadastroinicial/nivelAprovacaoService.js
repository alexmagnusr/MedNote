'use strict';
Globalsys.factory('nivelAprovacaoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var nivelAprovacaoServiceFactory = {};
    nivelAprovacaoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/NivelAprovacao/', data).success(function (response) {
            return response;
        });
    };

    nivelAprovacaoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/NivelAprovacao').success(function (response) {
            return response;
        });
    };


    nivelAprovacaoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/NivelAprovacao/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    nivelAprovacaoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/NivelAprovacao?id=' + data.Codigo).success(function (response) {
            return response;
        });;
    };


    nivelAprovacaoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/NivelAprovacao/' + data.Codigo, data)
    };

    return nivelAprovacaoServiceFactory;

}]);