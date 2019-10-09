'use strict';
Globalsys.controller('painelController', ['$scope', '$rootScope', 'painelService', 'admissaoService', '$uibModal', '$timeout', 'utilService', '$location', '$stateParams', function ($scope, $rootScope, painelService, admissaoService, $uibModal, $timeout, utilService, $location, $stateParams) {
    $scope.CodigoLeito = 0;
    $scope.CodigoAdmissao = 0;
    $scope.CodigoSetor = 0;
    $scope.leitos = [];
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;   
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));


    addLoader();
    painelService.abrirPainel(localStorage.CodigoSetor).then(function (response) {
        $scope.leitos = response.data;
        $scope.TaxaOcupacao = $scope.leitos[0].TaxaOcupacao;
        removeLoader();
    });

    $scope.tituloModal = "MedNote - " + localStorage.NomeEstabelecimento;
    $scope.setorModal = localStorage.NomeSetor;
    $scope.$modalInstance = $uibModal.open({
        animation: $scope.animationsEnabled,
        templateUrl: 'modalPainelMaster',
        windowClass: 'app-modal-window',
        scope: $scope
    });

    $scope.$modalInstance.result.catch(function () {
        if ($location.$$path === '/app/painel' || $location.$$path === '/app/setor') 
            $location.path('/app/setor');
        else
            $location.path($location.$$path);
    }, function () {
        $scope.leitos = {};
    });


    $scope.cancel = function () {
        addLoader();
        $location.path('/app/admissao');
    };




    $scope.abrirMasterAdmissao = function (data) {
        localStorage.NomeEstabelecimento = data.NomeEstabelecimento;
        localStorage.Identificador = data.Identificador;
        localStorage.NomeSetor = data.NomeSetor;
        localStorage.CodigoLeito = data.CodigoLeito;
        localStorage.CodigoSetor = data.CodigoSetor;
        
        if (data.CodigoAdmissao === null)
            localStorage.CodigoAdmissao = 0;
        else
            localStorage.CodigoAdmissao = data.CodigoAdmissao;
        addLoader();
        $location.path('/app/admissao/master/');
    };
}]);