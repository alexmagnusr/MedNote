'use strict';
Globalsys.controller('loginController', ['$scope', '$location', 'authService', 'notificacoesService', 'AUTHSETTINGS', '$timeout', function ($scope, $location, authService, notificacoesService, AUTHSETTINGS, $timeout) {

    $scope.loginData = {
        userName: "",
        password: "",
        useRefreshTokens: false,
        activeDirectory: false
    };
  

    $scope.message = "";


    $scope.login = function () {
        addLoader();
        authService.login($scope.loginData).then(function (response) {
            removeLoader();
           // notificacoesService.initialize();
            $location.path('/app/singleview');
        },
            function (err) {
                removeLoader();
                if (err == null) {
                    sweetAlert("Atenção", "login ou senha e estão invalidos" , "warning");
                } else {
                    sweetAlert("Atenção", err.error_description, "warning");
                }
               
            });
    };
    $scope.logOut = function () {
        addLoader();
        $timeout(function () {
            removeLoader();
            authService.logOut();
            //notificacoesService.finalizar();
            if (!authService.authentication.isAuth) {
                $location.path('/page/login');
            } else {
                if ($location.$$path == "/page/login")
                    $location.path('/app/singleview');
            }
        },100);
    }
    $scope.authExternalProvider = function (provider) {

        var redirectUri = location.protocol + '//' + location.host + '/authcomplete.html';

        var externalProviderUrl = AUTHSETTINGS.APISERVICEBASEURI + "api/Account/ExternalLogin?provider=" + provider
            + "&response_type=token&client_id=" + AUTHSETTINGS.CLIENTID
                                                                    + "&redirect_uri=" + redirectUri;
        window.$windowScope = $scope;

        var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
    };

    $scope.authCompletedCB = function (fragment) {

        $scope.$apply(function () {

            if (fragment.haslocalaccount == 'False') {

                authService.logOut();

                authService.externalAuthData = {
                    provider: fragment.provider,
                    userName: fragment.external_user_name,
                    externalAccessToken: fragment.external_access_token
                };

                $location.path('/associate');

            }
            else {
                //Obtain access token and redirect to orders
                var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                authService.obtainAccessToken(externalData).then(function (response) {

                    $location.path('/orders');

                },
             function (err) {
                 $scope.message = err.error_description;
             });
            }

        });
    }
}]);
