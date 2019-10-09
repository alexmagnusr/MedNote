'use strict';
Globalsys.controller('implementoController', ['$scope', 'implementoService', '$uibModal', function ($scope, implementoService, $uibModal) {
    $scope.registros = [];
    $scope.registro = {};
    $scope.tituloModal = "";
    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("implemento.xlsx",{headers:true}) FROM ?', [$scope.registros]);
    };
    
    $scope.salvar = function () {
        if ($scope.registro.Codigo == undefined) {
            addLoader();
            implementoService.cadastrar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.registros, response.data);
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');

                }
            }, function (error) {

            });
        } else {
            addLoader();
            implementoService.atualizar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.registros, response.data);
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }

    $scope.consultar = function () {
        addLoader();
        implementoService.consultar().then(function (response) {
            $scope.registros = response.data;
            removeLoader()
        });
    }

    $scope.abrirModal = function () {
        $scope.tituloModal = "Tipo Implemento - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalTipoImplemento',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.registro = {};
        });
    }
    $scope.deletar = function (data) {
        swal({
            title: "Atenção",
            text: "Você tem certeza que gostaria de remover este registro?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Sim!",
            cancelButtonText: "Não!",
            closeOnConfirm: true,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                addLoader();
               implementoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.registros, response.data);
                    }
                });

            }
        });
    }
    $scope.editar = function (data) {
        addLoader();
       implementoService.editar(data).then(function (response) {
            removeLoader();
            $scope.registro = response.data;
            $scope.tituloModal = "Tipo Implemento - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalTipoImplemento',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.registro = {};
            });
        });
    }
 
}]);
