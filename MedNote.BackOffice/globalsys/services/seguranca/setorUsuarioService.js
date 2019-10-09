'use strict';
Globalsys.factory('setorUsuarioService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var setorUsuarioServiceFactory = {};

    setorUsuarioServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/setorUsuario/Cadastro/', data).success(function (response) {
            return response;
        });
    };

    setorUsuarioServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/setorUsuario/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    setorUsuarioServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/setorUsuario/Consultar?codigo=' + data).success(function (response) {
            return response;
        });
    };

    return setorUsuarioServiceFactory;

}]);