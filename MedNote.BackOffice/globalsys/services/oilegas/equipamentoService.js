'use strict';
Globalsys.factory('equipamentoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var equipamentoServiceFactory = {};

    equipamentoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Equipamento/', data).success(function (response) {
            return response;
        });
    };

    equipamentoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Equipamento').success(function (response) {
            return response;
        });
    };

    equipamentoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Equipamento?id=' + data.Codigo);
    };

    equipamentoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/Equipamento/' + data.Codigo, data)
    };

    equipamentoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/Equipamento/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    return equipamentoServiceFactory;

}]);