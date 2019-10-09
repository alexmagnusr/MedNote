'use strict';
Globalsys.factory('contratoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var contratoServiceFactory = {};

    contratoServiceFactory.cadastrar = function (data) {
        return $http.post(serviceBase + 'api/Contrato/', data).success(function (response) {
            return response;
        });
    };

    contratoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/Contrato').success(function (response) {
            return response;
        });
    };

    contratoServiceFactory.consultarTiposEquipamentoContrato = function (data) {
        return $http.get(serviceBase + 'api/Contrato/ConsultarTiposEquipamentoContrato/' + data).success(function (response) {
            return response;
        });
    };

    contratoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/Contrato?id=' + data.Codigo);
    };

    contratoServiceFactory.atualizar = function (data) {
        return $http.put(serviceBase + 'api/Contrato/' + data.Codigo, data)
    };

    contratoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/Contrato/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    contratoServiceFactory.consultarCriterioMedicao = function (data) {
        return $http.get(serviceBase + 'api/Contrato/ConsultarCriterioMedicao/').success(function (response) {
            return response;
        });
    };

    return contratoServiceFactory;

}]);