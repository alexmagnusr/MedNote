'use strict';
Globalsys.factory('notificacoesService', ['$http', '$q', 'localStorageService', 'AUTHSETTINGS', '$rootScope', '$timeout', function ($http, $q, localStorageService, AUTHSETTINGS, $rootScope, $timeout) {
    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;
    var notificationHubProxy = {};

    /*

    notificationHubProxy.finalizar = function () {
        $.connection.hub.stop();
        $.connection.hub.logging = false;

    }

    notificationHubProxy.initialize = function () {
        $.connection.hub.url = serviceBase + "signalr";
        //jQuery.support.cors = true

        $.connection.hub.logging = true;
        var authData = localStorageService.get('authorizationData');
        var token = authData.token;
        $.signalR.ajaxDefaults.headers = { Authorization: "Bearer " + token };

        $timeout(function () { $.connection.hub.start(); }, 1000);
        //$.connection.hub.qs = { 'Bearer': token };

        notificationHubProxy = $.connection.notificationHub;
        notificationHubProxy.client.enviarNotificacao = function (message) {
            $rootScope.$broadcast('exibirNotificacao', message);

        };
        $.connection.hub.start().done(function () {
            console.log("started");
        }).fail(function (result) {
            console.log(result);
        });
    };*/

    notificationHubProxy.consultar = function (data) {
        return $http.get(serviceBase + 'api/Notificacao?visualizado=' + data).success(function (response) {
            return response;
        });
    };
    notificationHubProxy.qntNotificacoes = function (data) {
        return $http.get(serviceBase + 'api/Notificacao/QntNotificacoes?visualizado=' + data).success(function (response) {
            return response;
        });
    };
    notificationHubProxy.marcarComoLido = function (data) {
        return $http.post(serviceBase + 'api/Notificacao/MarcarComoVisualizado?codigo=' + data).success(function (response) {
            return response;
        });
    };
    return notificationHubProxy;
}]);