'use strict';
Globalsys.controller('nivelAprovacaoController', ['$scope', 'nivelAprovacaoService', 'centroDeCustoService', '$uibModal', '$timeout', function ($scope, nivelAprovacaoService, centroDeCustoService, $uibModal, $timeout) {
    $scope.classes = [];
    $scope.classe = {};
    $scope.Centros = [];
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
        alasql('SELECT * INTO XLSX("nivelAprovacao.xlsx",{headers:true}) FROM ?', [$scope.classes]);
    };

    $scope.abrirModal = function () {
        $scope.classe = {};
        $scope.tituloModal = "Nível de Aprovação - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalCadastro',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.classe = {};
        });
    }

    $scope.abrirModalAprovadores = function (item) {
        $scope.classe = {};
        $scope.tituloModalAprovador = "Aprovadores Centro de Custo - Nível " + item.Nivel;
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalAprovadores',
            scope: $scope
        }); 

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.classe = {};
        });
    }

    $scope.loadCentros = function () {
        if ($scope.Centros.length <= 0) {
            centroDeCustoService.consultar().then(function (response) {
                $scope.Centros = response.data;
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
            nivelAprovacaoService.cadastrar($scope.classe).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.classes, response.data);
                    $scope.$modalInstance.dismiss('cancel');
                    swal('Sucesso', 'Nível de Aprovação incluído com sucesso!', 'success');
                    resetArrays();
                }
            }, function (error) {

            });
        } else {
            //Atualizar
            nivelAprovacaoService.atualizar($scope.classe).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.classes, response.data);
                    $scope.$modalInstance.dismiss('cancel');
                    swal('Sucesso', 'Nível de Aprovação atualizado com sucesso!', 'success');
                    resetArrays();
                }
            }, function (error) {

            });
        }
    }

    $scope.editar = function (data) {
        //$scope.loadCliente();
        addLoader();
        nivelAprovacaoService.editar(data).then(function (response) {
            centroDeCustoService.consultar().then(function (responseCliente) {


               
                removeLoader();

                $scope.Centros = responseCliente.data;
                $scope.classe = response.data;
  
                $scope.classe.CentroCusto = getItem($scope.Centros, response.data.CodigoCentroDeCusto);
                

             

                $scope.tituloModal = "Nível de Aprovação - Editar";
                $scope.$modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'modalCadastro',
                    scope: $scope

                });
                $scope.$modalInstance.result.then(function () {
                }, function () {
                    $scope.classe = {};
                    $scope.Centros = [];
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
        nivelAprovacaoService.consultar().then(function (response) {
            $scope.classes = response.data;
            removeLoader()
        });
    }

    function resetArrays() {
        $scope.Centros = [];
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
               nivelAprovacaoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.classes, response.data);
                        swal('Sucesso', 'Nível de Aprovação removido com sucesso!', 'success');
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada!", "success");
            }
        });
    }

}]);