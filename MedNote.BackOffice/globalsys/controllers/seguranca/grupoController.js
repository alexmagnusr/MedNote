'use strict';
Globalsys.controller('grupoController', ['$scope', 'toaster', 'grupoService', '$uibModal', '$rootScope', '$state', 'clienteService', function ($scope, toaster, grupoService, $uibModal, $rootScope, $state, clienteService) {
    $scope.grupos = [];
    $scope.grupo = {};
    $scope.tituloModal = "";
    //$scope.clientes = [];
    //$scope.cliente = {};
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    $scope.loadGrupos = function () {
        //clienteService.consultar().then(function (response) {
        //    $scope.clientes = response.data;
        
        //});
        
        addLoader();
        //if ($scope.moduloID != null) {
        //    $scope.codCliente = $scope.moduloID.Codigo;
        //}
        //else {
        //    $scope.codCliente = 0;
        //}

        grupoService.consultar().then(function (response) {
            $scope.grupos = response.data;
            removeLoader();
        });
    }

    $scope.loadGrupos();

    $scope.cancel = function () {
        $scope.$modalGrupoInstance.dismiss('cancel');
        $scope.grupo = {};
    };


    //function consultar() {
    //    debugger
    //    addLoader();
    //    if ($scope.moduloID != null) {
    //        $scope.codCliente = $scope.moduloID.Codigo;
    //    }
    //    else {
    //        $scope.codCliente = 0;
    //    }

    //    grupoService.consultar($scope.codCliente).then(function (response) {
    //        $scope.grupos = response.data;
    //        removeLoader();
    //    });
    //}

    //consultar();

    //$scope.exportData = function () {
    //    alasql('SELECT * INTO XLSX("grupo.xlsx",{headers:true}) FROM ?', [$scope.grupos]);
    //};

    $scope.salvar = function () {

        if ($scope.grupo.Codigo == undefined) {
            addLoader();
           // $scope.grupo.CodigoCliente = $scope.grupo.Cliente.Codigo;

            grupoService.cadastrar($scope.grupo).then(function (response) {
                removeLoader();
                if (response.data) {

                    add($scope.grupos, response.data);
                    $scope.grupo = {};
                    $scope.$modalGrupoInstance.dismiss('cancel');

                } else {
                    sweetAlert("Atenção", response.data.Message, "error");
                }

            }, function (error) {

            });
        } else {
            addLoader();
            //$scope.grupo.CodigoCliente = $scope.grupo.Cliente.Codigo;
            grupoService.atualizar($scope.grupo).then(function (response) {
                removeLoader();
                if (response.data) {

                    update($scope.grupos, response.data);
                    $scope.grupo = {};
                    $scope.$modalGrupoInstance.dismiss('cancel');

                } else {
                    sweetAlert("Atenção", response.data.Message, "error");
                }

            }, function (error) {

            });
        }

        toaster.pop('success', 'Sucesso', 'Operação realizada com sucesso!', 2000);
    }
 

    $scope.abrirModal = function () {
        //clienteService.consultar().then(function (response) {
        //    $scope.Clientes = response.data;
        //    if ($scope.Clientes.length == 1)
        //        $scope.grupo.Cliente = $scope.Clientes[0];
        //});
        //$scope.grupo.Cliente = $scope.moduloID.Codigo;
        $scope.tituloModal = "Perfil - Novo";
        $scope.$modalGrupoInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalGrupo',
            scope: $scope

        });
        $scope.$modalGrupoInstance.result.then(function () {
        }, function () {
            $scope.grupo = {};
        });
    }

    $scope.deletar = function (data) {
        swal({
            title: "Atenção",
            text: "Você tem certeza que gostaria de remover este registro?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {

                addLoader();
                grupoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.grupos, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

    $scope.editar = function (data) {
        addLoader();
        grupoService.editar(data).then(function (response) {
            removeLoader();

            $scope.grupo = response.data;
            
            //$scope.grupo.Cliente = getItem($scope.clientes, response.data.Cliente);
            //$scope.cliente = $scope.grupo.Cliente;
            $scope.tituloModal = "Perfil - Editar";
            $scope.$modalGrupoInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalGrupo',
                scope: $scope

            });
            $scope.$modalGrupoInstance.result.then(function () {
            }, function () {
                $scope.grupo = {};
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
}]);