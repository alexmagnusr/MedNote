'use strict';
Globalsys.controller('produtosController', ['$scope', 'produtosService', '$uibModal', function ($scope, produtosService, $uibModal) {
    $scope.produtos = [];
    $scope.produto = {};
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
   
    $scope.salvar = function () {
        if ($scope.produto.Id == undefined) {
            addLoader();
            produtosService.cadastrar($scope.produto).then(function (response) {
                removeLoader();
                if (response.data.Success) {
                    consultar();
                    $scope.produto = {};
                    sweetAlert("", response.data.Message, "success");
                    $scope.$modalInstance.dismiss('cancel');
                } else {
                    sweetAlert("Atenção", response.data.Message, "error");
                }

            }, function (error) {

            });
        } else {
            addLoader();
            produtosService.atualizar($scope.produto).then(function (response) {
                removeLoader();
                if (response.data.Success) {
                    consultar();
                    $scope.produto = {};
                    sweetAlert("", response.data.Message, "success");
                    $scope.$modalInstance.dismiss('cancel');
                } else {
                    sweetAlert("Atenção", response.data.Message, "error");
                }

            }, function (error) {

            });
        }
    }
    function consultar() {
        addLoader();
        produtosService.consultar().then(function (response) {
            $scope.produtos = response.data;
            removeLoader()
        });
    }
    $scope.consultarDados = function () {
        consultar();
    }
   
    $scope.deletar = function (data) {
        swal({
            title: "Atenção",
            text: "Você tem certeza que gostaria de remover este registro?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Sim.",
            cancelButtonText: "Não.",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                addLoader();
                produtosService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data.Success) {
                        consultar();
                        sweetAlert("", response.data.Message, "success");
                        swal("", response.data.Message, "success");
                    } else {
                        sweetAlert("Atenção", response.data.Message, "error");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

    $scope.editar = function (data) {
        addLoader();
        produtosService.editar(data).then(function (response) {
            removeLoader();
            $scope.produto = response.data;
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalProduto.html',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.produto = {};
            });
        });
    }
    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };
    function addLoader() {
        $('body').addClass('loading').loader('show', {
            overlay: true
        });
    }
    function removeLoader() {
        if ($('body').hasClass('loading')) {
            $('body').removeClass('loading').loader('hide');
        }
    }

}]);