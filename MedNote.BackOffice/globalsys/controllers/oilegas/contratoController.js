'use strict';
Globalsys.controller('contratoController', ['$scope', 'contratoService', 'clienteService', 'tipoEquipamentoService', '$uibModal', '$timeout', function ($scope, contratoService, clienteService, tipoEquipamentoService, $uibModal, $timeout) {
    $scope.contratos = [];
    $scope.contrato = {};
    $scope.Clientes = [];
    $scope.CriteriosMedicao = [];
    $scope.TiposEquipamentoDisponiveis = [];
    $scope.TiposEquipamentoAdicionados = [];
    $scope.TiposEquipamentoDisponiveisSelecionados = [];
    $scope.TiposEquipamentoAdicionadosSelecionados = [];
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;

    $scope.novosTiposEquipamentoContrato = [];
    $scope.removerTiposEquipamentoContrato = [];


    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("contratos.xlsx",{headers:true}) FROM ?', [$scope.contratos]);
    };

    $scope.abrirModal = function () {
        $scope.contrato = {};
        $scope.tituloModal = "Contrato - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalContrato',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.contrato = {};
        });
    }

    $scope.abrirModalTipoEquipamento = function (data) {
        $scope.contrato = data;
        contratoService.consultarTiposEquipamentoContrato(data.Codigo).then(function (response) {
            $scope.contrato.TipoEquipamentos = response.data;
        });

        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalTipoEquipamentoContrato',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.contrato = {};
        });

    }

    $scope.loadClientes = function () {
        if ($scope.Clientes.length <= 0) {
            clienteService.consultar().then(function (response) {
                $scope.Clientes = response.data;
            });
        }
    }

    $scope.loadTipoEquipamentos = function () {
        if ($scope.contrato.Codigo == undefined) {
            tipoEquipamentoService.consultar().then(function (response) {
                $scope.TiposEquipamentoDisponiveis = response.data;
            });
        }
    }

    $scope.loadCriteriosMedicao = function () {
        contratoService.consultarCriterioMedicao().then(function (response) {
            $scope.CriteriosMedicao = response.data;
        });
    }

    $scope.selecionandoEquipamentosDisponiveis = function (item) {
        $scope.TiposEquipamentoDisponiveisSelecionados = item;
    }

    $scope.selecionandoEquipamentosJaAdicionados = function (item) {
        $scope.TiposEquipamentoAdicionadosSelecionados = item;
    }

    function getItemTiposEquipamentoDisponiveis(item) {
        var selecionado = {};
        for (var i = 0; i < $scope.TiposEquipamentoDisponiveis.length; i++) {
            if ($scope.TiposEquipamentoDisponiveis[i].Codigo === item.Codigo)
                selecionado = $scope.TiposEquipamentoDisponiveis[i];
        }
        return selecionado;
    }

    function getItemTiposEquipamentoJaAdd(item) {
        var selecionado = {};
        for (var i = 0; i < $scope.TiposEquipamentoAdicionados.length; i++) {
            if ($scope.TiposEquipamentoAdicionados[i].Codigo === item.Codigo)
                selecionado = $scope.TiposEquipamentoAdicionados[i];
        }
        return selecionado;
    }

    $scope.addTiposEquipamentoSelecionados = function () {
        //addLoader();
        for (var i = 0; i < $scope.TiposEquipamentoDisponiveisSelecionados.length; i++) {
            var index = $scope.TiposEquipamentoDisponiveis.indexOf($scope.TiposEquipamentoDisponiveisSelecionados[i]);
            $scope.TiposEquipamentoAdicionados.push($scope.TiposEquipamentoDisponiveisSelecionados[i]);
            $scope.TiposEquipamentoDisponiveis.splice(index, 1);
        }

        $scope.TiposEquipamentoDisponiveisSelecionados = [];
        //removeLoader();
    }

    $scope.removeTiposEquipamentoSelecionados = function () {
        var indexToRemove = [];
        for (var i = 0; i < $scope.TiposEquipamentoAdicionadosSelecionados.length; i++) {
            $scope.TiposEquipamentoDisponiveis.push($scope.TiposEquipamentoAdicionadosSelecionados[i]);
        }

        for (var i = 0; i < $scope.TiposEquipamentoAdicionadosSelecionados.length; i++) {
            var index = $scope.TiposEquipamentoAdicionados.indexOf($scope.TiposEquipamentoAdicionadosSelecionados[i]);
            $scope.TiposEquipamentoAdicionados.splice(index, 1);
        }

        $scope.TiposEquipamentoAdicionadosSelecionados = [];
    }

    $scope.salvar = function () {
        addLoader();

        //$scope.contrato.TipoEquipamentos = $scope.TiposEquipamentoAdicionados;
        if ($scope.contrato.Codigo == undefined) {
            //Salvar
            contratoService.cadastrar($scope.contrato).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.contratos, response.data);
                    $scope.$modalInstance.dismiss('cancel');
                    resetArrays();
                }
            }, function (error) {

            });
        } else {
            //Atualizar
            contratoService.atualizar($scope.contrato).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.contratos, response.data);
                    $scope.$modalInstance.dismiss('cancel');
                    resetArrays();
                }
            }, function (error) {

            });
        }
    }

    $scope.editar = function (data) {
        //$scope.loadCliente();
        addLoader();
        contratoService.editar(data).then(function (response) {
            clienteService.consultar().then(function (responseCliente) {
                removeLoader();

                $scope.Clientes = responseCliente.data;
                $scope.contrato = response.data;
                //$scope.Clientes = response.data.ListaCliente;
                $scope.CriteriosMedicao = response.ListaCriteriosMedicao;
                $scope.contrato.Cliente = getItem($scope.Clientes, response.data.CodigoCliente);
                //$scope.contrato.CriterioMedicao = getItem($scope.CriteriosMedicao, response.data.CriterioMedicao);

                $scope.tituloModal = "Contrato - Editar";
                $scope.$modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'modalContrato',
                    scope: $scope

                });
                $scope.$modalInstance.result.then(function () {
                }, function () {
                    $scope.contrato = {};
                    $scope.Clientes = [];
                });
            });
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

    consultar();
    function consultar() {
        addLoader();
        contratoService.consultar().then(function (response) {
            $scope.contratos = response.data;
            removeLoader()
        });
    }

    function resetArrays() {
        $scope.Clientes = [];
        $scope.CriteriosMedicao = [];
        $scope.TiposEquipamentoDisponiveis = [];
        $scope.TiposEquipamentoAdicionados = [];
        $scope.TiposEquipamentoDisponiveisSelecionados = [];
        $scope.TiposEquipamentoDisponiveisJaAssociadosAoContrato = [];
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
               contratoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.contratos, response.data);
                        swal("", response.data.Message, "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

}]);