'use strict';
Globalsys.factory('permissoesService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var permissoesServiceFactory = {};

    permissoesServiceFactory.cadastrar = function (novosAcoesDoGrupo, grupo) {
        debugger
        var Indata = { 'grupo': grupo, 'acoes': novosAcoesDoGrupo };
        return $http.post(serviceBase + 'api/permissoes/Cadastro/', Indata);
    };

    permissoesServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/permissoes?idGrupo=' + data).success(function (response) {
            return response;
        });
    };

    permissoesServiceFactory.deletar = function (removerPermissoesDoGrupo, grupo) {
        debugger
        var Indata = { 'grupo': grupo, 'acoes': removerPermissoesDoGrupo };
        return $http.post(serviceBase + 'api/permissoes/Delete/', Indata);
    };

    permissoesServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/permissoes/' + data.Codigo);
    };


    permissoesServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/permissoes/' + data.Codigo, data);
    };

    return permissoesServiceFactory;

}]);