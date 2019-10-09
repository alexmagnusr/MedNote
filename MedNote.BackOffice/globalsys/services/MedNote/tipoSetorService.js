'use strict';
Globalsys.factory('tipoSetorService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var tipoSetorServiceFactory = {};

    tipoSetorServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/tipoSetor/', data).success(function (response) {
            return response;
        });
    };

    tipoSetorServiceFactory.consultar = function (data) {
        
        return $http.get(serviceBase + 'api/tipoSetor/Consultar?inativos='+ data).success(function (response) {
            return response;
        });
    };

    tipoSetorServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/tipoSetor?id=' + data.Codigo);
    };

    tipoSetorServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/tipoSetor/' + data.Codigo, data);
    };

    tipoSetorServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/tipoSetor/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    return tipoSetorServiceFactory;

}]);