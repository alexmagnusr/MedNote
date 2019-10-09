'use strict';
Globalsys.factory('parametrosBaseService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var parametrosBaseServiceFactory = {};

    parametrosBaseServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/parametrosBase/', data).success(function (response) {
            return response;
        });
    };

    parametrosBaseServiceFactory.consultar = function (codigo, tipo, descricao = "") {
        var url = `api/parametrosBase/Consultar?codigo=${codigo}&tipo=${tipo}&descricao=${descricao}`;

        return $http.get(serviceBase + url).success(function (response) {
            return response;
        });
    };

    parametrosBaseServiceFactory.ObterTipos = function (data) {
        return $http.get(serviceBase + 'api/parametrosBase/tipos');
    };

    parametrosBaseServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/parametrosBase?id=' + data.Codigo);
    };

    parametrosBaseServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/parametrosBase/' + data.Codigo, data);
    };

    parametrosBaseServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/parametrosBase/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    return parametrosBaseServiceFactory;
}]);