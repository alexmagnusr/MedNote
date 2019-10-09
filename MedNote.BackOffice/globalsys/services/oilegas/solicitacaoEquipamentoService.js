'use strict';
Globalsys.factory('solicitacaoEquipamentoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;


    var solicitacaoequipamentoServiceFactory = {};

    solicitacaoequipamentoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/SolicitacaoEquipamento/', data).success(function (response) {
            return response;
        });
    };

    solicitacaoequipamentoServiceFactory.consultarSolicitacoesDoDia = function (data) {
        return $http.get(serviceBase + 'api/SolicitacaoEquipamento/ConsultarSolicitacoesDoDia').success(function (response) {
            return response;
        });
    };




    solicitacaoequipamentoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/SolicitacaoEquipamento').success(function (response) {
            return response;
        });
    };


    solicitacaoequipamentoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/SolicitacaoEquipamento/' + data.Codigo).success(function (response) {
            return response;
        });
    };


    solicitacaoequipamentoServiceFactory.detalhar = function (data) {
        return $http.get(serviceBase + 'api/SolicitacaoEquipamento?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };


    solicitacaoequipamentoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/SolicitacaoEquipamento/' + data.Codigo, data)
    };

    solicitacaoequipamentoServiceFactory.getCssStatus = function (data) {
        var css = "";
        switch (data) {
            case 'Em Aprovação':
            case 'Programada':
                css = "#81d4fa";
                break;
            case 'Em Programação':
                //css = "bg-primary-light";
                css = "#ffd600";
                break;

            case 'Recusada':
            case 'Cancelada':
                //css = "bg-danger-light";
                css = "#ef9a9a";
                break;
            case 'Em Atendimento':
                //css = "bg-success-light";
                css = "#a5d6a7";
                break;
            case 'Refeição - Início':
            case 'Refeição - Fim':
            case 'Descanso - Início':
            case 'Descanso - Fim':
                //css = "bg-yellow-light";
                css = "#ffab91";
                break;
            case 'Finalizada':
                //css = "bg-gray-dark";
                css = "#9e9e9e";
                break;
        }

        return css;
    }


    return solicitacaoequipamentoServiceFactory;
}]);
