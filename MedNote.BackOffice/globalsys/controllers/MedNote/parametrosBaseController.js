'use strict';
Globalsys.controller('parametrosBaseController', ['$scope', 'parametrosBaseService', '$uibModal', '$timeout', function ($scope, parametrosBaseService, $uibModal, $timeout) {
    $scope.listaParametrosBase = [];
    $scope.parametrosBase = {};
    $scope.Tipos = [];
    $scope.tipo = 1;
    $scope.descricao = "";
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    loadData();
    function loadData () {
        parametrosBaseService.ObterTipos().then(function (response) {
            $scope.Tipos = response.data;
        });

        $scope.codCliente = $scope.moduloID != null ? $scope.moduloID.Codigo : 0;

        parametrosBaseService.consultar($scope.codCliente, $scope.tipo, $scope.descricao)
        .then(function (response) {
            $scope.listaParametrosBase = response.data;
        });
    }

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("cadastros_basicos.xlsx",{headers:true}) FROM ?', [$scope.listaParametrosBase]);
    };

    $scope.abrirModal = function () {
        $scope.parametrosBase = {};
        $scope.tituloModal = "Cadastro Básico - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalParametrosBase',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.parametrosBase = {};
        });
    }

    $scope.salvar = function () {
        
        //$scope.parametrosBase.Cliente = $scope.moduloID.Codigo;
        $scope.parametrosBase.Cliente = { Codigo: $scope.moduloID.Codigo };
        if ($scope.parametrosBase.Codigo == undefined) {
            addLoader();
            parametrosBaseService.cadastrar($scope.parametrosBase).then(function (response) {
                removeLoader();
                if (response.data) {
                    if (response.data.Tipo == $scope.tipo)
                        add($scope.listaParametrosBase, response.data);

                    $scope.parametrosBase = {};
                    $scope.$modalInstance.dismiss('cancel');
                }
            }, function (error) {

            });
        } else {
            addLoader();
            parametrosBaseService.atualizar($scope.parametrosBase).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.listaParametrosBase, response.data);
                    $scope.parametrosBase = {};
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }

    $scope.consultar = function () {
        addLoader();
        if ($scope.moduloID != null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }

        parametrosBaseService.consultar($scope.codCliente, $scope.tipo, $scope.descricao).then(function (response) {
            $scope.listaParametrosBase = response.data;
            removeLoader();
        });
    }

    $scope.editar = function (data) {
        addLoader();
        parametrosBaseService.editar(data).then(function (response) {
            removeLoader();
            $scope.parametrosBase = response.data;
            // $timeout(function () {
            //    if (response.data.Estabelecimento !== null)
            //         $scope.usuario.Estabelecimento = getItem($scope.estabelecimentos, response.data.Estabelecimento);
            // });

            $scope.tituloModal = "Cadastro Básico - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalParametrosBase',
                scope: $scope
            });

            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.parametrosBase = {};
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
                parametrosBaseService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.listaParametrosBase, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });
            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

}]);