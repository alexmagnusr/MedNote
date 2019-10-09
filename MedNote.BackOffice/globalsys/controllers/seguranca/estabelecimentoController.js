'use strict';
Globalsys.controller('estabelecimentoController', ['$scope', 'estabelecimentoService', 'clienteService', 'dadosGeraisService', '$uibModal', '$timeout', 'utilService', function ($scope, estabelecimentoService, clienteService, dadosGeraisService, $uibModal, $timeout, utilService) {
    $scope.estabelecimentos = [];
    $scope.estabelecimento = {};
    $scope.clientes = [];
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    $scope.loadCliente = function () {
        
        clienteService.consultar().then(function (response) {
            $scope.clientes = response.data;
        });
    }


    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("estabelecimentos.xlsx",{headers:true}) FROM ?', [$scope.estabelecimentos]);
    };

    $scope.datePickerSetting = {
        dateOptions: {
            formatYear: 'yy',
            startingDay: 1
        },
        format: 'dd/MM/yyyy',
        opened: false
    };
    
    $scope.abrirModal = function () {
        $scope.estabelecimento = {};
        
        $scope.estabelecimento.Cliente = $scope.moduloID.Codigo;
        $scope.tituloModal = "Estabelecimento - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalEstabelecimento',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.estabelecimento = {};
        });
    }

    $scope.salvar = function () {
        
        $scope.estabelecimento.Cliente = { Codigo: $scope.estabelecimento.Cliente };
        if ($scope.estabelecimento.Codigo == undefined) {
            addLoader();
            estabelecimentoService.cadastrar($scope.estabelecimento).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.estabelecimentos, response.data);
                    $scope.estabelecimento = {};
                    $scope.$modalInstance.dismiss('cancel');

                }
            }, function (error) {

            });
        } else {
            addLoader();
            estabelecimentoService.atualizar($scope.estabelecimento).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.estabelecimentos, response.data);
                    $scope.estabelecimento = {};
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }

    consultar();
    function consultar() {
        addLoader();
        if ($scope.moduloID != null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }

        estabelecimentoService.consultar($scope.codCliente).then(function (response) {
            $scope.estabelecimentos = response.data;
            removeLoader();
        });
    }
   
    $scope.editar = function (data) {
        addLoader();
        
        estabelecimentoService.editar(data).then(function (response) {
            removeLoader();
            $scope.estabelecimento = response.data;
            $scope.estabelecimento.Cliente = $scope.moduloID.Codigo;
            $scope.tituloModal = "Estabelecimento - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalEstabelecimento',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.estabelecimento = {};
            });

        });
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
                estabelecimentoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.estabelecimentos, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

}]);