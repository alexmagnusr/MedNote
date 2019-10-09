'use strict';
Globalsys.factory('estabelecimentoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var estabelecimentoServiceFactory = {};

    estabelecimentoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/estabelecimentoSaude/Cadastro/', data).success(function (response) {
            return response;
        });
    };

    estabelecimentoServiceFactory.consultar = function (data) {
        
        return $http.get(serviceBase + 'api/estabelecimentoSaude/Consultar?codigo=' + data).success(function (response) {
            return response;
        });
    };

    estabelecimentoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/estabelecimentoSaude?id=' + data.Codigo);
    };

    estabelecimentoServiceFactory.atualizar = function (data) {
        return $http.post(serviceBase + 'api/estabelecimentoSaude/Atualiza?id=' + data.Codigo, data);
    };

    estabelecimentoServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/estabelecimentoSaude/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    estabelecimentoServiceFactory.ConsultarEstabelecimentoPorCliente = function (data) {
        return $http.get(serviceBase + 'api/estabelecimentoSaude/ConsultarEstabelecimentoPorCliente/' + data).success(function (response) {
            return response;
        });
    };

    return estabelecimentoServiceFactory;

}]);