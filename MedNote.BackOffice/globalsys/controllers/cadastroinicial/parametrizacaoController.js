'use strict';
Globalsys.controller('parametrizacaoController', ['$scope', 'parametrizacaoService', '$uibModal', '$timeout', 'gerenciaService', function ($scope, parametrizacaoService, $uibModal, $timeout, gerenciaService) {
    $scope.centrosDecustos = [];
    $scope.gerencias = [];
    $scope.parametrizacao = {};
    $scope.tituloModal = "";
    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
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


            addLoader();
            parametrizacaoService.atualizar($scope.parametrizacao).then(function (response) {
                removeLoader();
                if (response.data) {
                    update(1, response.data);
                    swal('Sucesso', 'Parâmetros atualizados com sucesso!', 'success');
                }

            }, function (error) {

            });

    }

    $scope.consultar = function () {
        addLoader();
        parametrizacaoService.consultar().then(function (response) {
            $scope.centrosDecustos = response.data;
            removeLoader()
        });
    }

    $scope.abrirModal = function () {
        $scope.tituloModal = "Centro de custo - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalparametrizacao',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.parametrizacao = {};
        });
    }
   
    $scope.editar = function (Codigo) {

        addLoader();
        parametrizacaoService.editar(Codigo).then(function (response) {
            removeLoader();
            $scope.parametrizacao = response.data;

            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.parametrizacao = {};
            });
        });
    }

    $scope.editar(1);

}]);