'use strict';
Globalsys.controller('auditoriaController', ['$scope', 'auditoriaService', '$uibModal', function ($scope, auditoriaService, $uibModal) {
    $scope.auditorias = [];
    $scope.auditoria = {};
    $scope.tipo = "";
    $scope.$watch('tipo', function (data) {
        if (data != null && data != "") {
            addLoader();
            auditoriaService.consultar(data).then(function (response) {
                $scope.auditorias = [];
                $scope.auditorias = response.data;
                removeLoader()
            });
        }
    });

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("auditorias.xlsx",{headers:true}) FROM ?', [$scope.auditorias]);
    };

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
        auditoriaService.consultar($scope.tipo).then(function (response) {
            $scope.auditorias = response.data;
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
        $scope.auditoria = data;
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalAuditoria',
            scope: $scope

        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.auditoria = {};
        });

    }


}]);