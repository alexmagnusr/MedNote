'use strict';
Globalsys.factory('areasService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var areasServiceFactory = {};
    areasServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Areas/', data).success(function (response) {
            return response;
        });
    };

    areasServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Areas').success(function (response) {
            return response;
        });
    };

    areasServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/Areas/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    areasServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Areas?id=' + data.Codigo);
    };


    areasServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/Areas/' + data.Codigo, data)
    };

    areasServiceFactory.consultarGrauDeUrgenciaArea = function (data) {
        return $http.get(serviceBase + 'api/Areas/ConsultarGrauDeUrgenciaArea/').success(function (response) {
            return response;
        });
    };
    return areasServiceFactory;

}]);