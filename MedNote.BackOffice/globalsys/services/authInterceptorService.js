'use strict';
Globalsys.factory('authInterceptorService', ['$q', '$injector', '$location', 'localStorageService', function ($q, $injector, $location, localStorageService) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {
        config.headers = config.headers || {};

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }

        return config;
    }
    var _response = function (response) {
        if (response.data.Success != undefined && response.data.Success == false) {
            sweetAlert("Atenção", response.data.Message, "warning");
            removeLoader();
            return $q.reject(response);
        }
        if (response.data.error) {
            return $q.reject(response);
        }

        return response;
    }

    var _responseError = function (rejection) {
        if (rejection.status === 401) {
            var authService = $injector.get('authService');
            var authData = localStorageService.get('authorizationData');

            if (authData) {
                if (authData.useRefreshTokens) {
                    $location.path('/refresh');
                    return $q.reject(rejection);
                }
            }
            authService.logOut();
            removeLoader()
           // $location.path('/login');
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;
    authInterceptorServiceFactory.response = _response;

    return authInterceptorServiceFactory;
}]);