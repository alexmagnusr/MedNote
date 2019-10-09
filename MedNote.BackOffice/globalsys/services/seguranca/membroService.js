'use strict';
Globalsys.factory('membroService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var membroServiceFactory = {};

    membroServiceFactory.cadastrar = function (novosMembrosDoGrupo, grupo) {
        var Indata = { 'grupo': grupo, 'membros': novosMembrosDoGrupo };
        return $http.post(serviceBase + 'api/membro/Cadastro/', Indata);
    };

    membroServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/membro?idGrupo=' + data).success(function (response) {
            return response;
        });
    };

    membroServiceFactory.deletar = function (removerMembrosDoGrupo, grupo) {
        var Indata = { 'grupo': grupo, 'membros': removerMembrosDoGrupo };
        return $http.post(serviceBase + 'api/membro/Delete/', Indata);
    };

    membroServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/membro?id=' + data.Codigo);
    };

    membroServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/membro/' + data.Codigo, data)
    };

    return membroServiceFactory;

}]);