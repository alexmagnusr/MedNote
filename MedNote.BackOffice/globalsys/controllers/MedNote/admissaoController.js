'use strict';
Globalsys.controller('admissaoController', ['$scope', '$rootScope', 'painelService', 'admissaoService', '$uibModal', '$timeout', 'utilService', '$location', '$stateParams', function ($scope, $rootScope, painelService, admissaoService, $uibModal, $timeout, utilService, $location, $stateParams) {
    $scope.CodigoLeito = 0;
    $scope.CodigoAdmissao = 0;
    $scope.CodigoSetor = 0;
    $scope.leitos = [];
    $scope.setores = [];
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));


    addLoader();
    painelService.consultar().then(function (response) {
        $scope.setores = response.data;
        removeLoader();
    });


    $scope.abrirPainelLeitos = function (data) {
        localStorage.NomeEstabelecimento = data.EstabelecimentosNome;
        localStorage.NomeSetor = data.Nome;

        localStorage.CodigoSetor = data.Codigo;
        addLoader();
        $location.path('/app/painel');
    };
}]);