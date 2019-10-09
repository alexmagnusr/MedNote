'use strict';
Globalsys.factory('parametrizacaoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var parametrizacaoServiceFactory = {};
    parametrizacaoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Parametrizacao/', data).success(function (response) {
            return response;
        });
    };

    parametrizacaoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Parametrizacao').success(function (response) {
            return response;
        });
    };


    parametrizacaoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/Parametrizacao/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    parametrizacaoServiceFactory.editar = function (Codigo) {
        return $http.get(serviceBase + 'api/Parametrizacao?id=' + Codigo).success(function (response) {
            return response;
        });;
    };


    parametrizacaoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/Parametrizacao/1', data)
    };

    return parametrizacaoServiceFactory;

}]);