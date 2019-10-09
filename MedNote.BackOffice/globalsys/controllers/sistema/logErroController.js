'use strict';
Globalsys.controller('logErroController', ['$scope', 'logErroService', '$uibModal', function ($scope, logErroService, $uibModal) {
    $scope.logerros = [];
    $scope.logerro = {};
    $scope.abrirModal = function () {
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalProduto.html',
            scope: $scope

        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.produto = {};
        });
    }


    function consultar() {
        addLoader();
        logErroService.consultar().then(function (response) {
            $scope.logerros = response.data;
            removeLoader()
        });
    }
    $scope.consultarDados = function () {
        consultar();
    }

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };
    $scope.editar = function (data) {
        $scope.logerro = data;
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalLogErro',
            scope: $scope

        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.produto = {};
        });

    }


}]);