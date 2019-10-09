'use strict';
Globalsys.factory('setorService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var setorServiceFactory = {};

    setorServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/setor/', data).success(function (response) {
            return response;
        });
    };

    setorServiceFactory.consultar = function (data) {
        
        return $http.get(serviceBase + 'api/setor/Consultar?codigo=' + data).success(function (response) {
            
            return response;
        });
    };


    setorServiceFactory.ConsultarPorEstabelecimento = function (data) {
        return $http.get(serviceBase + 'api/setor/ConsultarPorEstabelecimento?codigo=' + data).success(function (response) {

            return response;
        });
    };

    setorServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/setor?id=' + data.Codigo);
    };

    setorServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/setor/' + data.Codigo, data);
    };

    setorServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/setor/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    return setorServiceFactory;

}]);