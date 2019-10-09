'use strict';
Globalsys.factory('implementoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;


var implementoServiceFactory = {};

implementoServiceFactory.cadastrar = function (data) {
    return $http.post(serviceBase + 'api/Implemento/', data).success(function (response) {
        return response;
    });
};


implementoServiceFactory.consultar = function (data) {
    return $http.get(serviceBase + 'api/Implemento').success(function (response) {
        return response;
    });
};


implementoServiceFactory.deletar = function (data) {
    return $http.delete(serviceBase + 'api/Implemento/' + data.Codigo).success(function (response) {
        return response;
    });
};


implementoServiceFactory.editar = function (data) {
    return $http.get(serviceBase + 'api/Implemento?id=' + data.Codigo).success(function (response) {
        return response;
    });
};


implementoServiceFactory.atualizar = function (data) {
    return $http.put(serviceBase + 'api/Implemento/' + data.Codigo, data)
 };


    return implementoServiceFactory;
}]);
