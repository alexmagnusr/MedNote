'use strict';
Globalsys.controller('tipoEquipamentoContratoController', ['$scope', 'tipoEquipamentoContratoService', 'contratoService', 'tipoEquipamentoService', '$uibModal', '$timeout', function ($scope, tipoEquipamentoContratoService, contratoService, tipoEquipamentoService, $uibModal, $timeout) {
    $scope.contratos = [];
    $scope.contrato = {};
    $scope.TiposEquipamentoDisponiveis = [];
    $scope.TiposEquipamentoAdicionados = [];
    $scope.TiposEquipamentoDisponiveisSelecionados = [];
    $scope.TiposEquipamentoAdicionadosSelecionados = [];
    $scope.TiposEquipamentoContrato = [];


    function resetArrays() {
        $scope.contrato = {};
        //$scope.contratos = [];
        $scope.TiposEquipamentoDisponiveis = [];
        $scope.TiposEquipamentoAdicionados = [];
        $scope.TiposEquipamentoDisponiveisSelecionados = [];
        $scope.TiposEquipamentoDisponiveisJaAssociadosAoContrato = [];
        $scope.TiposEquipamentoContrato = [];
    }

    $scope.carregarContratos = function (codigo) {

        contratoService.consultar().then(function (response) {
            $scope.contratos = response.data;
        });
    }

    $scope.carregarDadosContrato = function (codigo) {
        addLoader();
        tipoEquipamentoContratoService.consultar(codigo).then(function (response) {
            carregaComponentes(response.data);
            removeLoader();
        });
    }

    function carregaComponentes(data) {
        $scope.TiposEquipamentoAdicionados = data.ListaEquipamentosContrato;
        $scope.TiposEquipamentoDisponiveis = data.ListaEquipamentosDisponiveis;
    }

    function loadTipoEquipamentos() {
        tipoEquipamentoService.consultar().then(function (response) {
            $scope.TiposEquipamentoDisponiveis = response.data;
        });
    }

    $scope.selecionandoEquipamentosDisponiveis = function (item) {
        $scope.TiposEquipamentoDisponiveisSelecionados = item;
    }

    $scope.selecionandoEquipamentosJaAdicionados = function (item) {
        $scope.TiposEquipamentoAdicionadosSelecionados = item;
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

    $scope.cancel = function () {
        resetArrays();
    }

    $scope.salvar = function () {
        addLoader();
        adicionaContratoEquipamentos();
        if ($scope.contrato.Codigo != undefined) {
            tipoEquipamentoContratoService.cadastrar($scope.contrato.Codigo, $scope.TiposEquipamentoContrato).then(function (response) {
                removeLoader();
                if (response.data) {
                    toaster.pop('success', 'Sucesso', 'Operação realizada com sucesso!', 2000);
                    resetArrays();
                }
            }, function (error) {

            });
        }
    }

    function adicionaContratoEquipamentos() {
        $scope.TiposEquipamentoContrato = [];
        for (var i = 0; i < $scope.TiposEquipamentoAdicionados.length; i++) {
            var tipoequipamento = new Object();
            tipoequipamento = $scope.TiposEquipamentoAdicionados[i].Codigo;
            $scope.TiposEquipamentoContrato.push(tipoequipamento);
        }
    }
}]);