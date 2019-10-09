'use strict';
Globalsys.controller('classeFinanceiraController', ['$scope', 'classeFinanceiraService', 'tipoDespesaService', 'tipoReembolsoService', '$uibModal', '$timeout', function ($scope, classeFinanceiraService, tipoDespesaService, tipoReembolsoService, $uibModal, $timeout) {
    $scope.classes = [];
    $scope.classe = {};
    $scope.Reembolsos = [];
    $scope.Despesas = [];

    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;

    $scope.novosTiposEquipamentoContrato = [];
    $scope.removerTiposEquipamentoContrato = [];


    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("classeFinanceira.xlsx",{headers:true}) FROM ?', [$scope.classes]);
    };

    $scope.abrirModal = function () {
        $scope.classe = {};
        $scope.tituloModal = "Classe financeira - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalContrato',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.classe = {};
        });
    }

    $scope.loadReembolsos = function () {
        if ($scope.Reembolsos.length <= 0) {
            tipoReembolsoService.consultar().then(function (response) {
                $scope.Reembolsos = response.data;
            });
        }
    }

    $scope.loadDespesas = function () {
        if ($scope.Despesas.length <= 0) {
            tipoDespesaService.consultar().then(function (response) {
                $scope.Despesas = response.data;
            });
        }
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

        //$scope.classe.TipoEquipamentos = $scope.TiposEquipamentoAdicionados;
        if ($scope.classe.Codigo == undefined) {
            //Salvar
            classeFinanceiraService.cadastrar($scope.classe).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.classes, response.data);
                    $scope.$modalInstance.dismiss('cancel');
                    swal('Sucesso', 'Classe Financeira incluída com sucesso!', 'success');
                    resetArrays();
                }
            }, function (error) {

            });
        } else {
            //Atualizar
            classeFinanceiraService.atualizar($scope.classe).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.classes, response.data);
                    $scope.$modalInstance.dismiss('cancel');
                    swal('Sucesso', 'Classe Financeira atualizada com sucesso!', 'success');
                    resetArrays();
                }
            }, function (error) {

            });
        }
    }

    $scope.editar = function (data) {
        //$scope.loadCliente();
        addLoader();
        classeFinanceiraService.editar(data).then(function (response) {
            tipoReembolsoService.consultar().then(function (responseCliente) {


               
                removeLoader();

                $scope.Reembolsos = responseCliente.data;
                $scope.classe = response.data;
  
                $scope.classe.TipoReembolso = getItem($scope.Reembolsos, response.data.CodigoTipoReembolso);
                


                tipoDespesaService.consultar().then(function (responseDespesa) {
                    $scope.Despesas = responseDespesa.data;
                    $scope.classe.TipoDespesa = getItem($scope.Despesas, response.data.CodigoTipoDespesa);
                });
                

                $scope.tituloModal = "Classe financeira - Editar";
                $scope.$modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'modalContrato',
                    scope: $scope

                });
                $scope.$modalInstance.result.then(function () {
                }, function () {
                    $scope.classe = {};
                    $scope.Reembolsos = [];
                });
            }

            );
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
        classeFinanceiraService.consultar().then(function (response) {
            $scope.classes = response.data;
            removeLoader()
        });
    }

    function resetArrays() {
        $scope.Reembolsos = [];
        $scope.Despesas = [];
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
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                addLoader();
               classeFinanceiraService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.classes, response.data);
                        swal('Sucesso', 'Classe Financeira removida com sucesso!', 'success');
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada!", "success");
            }
        });
    }

}]);