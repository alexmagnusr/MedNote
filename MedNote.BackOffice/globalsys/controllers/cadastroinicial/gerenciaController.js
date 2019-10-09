'use strict';
Globalsys.controller('gerenciaController', ['$scope', 'gerenciaService', 'clienteService', '$uibModal', '$timeout', function ($scope, gerenciaService, clienteService, $uibModal, $timeout) {
    $scope.gerencias = [];
    $scope.gerencia = {};
    $scope.Clientes = [];
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));
    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("departamentos.xlsx",{headers:true}) FROM ?', [$scope.gerencias]);
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
        $scope.gerencia = {};
        $scope.tituloModal = "Departamento - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalGerencia',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.gerencia = {};
        });
    }

    $scope.salvar = function () {
        if ($scope.gerencia.Codigo == undefined) {
            addLoader();
            gerenciaService.cadastrar($scope.gerencia).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.gerencias, response.data);
                    $scope.gerencia = {};
                    $scope.Clientes = [];
                    $scope.$modalInstance.dismiss('cancel');

                }
            }, function (error) {

            });
        } else {
            addLoader();
            gerenciaService.atualizar($scope.gerencia).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.gerencias, response.data);
                    $scope.gerencia = {};
                    $scope.Clientes = [];
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }

    $scope.editar = function (data) {
        //$scope.loadCliente();
        addLoader();
        gerenciaService.editar(data).then(function (response) {
            removeLoader();
            $scope.gerencia = response.data;
            $scope.Clientes = response.data.ListaCliente;
            $scope.gerencia.Cliente = getItem(response.data.ListaCliente, $scope.gerencia.CodigoCliente);

            $scope.tituloModal = "Departamento - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalGerencia',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.gerencia = {};
                $scope.Clientes = [];
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
                gerenciaService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.gerencias, response.data);
                        swal("", response.data.Message, "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

    consultar();
    function consultar() {
        addLoader();
        gerenciaService.consultar().then(function (response) {
            $scope.gerencias = response.data;
            removeLoader()
        });
    }

    $scope.loadCliente = function () {
        if ($scope.Clientes.length <= 0) {
            clienteService.consultar().then(function (response) {
                $scope.Clientes = response.data;
            });
        }
       
    }

    function getItem(lista, item) {
        var selecionado = {};
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].Codigo == item)
                selecionado = lista[i];
        }
        return selecionado;
    }

}]);