'use strict';
Globalsys.factory('classeFinanceiraService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var classeFinanceiraServiceFactory = {};
    classeFinanceiraServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/ClasseFinanceira/', data).success(function (response) {
            return response;
        });
    };

    classeFinanceiraServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/ClasseFinanceira').success(function (response) {
            return response;
        });
    };


    classeFinanceiraServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/ClasseFinanceira/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    classeFinanceiraServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/ClasseFinanceira?id=' + data.Codigo).success(function (response) {
            return response;
        });;
    };


    classeFinanceiraServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/ClasseFinanceira/' + data.Codigo, data)
    };

    return classeFinanceiraServiceFactory;

}]);