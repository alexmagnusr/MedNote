'use strict';
Globalsys.factory('regimeService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;


var regimeServiceFactory = {};

regimeServiceFactory.cadastrar = function (data) {
    return $http.post(serviceBase + 'api/Regime/', data).success(function (response) {
        return response;
    });
};


regimeServiceFactory.consultar = function (data) {
    return $http.get(serviceBase + 'api/Regime').success(function (response) {
        return response;
    });
};


regimeServiceFactory.deletar = function (data) {
    return $http.delete(serviceBase + 'api/Regime/' + data.Codigo).success(function (response) {
        return response;
    });
};


regimeServiceFactory.editar = function (data) {
    return $http.get(serviceBase + 'api/Regime?id=' + data.Codigo).success(function (response) {
        return response;
    });
};


regimeServiceFactory.atualizar = function (data) {
    return $http.put(serviceBase + 'api/Regime/' + data.Codigo, data)
 };


    return regimeServiceFactory;
}]);
