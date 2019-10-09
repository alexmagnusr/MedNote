'use strict';
Globalsys.factory('tipoEquipamentoContratoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {
    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var tipoEquipamentoContratoServiceFactory = {};

    tipoEquipamentoContratoServiceFactory.cadastrar = function (cdContrato, data) {
        return $http.post(serviceBase + 'api/TipoEquipamentoContrato?cdContrato=' + cdContrato, data).success(function (response) {
            return response;
        });
    };

    //tipoEquipamentoContratoServiceFactory.atualizar = function (data) {
    //    return $http.put(serviceBase + 'api/TipoEquipamentoContrato/', data)
    //};

    tipoEquipamentoContratoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/TipoEquipamentoContrato/' + data).success(function (response) {
            return response;
        });
    };


    return tipoEquipamentoContratoServiceFactory;
}]);