'use strict';
Globalsys.factory('permissoesUsuariosService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var permissoesUsuariosServiceFactory = {};

    permissoesUsuariosServiceFactory.cadastrar = function (novosAcoesDoUsuario, usuario) {
        var Indata = { 'usuario': usuario, 'acoes': novosAcoesDoUsuario };
        return $http.post(serviceBase + 'api/PermissoesUsuarios/Cadastro/', Indata);
    };

    permissoesUsuariosServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/PermissoesUsuarios?idUsuario=' + data).success(function (response) {
            return response;
        });
    };

    permissoesUsuariosServiceFactory.deletar = function (removerPermissoesUsuarioDoUsuario, usuario) {
        var Indata = { 'usuario': usuario, 'acoes': removerPermissoesUsuarioDoUsuario };
        return $http.post(serviceBase + 'api/PermissoesUsuarios/Delete/', Indata);
    };

    permissoesUsuariosServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/PermissoesUsuarios/' + data.Codigo);
    };


    permissoesUsuariosServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/PermissoesUsuarios/' + data.Codigo, data)
    };

    return permissoesUsuariosServiceFactory;

}]);