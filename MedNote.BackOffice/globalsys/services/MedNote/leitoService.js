'use strict';
Globalsys.factory('leitoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var leitoServiceFactory = {};

    leitoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/leito/', data).success(function (response) {
            return response;
        });
    };

    leitoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/leito/Consultar?codigo=' + data).success(function (response) {
            return response;
        });
    };

    leitoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/leito?id=' + data.Codigo);
    };

    leitoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/leito/' + data.Codigo, data);
    };

    leitoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/leito/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    return leitoServiceFactory;

}]);