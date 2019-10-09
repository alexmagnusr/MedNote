'use strict';
Globalsys.controller('funcaoController', ['$rootScope', '$scope', 'toaster', 'funcaoService', '$uibModal', '$timeout', function ($rootScope, $scope, toaster, funcaoService, $uibModal, $timeout) {
    $scope.funcoes = [];

    $scope.tiposFuncao = [];
    $scope.canaisFuncao = [];
    $scope.funcoesModulos = [];
    $scope.canais = [];

    $scope.funcao = {};
    $scope.tituloModal = "Cadastro de Páginas e Modulos";

    $scope.cancel = function () {
        $scope.$modalFuncao.dismiss('cancel');
    };

    $scope.salvar = function () {
        if ($scope.funcao.Codigo == undefined) {
            addLoader();
            funcaoService.cadastrar($scope.funcao).then(function (response) {
                removeLoader();
                if (response.data) {

                    add($scope.funcoes, response.data);
                    $scope.funcao = {};
                    $scope.$modalFuncao.dismiss('cancel');

                } else {
                    sweetAlert("Atenção", response.data.Message, "error");
                }

            }, function (error) {

            });
        } else {
            addLoader();
            funcaoService.atualizar($scope.funcao).then(function (response) {
                removeLoader();
                if (response.data) {

                    update($scope.funcoes, response.data);
                    $scope.funcao = {};
                    $scope.$modalFuncao.dismiss('cancel');

                } else {
                    sweetAlert("Atenção", response.data.Message, "error");
                }

            }, function (error) {

            });
        }

        toaster.pop('success', 'Sucesso', 'Operação realizada com sucesso!', 2000);
    }

    $scope.consultar = function () {
        addLoader();
        funcaoService.consultar().then(function (response) {
            $scope.funcoes = response.data;
            removeLoader()
        });
    }

    $scope.loadTipo = function () {
        funcaoService.ConsultarTipo().then(function (response) {
            $scope.tiposFuncao = response.data;
        });
    }
  
    $scope.loadCanal = function () {
        funcaoService.ConsultarCanal().then(function (response) {
            $scope.canais = response.data;
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

    $scope.loadCanal();

    $scope.loadModulo = function () {
        funcaoService.ConsultarPorTipo(0).then(function (response) {
            $scope.funcoesModulos = response.data;
            if ($scope.funcao.Codigo != undefined) {
                $scope.funcao.Pai = getItem($scope.funcoesModulos, $scope.funcao.CodPai);
            }
        });
    }

    $scope.loadTipo();

    $scope.consultar();

    $scope.abrirModal = function () {
        $scope.tituloModal = "Páginas e Modulos - Novo";
        $scope.funcao = {};
        $scope.$modalFuncao = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalFuncao',
            scope: $scope
        });
        $scope.$modalFuncao.result.then(function () {
        }, function () {
            $scope.funcao = {};
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
                funcaoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.funcoes, response.data);
                    }
                });

            }
        });
    }
    $scope.editar = function (data) {
        addLoader();
        funcaoService.editar(data).then(function (response) {
            removeLoader();
            $scope.funcao = response.data;
            $scope.tituloModal = "Função - Editar";
            $scope.$modalFuncao = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalFuncao',
                scope: $scope

            });
            $scope.$modalFuncao.result.then(function () {
            }, function () {
                $scope.funcao = {};
            });
        });
    }

}]);