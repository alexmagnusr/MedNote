'use strict';
Globalsys.factory('usuarioService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var usuarioServiceFactory = {};
    usuarioServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Usuario/Cadastro/', data).success(function (response) {
            return response;
        });
    };
    usuarioServiceFactory.recuperarSenha = function (data) {
        return $http.get(serviceBase + 'api/Usuario/RecuperarSenha?email=' + data.email).success(function (response) {
            return response;
        });
    };
    usuarioServiceFactory.consultar = function (data) {
        
        return $http.get(serviceBase + 'api/Usuario/Consultar?codigo=' + data).success(function (response) {
            return response;
        });
    };
    

    usuarioServiceFactory.consultarComFiltroDeTipoDeColaborador = function (data) {
        return $http.get(serviceBase + 'api/Usuario/ConsultarUsuarioPorTipoColaborador/' + data).success(function (response) {
            return response;
        });
    };

    usuarioServiceFactory.deletar = function (data) {
        return $http.post(serviceBase + 'api/Usuario/Delete?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    usuarioServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Usuario?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    usuarioServiceFactory.obterNomeDoUsuarioLogado = function (data) {
        return $http.get(serviceBase + 'api/Usuario/ObterNomeDoUsuarioLogado').success(function (response) {
            return response;
        });
    };

    usuarioServiceFactory.atualizar = function (data) {
        return $http.post(serviceBase + 'api/Usuario/Atualiza?id=' + data.Codigo, data);
    };

    usuarioServiceFactory.detalhar = function (data) {
        return $http.get(serviceBase + 'api/Usuario/Detalhe/' + data).success(function (response) {
            return response;
        });
    };

    usuarioServiceFactory.trocarSenha = function (data) {
        return $http.post(serviceBase + 'api/Usuario/TrocarSenha/', data).success(function (response) {
            return response;
        });
    };

    usuarioServiceFactory.obterUsuarioLogado = function (data) {
        return $http.get(serviceBase + 'api/Usuario/ObterUsuarioLogado/').success(function (response) {
            return response;
        });
    };

    usuarioServiceFactory.meusDados = function (data) {
        return $http.get(serviceBase + 'api/Usuario/MeusDados/').success(function (response) {
            return response;
        });
    };

    usuarioServiceFactory.ObterClienteUsuario = function (data) {
        return $http.get(serviceBase + 'api/Usuario/cliente').success(function (response) {
            return response;
        });
    };

    return usuarioServiceFactory;

}]);