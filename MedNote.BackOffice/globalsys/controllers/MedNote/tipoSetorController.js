'use strict';
Globalsys.controller('tipoSetorController', ['$scope', 'tipoSetorService', 'estabelecimentoService', 'clienteService', 'dadosGeraisService', '$uibModal', '$timeout', 'utilService', function ($scope, tipoSetorService, estabelecimentoService, clienteService, dadosGeraisService, $uibModal, $timeout, utilService) {
    $scope.tipoSetores = [];
    $scope.tipoSetor = {};
    $scope.clientes = [];
    //$scope.estabelecimentos = [];
    //$scope.estabelecimento = {};
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    //$scope.loadEstabelecimento = function () {
    //    estabelecimentoService.consultar($scope.moduloID.Codigo).then(function (response) {
    //        $scope.estabelecimentos = response.data;
    //    });
    //}
    

    //$scope.exportData = function () {
    //    alasql('SELECT * INTO XLSX("estabelecimentos.xlsx",{headers:true}) FROM ?', [$scope.estabelecimentos]);
    //};

    $scope.datePickerSetting = {
        dateOptions: {
            formatYear: 'yy',
            startingDay: 1
        },
        format: 'dd/MM/yyyy',
        opened: false
    };
    
    $scope.abrirModal = function () {
        $scope.tipoSetor = {};
        
       
        $scope.tituloModal = "Tipo de Setor - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalTipoSetor',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.tipoSetor = {};
        });
    }

    $scope.salvar = function () {
        
        $scope.tipoSetor.Estabelecimento = { Codigo: $scope.tipoSetor.Estabelecimento };
        if ($scope.tipoSetor.Codigo == undefined) {
            addLoader();
            tipoSetorService.cadastrar($scope.tipoSetor).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.tipoSetores, response.data);
                    $scope.tipoSetor = {};
                    $scope.$modalInstance.dismiss('cancel');

                }
            }, function (error) {

            });
        } else {
            addLoader();
            tipoSetorService.atualizar($scope.tipoSetor).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.tipoSetores, response.data);
                    $scope.tipoSetor = {};
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }

    function consultar() {
        
        addLoader();
        if ($scope.moduloID != null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }

        tipoSetorService.consultar(true).then(function (response) {
            $scope.tipoSetores = response.data;
            removeLoader();
        });
    }

    consultar();

    $scope.editar = function (data) {
        addLoader();

        
        tipoSetorService.editar(data).then(function (response) {
            removeLoader();
            $scope.tipoSetor = response.data;
            $timeout(function () {
                if (response.data.Estabelecimento == null) {
                    $scope.tipoSetor.Estabelecimento = response.data.Estabelecimento;

                }
                    
            });

            $scope.tituloModal = "Tipo de Setor - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalTipoSetor',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.tipoSetor = {};
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
                tipoSetorService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.tipoSetores, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

}]);