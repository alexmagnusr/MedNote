'use strict';
Globalsys.factory('projetoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var projetoServiceFactory = {};
    projetoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Projeto/', data).success(function (response) {
            return response;
        });
    };

    projetoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Projeto').success(function (response) {
            return response;
        });
    };


    projetoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/Projeto/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    projetoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Projeto?id=' + data.Codigo).success(function (response) {
            return response;
        });;
    };


    projetoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/Projeto/' + data.Codigo, data)
    };

    return projetoServiceFactory;

}]);