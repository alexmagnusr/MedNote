'use strict';
Globalsys.factory('grupoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var GrupoServiceFactory = {};

    GrupoServiceFactory.cadastrar = function (data) {

        return $http.post(serviceBase + 'api/Grupo/Cadastro/', data).success(function(response) {
            return response;
        });
    };

    GrupoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Grupo/Consultar?codigo=' + data).success(function (response) {
            return response;
        });
    };

    GrupoServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/Grupo/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    GrupoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Grupo?id=' + data.Codigo);
    };


    GrupoServiceFactory.atualizar = function (data) {
        return $http.post(serviceBase + 'api/Grupo/Atualiza?id=' + data.Codigo, data);
    };

    return GrupoServiceFactory;

}]);