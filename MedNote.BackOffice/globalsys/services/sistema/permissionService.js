'use strict';
Globalsys.factory('permissionService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;

    var permissionServiceFactory = {};

    permissionServiceFactory.verify = function (data) {
        return $http.post(serviceBase + 'api/HasPermission/', data).success(function (response) {
            return response;
        });
    };



    return permissionServiceFactory;

}]);