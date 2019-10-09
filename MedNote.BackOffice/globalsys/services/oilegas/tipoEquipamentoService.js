'use strict';
Globalsys.factory('tipoEquipamentoService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var tipoEquipamentoServiceFactory = {};


    tipoEquipamentoServiceFactory.cadastrar = function (data, imagem) {
        var fd = new FormData();
        fd.append('imagem', imagem);
        var record = JSON.stringify(data);
        fd.append("data", record);
        return $http.post(serviceBase + 'api/TipoEquipamento/', fd, {
            withCredentials: false,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        }).success(function (response) {
            return response;
        });
    };

    tipoEquipamentoServiceFactory.consultar = function (data) {
        return $http.get(serviceBase + 'api/TipoEquipamento').success(function (response) {
            return response;
        });
    };



    tipoEquipamentoServiceFactory.deletar = function (data) {
        return $http.delete(serviceBase + 'api/TipoEquipamento/' + data.Codigo).success(function (response) {
            return response;
        });
    };

    tipoEquipamentoServiceFactory.editar = function (data) {
        return $http.get(serviceBase + 'api/TipoEquipamento?id=' + data.Codigo);
    };


    tipoEquipamentoServiceFactory.atualizar = function (data, imagem) {

        var fd = new FormData();
        fd.append('imagem', imagem);
        var record = JSON.stringify(data);
        fd.append("data", record);
        return $http.put(serviceBase + 'api/TipoEquipamento/', fd, {
            withCredentials: false,
            headers: {
                'Content-Type': undefined
            },
            transformRequest: angular.identity
        }).success(function (response) {
            return response;
        });

        //return $http.put(serviceBase + 'api/TipoEquipamento/' + data.Codigo, data)
    };

    return tipoEquipamentoServiceFactory;

}]);