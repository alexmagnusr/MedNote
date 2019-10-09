'use strict';
Globalsys.factory('gestaodassolicitacoesService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;


    var gestaodassolicitacoesServiceFactory = {};

    gestaodassolicitacoesServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/GestaoDasSolicitacoes/', data).success(function (response) {
            return response;
        });
    };
    gestaodassolicitacoesServiceFactory.editarAlocacao = function (data) {
        return $http.post(serviceBase + 'api/GestaoDasSolicitacoes/EditarAlocacao/', data).success(function (response) {
            return response;
        });
    };


    gestaodassolicitacoesServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/GestaoDasSolicitacoes', { params: { "data": data } }).success(function (response) {
            return response;
        });
    };


    gestaodassolicitacoesServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/GestaoDasSolicitacoes/' + data.Codigo).success(function (response) {
            return response;
        });
    };


    gestaodassolicitacoesServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/GestaoDasSolicitacoes?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };


    gestaodassolicitacoesServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/GestaoDasSolicitacoes/' + data.Codigo, data)
    };

    gestaodassolicitacoesServiceFactory.tipoEquipamentoPorSolicitacoes = function (data) {
        return $http.get(serviceBase + 'api/GestaoDasSolicitacoes/TipoEquipamentoPorSolicitacoes', { params: { "data": data } }).success(function (response) {
            return response;
        });
    };

    gestaodassolicitacoesServiceFactory.consultarMotoristas = function (data) {
        return $http.get(serviceBase + 'api/GestaoDasSolicitacoes/ConsultarMotoristas').success(function (response) {
            return response;
        });
    };
    gestaodassolicitacoesServiceFactory.consultarEquipamentoPorTipo = function (data) {
        return $http.get(serviceBase + 'api/GestaoDasSolicitacoes/ConsultarEquipamentoPorTipo?idTipoEquipamento=' + data).success(function (response) {
            return response;
        });
    };

    gestaodassolicitacoesServiceFactory.carregarComponentesModal = function (value) {
        return $http.post(serviceBase + 'api/GestaoDasSolicitacoes/CarregarComponentesModal/', value).success(function (response) {
            return response;
        });
    };

    gestaodassolicitacoesServiceFactory.ImprimirPOS = function (id) {
        return $http({
            url: AUTHSETTINGS.APISERVICEBASEURI + 'api/GestaoDasSolicitacoes/ImprimirPOS/' + id,
            method: 'GET',
            params: {},
            headers: {
                'Content-type': 'application/pdf'
            },
            responseType: 'arraybuffer'
        }).success(function (data, status, headers, config) {
            return data;
        });
    };

    return gestaodassolicitacoesServiceFactory;
}]);
