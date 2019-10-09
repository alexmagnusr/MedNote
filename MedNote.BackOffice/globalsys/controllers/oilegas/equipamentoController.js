'use strict';
Globalsys.controller('equipamentoController', ['$scope', 'equipamentoService', 'tipoEquipamentoService', '$uibModal', '$timeout', function ($scope, equipamentoService, tipoEquipamentoService, $uibModal, $timeout) {
    $scope.equipamentos = [];
    $scope.equipamento = {};
    $scope.TipoEquipamentos = [];
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("equipamentos.xlsx",{headers:true}) FROM ?', [$scope.equipamentos]);
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
        $scope.equipamento = {};
        $scope.tituloModal = "Equipamento - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalEquipamento',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.equipamento = {};
        });
    }

    $scope.salvar = function () {
        $scope.equipamento.TipoEquipamento.Imagem = null;
        if ($scope.equipamento.Codigo == undefined) {
            addLoader();
            equipamentoService.cadastrar($scope.equipamento).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.equipamentos, response.data);
                    $scope.equipamento = {};
                    $scope.TipoEquipamentos = [];
                    $scope.$modalInstance.dismiss('cancel');

                }
            }, function (error) {

            });
        } else {
            addLoader();
            equipamentoService.atualizar($scope.equipamento).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.equipamentos, response.data);
                    $scope.equipamento = {};
                    $scope.TipoEquipamentos = [];
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }

    $scope.editar = function (data) {
        //$scope.loadCliente();
        addLoader();
        equipamentoService.editar(data).then(function (response) {
            removeLoader();
            $scope.equipamento = response.data;
            //$scope.TipoEquipamentos = response.data.ListaTipoEquipamento;
            //$scope.equipamento.TipoEquipamento = getItem(response.data.ListaTipoEquipamento, $scope.equipamento.CodigoEquipamento);

            $scope.tituloModal = "Equipamento - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalEquipamento',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.equipamento = {};
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
                equipamentoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.equipamentos, response.data);
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
        equipamentoService.consultar().then(function (response) {
            $scope.equipamentos = response.data;
            removeLoader();
        });
    }

    $scope.loadTipoEquipamento = function () {
        addLoader();
        tipoEquipamentoService.consultar().then(function (response) {
            $scope.TipoEquipamentos = response.data;
            if ($scope.equipamento.Codigo != undefined) {
                $scope.equipamento.TipoEquipamento = getItem($scope.TipoEquipamentos, $scope.equipamento.CodigoEquipamento);
            }
            removeLoader();
        });
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