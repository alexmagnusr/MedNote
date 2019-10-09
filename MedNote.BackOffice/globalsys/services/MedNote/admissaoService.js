'use strict';
Globalsys.factory('admissaoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var admissaoServiceFactory = {};

    admissaoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/admissao/', data).success(function (response) {
            return response;
        });
    };

    admissaoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/admissao/Consultar?codigo=' + data).success(function (response) {
            return response;
        });
    };

    admissaoServiceFactory.editar = function (codigo) {
        return $http.get(serviceBase + 'api/admissao?id=' + codigo);
    };

    admissaoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/admissao/' + data.Codigo, data);
    };

    admissaoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/admissao/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    admissaoServiceFactory.consultarPacientes = function (nome, codigoSetor) {
        return $http.get(serviceBase + 'api/admissao/ConsultarPacientes?nome=' + nome + '&codigoSetor=' + codigoSetor).success(function (response) {
            return response;
        });
    };

    admissaoServiceFactory.consultarConvenios = function (data) {
        return $http.get(serviceBase + 'api/admissao/ConsultarConvenios?codigo=' + data).success(function (response) {
            return response;
        });
    }; 

    admissaoServiceFactory.consultarPacienteInternacao = function (data) {
        return $http.get(serviceBase + 'api/admissao/PacienteInternacao?id=' + data).success(function (response) {
            return response;
        });
    };

    return admissaoServiceFactory;

}]);