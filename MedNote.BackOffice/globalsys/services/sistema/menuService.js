'use strict';
Globalsys.factory('menuService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {
    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;
    var menuServiceFactory = {};
    menuServiceFactory.carregarMenu = function (data) {
        return $http.get(serviceBase + 'api/Menu?codigo=' + data).success(function (response) {
            return response;
        });
    };


    return menuServiceFactory;

}]);