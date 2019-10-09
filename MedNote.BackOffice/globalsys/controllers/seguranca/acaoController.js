'use strict';
Globalsys.controller('acaoController', ['$scope', 'acaoService', '$uibModal', 'funcaoService', function ($scope, acaoService, $uibModal, funcaoService) {
    $scope.registros = [];
    $scope.funcoes = [];
    $scope.registro = {};
    $scope.tituloModal = "";
    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };
    
    $scope.salvar = function () {
        var funcao = $scope.registro.Funcao;
        $scope.registro.Funcao = {};
        $scope.registro.Funcao.Codigo = funcao.Codigo;

        if ($scope.registro.Codigo == undefined) {
            addLoader();
            
            acaoService.cadastrar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.registros, response.data);
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');
                }
            }, function (error) {

            });
        } else {
            addLoader();
            acaoService.atualizar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.registros, response.data);
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');
                }
            }, function (error) {

            });
        }
    }

    $scope.consultarFuncoes = function () {
        if ($scope.funcoes.length <= 0) {
            funcaoService.consultar().then(function (response) {
                $scope.funcoes = response.data;
                if ($scope.registro.Codigo != undefined) {
                    $scope.registro.Funcao = getItem($scope.funcoes, $scope.registro.CodFuncao);
                }
            });
        } else {
            if ($scope.registro.Codigo != undefined) {
                $scope.registro.Funcao = getItem($scope.funcoes, $scope.registro.CodFuncao);
            }
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

    $scope.consultar = function () {
        addLoader();
        acaoService.consultar().then(function (response) {
            $scope.registros = response.data;
            removeLoader();
        });
    }

    $scope.abrirModal = function () {
        $scope.tituloModal = "ACAO - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalACAO',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.registro = {};
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
               acaoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.registros, response.data);
                    }
                });

            }
        });
    }
    $scope.editar = function (data) {
        addLoader();
       acaoService.editar(data).then(function (response) {
            removeLoader();
            $scope.registro = response.data;
            $scope.tituloModal = "ACAO - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalACAO',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.registro = {};
            });
        });
    }
 
}]);
