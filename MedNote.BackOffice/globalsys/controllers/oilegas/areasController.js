'use strict';
Globalsys.controller('areasController', ['$scope', 'clienteService', 'areasService', '$uibModal', '$timeout', 'funcaoService', function ($scope, clienteService, areasService, $uibModal, $timeout, funcaoService) {
    $scope.areas = [];
    $scope.clientes = [];
    $scope.grausDeGrauDeUrgencia = [];
    $scope.area = {};
    $scope.tituloModal = "";
    $scope.tiposColaborador = [];
    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };
    $scope.consultarCliente = function () {
        clienteService.consultar().then(function (response) {
            $scope.clientes = response.data;
        });
    }

    $scope.exportData = function () {
      
        //var dir_paginate = document.getElementById("trAreas").getAttribute("dir-paginate").replace(/\s/g, "");
        //var poisicaoTag = dir_paginate.indexOf("itemsPerPage:");

        //var numItensPoPagina = parseInt(dir_paginate.charAt(poisicaoTag + "itemsPerPage:".length)); 
        //var paginaAtual = parseInt(document.getElementById("pagina_atual").innerHTML);

        alasql('SELECT * INTO XLSX("areas.xlsx",{headers:true}) FROM ?', [$scope.areas]);
    };

    $scope.exportToExcel = function (tableId) { // ex: '#my-table'
        $(".display").remove();
      
    
        var exportHref = Excel.tableToExcel(tableId, 'WireWorkbenchDataExport');
        $timeout(function () { location.href = exportHref; }, 100); // trigger download
        $window.location.reload();
    }

    $scope.consultarCliente();

    $scope.consultarGraudDeUrgencia = function () {
        if ($scope.grausDeGrauDeUrgencia.length <= 0) {
            areasService.consultarGrauDeUrgenciaArea().then(function (response) {
                $scope.grausDeGrauDeUrgencia = response.data;
            });
        }  
    }

    function getItem(lista, item) {
        var selecionado = {};
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].Codigo == item.Codigo)
                selecionado = lista[i];
        }
        return selecionado;
    }
    $scope.consultar = function () {
        addLoader();
        areasService.consultar().then(function (response) {
            $scope.areas = response.data;
            removeLoader()
        });
    }
    $scope.salvar = function () {
        if ($scope.area.Codigo == undefined) {
            addLoader();
            areasService.cadastrar($scope.area).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.areas, response.data);
                    $scope.area = {};
                    $scope.$modalInstance.dismiss('cancel');

                }
            }, function (error) {

            });
        } else {
            addLoader();
            areasService.atualizar($scope.area).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.areas, response.data);
                    $scope.area = {};
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }


    $scope.abrirModal = function () {
        $scope.tituloModal = "Área - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalAreas',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.area = {};
        });
    }

    $scope.editar = function (data) {
        addLoader();
        areasService.editar(data).then(function (response) {
            removeLoader();
            $scope.area = response.data;
            $scope.area.Cliente = getItem($scope.clientes, $scope.area.Cliente);
            $scope.tituloModal = "Área - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalAreas',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.area = {};
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
            confirmButtonText: "Sim!",
            cancelButtonText: "Não!",
            closeOnConfirm: true,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                addLoader();
                areasService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.areas, response.data);
                    }
                });

            }
        });
    }
}]);