'use strict';
Globalsys.factory('tokensManagerService', ['$http', 'AUTHSETTINGS', function ($http, AUTHSETTINGS) {

    var serviceBase = AUTHSETTINGS.APISERVICEBASEURI;
    
    var tokenManagerServiceFactory = {};

    var _getRefreshTokens = function () {

        return $http.get(serviceBase + 'api/refreshtokens').then(function (results) {
            return results;
        });
    };

    var _deleteRefreshTokens = function (tokenid) {

        return $http.delete(serviceBase + 'api/refreshtokens/?tokenid=' + tokenid).then(function (results) {
            return results;
        });
    };

    tokenManagerServiceFactory.deleteRefreshTokens = _deleteRefreshTokens;
    tokenManagerServiceFactory.getRefreshTokens = _getRefreshTokens;

    return tokenManagerServiceFactory;

}]);