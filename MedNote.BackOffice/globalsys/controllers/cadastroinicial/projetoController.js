'use strict';
Globalsys.controller('projetoController', ['$scope', 'projetoService', '$uibModal', '$timeout', function ($scope, projetoService, $uibModal, $timeout) {
    $scope.listaRegistros = [];
    $scope.gerencias = [];
    $scope.registro = {};
    $scope.tituloModal = "";
    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };
    $scope.consultarGerencia = function () {
        gerenciaService.consultar().then(function (response) {
            $scope.gerencias = response.data;

        });
    }
    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("projeto.xlsx",{headers:true}) FROM ?', [$scope.listaRegistros]);
    };

    function getItem(lista, item) {
        var selecionado = {};
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].Codigo == item.Codigo)
                selecionado = lista[i];
        }
        return selecionado;
    }

    $scope.salvar = function () {
        if ($scope.registro.Codigo == undefined) {
            addLoader();
            projetoService.cadastrar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.listaRegistros, response.data);
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');
                    swal('Sucesso', 'Projeto inserido com sucesso!', 'success');
                }
            }, function (error) {

            });
        } else {
            addLoader();
            projetoService.atualizar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.listaRegistros, response.data);
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');
                    swal('Sucesso', 'Projeto atualizado com sucesso!', 'success');
                }

            }, function (error) {

            });
        }
    }

    $scope.consultar = function () {
        addLoader();
        projetoService.consultar().then(function (response) {
            $scope.listaRegistros = response.data;
            removeLoader()
        });
    }

    $scope.abrirModal = function () {
        $scope.tituloModal = "Projeto - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalCadastro',
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
                projetoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.listaRegistros, response.data);
                        swal('Sucesso', 'Projeto removido com sucesso!', 'success');
                    }
                });

            }
        });
    }
    $scope.editar = function (data) {
        addLoader();
        projetoService.editar(data).then(function (response) {
            removeLoader();
            $scope.registro = response.data;
            $scope.tituloModal = "Projeto - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalCadastro',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.registro = {};
            });
        });
    }

}]);