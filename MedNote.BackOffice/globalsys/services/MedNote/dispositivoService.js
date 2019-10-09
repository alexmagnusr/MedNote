'use strict';
Globalsys.factory('dispositivoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var dispositivoServiceFactory = {};

    dispositivoServiceFactory.implantar = function (data) {
        return $http.post(serviceBase + 'api/siglaDispositivo/Implantar', data).success(function (response) {
            return response;
        });
    };
    
    dispositivoServiceFactory.editar = function (data) {
        debugger
        return $http.get(serviceBase + 'api/siglaDispositivo/Editar?id=' + data.Codigo);
    };

    dispositivoServiceFactory.consultardispositivointernacao = function (data) {
        
        return $http.get(serviceBase + 'api/siglaDispositivo/ConsultarDispositivoInternacao?id=' + data);
    };

    dispositivoServiceFactory.consultar = function (data) {
        debugger
        return $http.get(serviceBase + 'api/siglaDispositivo/Consultar?id=' + data.Codigo);
    };

    dispositivoServiceFactory.editardispositivointernacao = function (data) {
        debugger
        return $http.get(serviceBase + 'api/siglaDispositivo/EditarDispositivoInternacao?id=' + data.Codigo);
    };

    dispositivoServiceFactory.atualizardispositivointernacao = function (codigo, dispositivo) {        
        var Indata = { 'codigo': codigo, 'dispositivo': dispositivo };
        return $http.post(serviceBase + 'api/siglaDispositivo/AtualizarDispositivoInternacao/', Indata);
    };

    dispositivoServiceFactory.deletardispositivointernacao = function (data) {
        debugger
        return $http.post(serviceBase + 'api/siglaDispositivo/DeletarDispositivoInternacao?id=' + data.Codigo).success(function (response) {
            return response;
        });
    };

    dispositivoServiceFactory.consultarlateralidade = function (data) {
        return $http.get(serviceBase + 'api/siglaDispositivo/ConsultarLateralidade').success(function (response) {
            return response;
        });
    };

    dispositivoServiceFactory.consultarcategoria = function (data) {
        return $http.get(serviceBase + 'api/siglaDispositivo/ConsultarCategoria').success(function (response) {
            return response;
        });
    };

    dispositivoServiceFactory.consultartipo = function (data) {
        debugger
        return $http.get(serviceBase + 'api/siglaDispositivo/ConsultarTipo?categoria=' + data);
    };

    dispositivoServiceFactory.consultarsitio = function (data) {
        return $http.get(serviceBase + 'api/siglaDispositivo/ConsultarSitio?tipo=' + data);
    };


    return dispositivoServiceFactory;

}]);