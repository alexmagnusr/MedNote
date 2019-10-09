'use strict';
Globalsys.factory('especialidadeService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var especialidadeServiceFactory = {};

    especialidadeServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/especialidade/Cadastro/', data).success(function (response) {
            return response;
        });
    };

    especialidadeServiceFactory.consultar = function (data) {
        
        return $http.get(serviceBase + 'api/especialidade/Consultar?codigo='+ data).success(function (response) {
            return response;
        });
    };

    especialidadeServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/especialidade?id=' + data.Codigo);
    };

    especialidadeServiceFactory.atualizar = function (data) {
        return $http.post(serviceBase + 'api/especialidade/Atualiza?id=' + data.Codigo, data);
    };

    especialidadeServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/especialidade/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    return especialidadeServiceFactory;

}]);