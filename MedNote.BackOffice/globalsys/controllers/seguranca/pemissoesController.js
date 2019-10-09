'use strict';
Globalsys.controller('permissoesController', ['$scope', 'toaster' ,'permissoesService', 'grupoService', 'funcaoService', 'acaoService', '$uibModal', '$rootScope', function ($scope, toaster, permissoesService, grupoService, funcaoService, acaoService, $uibModal, $rootScope) {

    $scope.grupo = [];
    $scope.grupos = [];
    $scope.modulos = [];
    $scope.paginas = [];
    $scope.permissoes = [];

    $scope.acoesDisponiveis = [];
    $scope.acoesAdicionadas = [];

    $scope.acoesDisponiveisSelecionadas = [];
    $scope.acoesDisponiveisJaAssociadosAoGrupo = [];

    $scope.removerPermissoesDoGrupo = [];
    $scope.novosPermissoesDoGrupo = [];

    $scope.permissao = {};
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    function resetArrays() {
        $scope.grupo = [];
        $scope.grupos = [];
        $scope.modulos = [];
        $scope.paginas = [];
        $scope.permissoes = [];

        $scope.acoesDisponiveis = [];
        $scope.acoesAdicionadas = [];

        $scope.acoesDisponiveisSelecionadas = [];
        $scope.acoesDisponiveisJaAssociadosAoGrupo = [];

        $scope.removerPermissoesDoGrupo = [];
        $scope.novosPermissoesDoGrupo = [];

        $scope.loadGrupos();
        $scope.loadModulo();
        $scope.loadPagina();
    }

    $scope.$watch('permissao.Grupo', function (idGrupo) {
        if (idGrupo !== null && idGrupo !== "" && idGrupo !== undefined) {
            addLoader();
            consultar(idGrupo);
            $scope.grupo = idGrupo;
            $scope.loadModulo();
            removeLoader();
        }
    });

    $scope.salvar = function () {
        addLoader();
        debugger
        for (var i = 0; i < $scope.permissoes.length; i++) {
            if ($scope.acoesAdicionadas.length <= 0) {
                $scope.removerPermissoesDoGrupo.push($scope.permissoes[i].Codigo);
            }
            else if (angular.toJson($scope.acoesAdicionadas).indexOf(angular.toJson($scope.permissoes[i])) === -1) {
                $scope.removerPermissoesDoGrupo.push($scope.permissoes[i].Codigo);
            }
        }

        for (var j = 0; j < $scope.acoesAdicionadas.length; j++) {
            if ($scope.permissoes.length <= 0) {
                $scope.novosPermissoesDoGrupo.push($scope.acoesAdicionadas[j].Codigo);
            }
            else if (angular.toJson($scope.permissoes).indexOf(angular.toJson($scope.acoesAdicionadas[j])) === -1) {
                $scope.novosPermissoesDoGrupo.push($scope.acoesAdicionadas[j].Codigo);
            }
        }

        if ($scope.removerPermissoesDoGrupo.length > 0) {
            permissoesService.deletar($scope.removerPermissoesDoGrupo, $scope.grupo).then(function (response) {
            }, function (error) { });
        }

        if ($scope.novosPermissoesDoGrupo.length > 0) {
            permissoesService.cadastrar($scope.novosPermissoesDoGrupo, $scope.grupo).then(function (response) {
            }, function (error) { });
        }

        removeLoader();

        toaster.pop('success', 'Sucesso', 'Operação realizada com sucesso!', 2000);

        resetArrays()
    }

    $scope.loadGrupos = function () {
        
        if ($scope.moduloID !== null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }
        grupoService.consultar($scope.codCliente).then(function (response) {
            $scope.grupos = response.data;
            if ($rootScope.grupoSelecionadoNaGridGrupo) {
                $scope.permissao.Grupo = $rootScope.grupoSelecionadoNaGridGrupo.Codigo;
                $rootScope.grupoSelecionadoNaGridGrupo.Codigo = undefined;
            }
        });
    }

    $scope.loadModulo = function () {
        
        funcaoService.ConsultarPorTipo(0).then(function (response) {
            $scope.modulos = response.data;
        });
    }

    $scope.loadPagina = function (data) {
        if (data) {
            funcaoService.ConsultarPaginasPorModulo(data).then(function (response) {
                $scope.paginas = response.data;
            });
        }
    }

    $scope.loadAcoesDisponiveis = function (data) {
        if (data) {
            acaoService.ConsultarAcoesPorPagina(data).then(function (response) {
                $scope.acoesDisponiveis = response.data;
            });
        }
    }

    function consultar(idGrupo) {
        addLoader();
        permissoesService.consultar(idGrupo).then(function (response) {
            $scope.permissoes = response.data;

            for (var i = 0; i < $scope.permissoes.length; i++) {
                if (angular.toJson($scope.acoesAdicionadas).indexOf(angular.toJson($scope.permissoes[i])) === -1) {
                    $scope.acoesAdicionadas.push($scope.permissoes[i]);
                }
            }

            removeLoader()
        });
    }

    $scope.addAcoesSelecionadosAoGrupo = function () {
        addLoader();
        for (var i = 0; i < $scope.acoesDisponiveisSelecionadas.length; i++) {
            if (angular.toJson($scope.acoesAdicionadas).indexOf(angular.toJson($scope.acoesDisponiveisSelecionadas[i])) === -1) {
                $scope.acoesAdicionadas.push($scope.acoesDisponiveisSelecionadas[i]);
            }
        }
        removeLoader();
    }

    $scope.removeAcoesSelecionadosAoGrupo = function () {
        addLoader();
        for (var i = 0; i < $scope.acoesDisponiveisJaAssociadosAoGrupo.length; i++) {
            //var index = angular.toJson($scope.acoesAdicionadas).indexOf(angular.toJson($scope.acoesDisponiveisJaAssociadosAoGrupo[i]));
            var index = $scope.acoesAdicionadas.indexOf($scope.acoesDisponiveisJaAssociadosAoGrupo[i]);
            $scope.acoesAdicionadas.splice(index, 1);
        }
        removeLoader();
    }

    function getItemAcaoDisponivel(item) {
        var selecionado = {};
        for (var i = 0; i < $scope.acoesDisponiveis.length; i++) {
            if ($scope.acoesDisponiveis[i].Codigo === item)
                selecionado = $scope.acoesDisponiveis[i];
        }
        return selecionado;
    }

    function getItemAcaoJaAdd(item) {
        var selecionado = {};
        for (var i = 0; i < $scope.acoesAdicionadas.length; i++) {
            if ($scope.acoesAdicionadas[i].Codigo === item)
                selecionado = $scope.acoesAdicionadas[i];
        }
        return selecionado;
    }

    $scope.selecionandoAcoesDisponiveis = function (item) {
        if ($scope.acoesDisponiveisSelecionadas.length > 0) {
            $scope.acoesDisponiveisSelecionadas.length = 0;
        }

        for (var i = 0; i < item.length; i++) {
            $scope.acoesDisponiveisSelecionadas.push(getItemAcaoDisponivel(item[i]));
        }
    }

    $scope.selecionandoAcoesJaAdicionadosNoGrupo = function (item) {
        if ($scope.acoesDisponiveisJaAssociadosAoGrupo.length > 0) {
            $scope.acoesDisponiveisJaAssociadosAoGrupo.length = 0;
        }

        for (var i = 0; i < item.length; i++) {
            $scope.acoesDisponiveisJaAssociadosAoGrupo.push(getItemAcaoJaAdd(item[i]));
        }

    }

    $scope.cancel = function () {
        resetArrays();
    };

}]);