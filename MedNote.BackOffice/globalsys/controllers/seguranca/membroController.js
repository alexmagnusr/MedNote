'use strict';
Globalsys.controller('membroController', ['$scope', 'toaster', 'membroService', 'grupoService',  'usuarioService', '$uibModal', '$rootScope', function ($scope, toaster, membroService, grupoService, usuarioService, $uibModal, $rootScope) {
    $scope.membros = [];
    $scope.grupo = [];
    $scope.usuarios = [];
    $scope.tiposColaborador = [];
    $scope.usuariosDisponiveis = [];
    $scope.usuariosDisponiveisSelecionados = [];
    $scope.usuariosDisponiveisJaAssociadosAoGrupo = [];
    $scope.usuariosAdicionados = [];

    $scope.novosMembrosDoGrupo = [];
    $scope.removerMembrosDoGrupo = [];

    $scope.membro = {};
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    function resetArrays() {
        $scope.membros = [];
        $scope.grupo = [];
        //$scope.tiposColaborador = [];
        $scope.usuariosDisponiveis = [];
        $scope.usuariosDisponiveisSelecionados = [];
        $scope.usuariosDisponiveisJaAssociadosAoGrupo = [];
        $scope.usuariosAdicionados = [];

        $scope.novosMembrosDoGrupo = [];
        $scope.removerMembrosDoGrupo = [];
        $scope.usuarios = [];
        $scope.membro.Grupo = undefined;
        $scope.membro.TpColaborador = undefined;

        $scope.loadGrupos();
    }
    
    loadTipoColaboradores();

    $scope.getMembroFormated = function (value) {
        return value.NomeFormatado + " <i>Teste Itálico</i>";
    }

    $scope.$watch('membro.TpColaborador', function (data) {
        if (data !== null && data !== "" && data !== undefined) {
            addLoader();
            loadUsuariosDisponiveis(data);        
            removeLoader();
        }
    });

    $scope.$watch('membro.Grupo', function (idGrupo) {
        if (idGrupo !== null && idGrupo !== "" && idGrupo !== undefined) {
            addLoader();
            $scope.usuariosAdicionados = [];
            consultar(idGrupo);
            $scope.grupo = idGrupo;
            removeLoader();
        }
    });

    $scope.salvar = function () {
        addLoader();

        for (var i = 0; i < $scope.membros.length; i++) {
            if (angular.toJson($scope.usuariosAdicionados).indexOf(angular.toJson($scope.membros[i])) === -1) {
                $scope.removerMembrosDoGrupo.push($scope.membros[i].Codigo);                
            }
        }

        for (var i = 0; i < $scope.usuariosAdicionados.length; i++) {
            if (angular.toJson($scope.membros).indexOf(angular.toJson($scope.usuariosAdicionados[i])) === -1) {
                $scope.novosMembrosDoGrupo.push($scope.usuariosAdicionados[i].Codigo);
            }
        }

        if ($scope.removerMembrosDoGrupo.length > 0) {
            membroService.deletar($scope.removerMembrosDoGrupo, $scope.grupo).then(function (response) {
                if (response.data) {
                }
            }, function (error) { });
        }
      
        if ($scope.novosMembrosDoGrupo.length > 0) {
            membroService.cadastrar($scope.novosMembrosDoGrupo, $scope.grupo).then(function (response) {
                if (response.data) {
                }
            }, function (error) { });
        }

        removeLoader();

        toaster.pop('success', 'Sucesso', 'Operação realizada com sucesso!', 2000);

        resetArrays()
    }

    $scope.loadGrupos = function () {
        if ($scope.moduloID != null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }
        grupoService.consultar($scope.codCliente).then(function (response) {
            $scope.grupos = response.data;
            if ($rootScope.grupoSelecionadoNaGridGrupo) {
                $scope.membro.Grupo = $rootScope.grupoSelecionadoNaGridGrupo.Codigo;
                $rootScope.grupoSelecionadoNaGridGrupo.Codigo = undefined;
            }
        });
    }
        
    function loadUsuariosDisponiveis(tipoColaborador) {
        usuarioService.consultarComFiltroDeTipoDeColaborador(tipoColaborador).then(function (response) {
            $scope.usuariosDisponiveis = response.data;
        });
    }


    function consultar(idGrupo) {
        addLoader();
        membroService.consultar(idGrupo).then(function (response) {
            $scope.membros = response.data;

            for (var i = 0; i < $scope.membros.length; i++) {
                if (angular.toJson($scope.usuariosAdicionados).indexOf(angular.toJson($scope.membros[i])) === -1) {
                    $scope.usuariosAdicionados.push($scope.membros[i]);
                }
            }

            removeLoader()
        });
    }
    
    $scope.addUsuariosSelecionadosAoGrupo = function () {
        addLoader();
        for (var i = 0; i < $scope.usuariosDisponiveisSelecionados.length; i++) {
            if (angular.toJson($scope.usuariosAdicionados).indexOf(angular.toJson($scope.usuariosDisponiveisSelecionados[i])) === -1) {
                $scope.usuariosAdicionados.push($scope.usuariosDisponiveisSelecionados[i]);
            }
        }
        removeLoader();
    }

    $scope.removeUsuariosSelecionadosAoGrupo = function () {
        addLoader();
        for (var i = 0; i < $scope.usuariosDisponiveisJaAssociadosAoGrupo.length; i++) {
            //var index = angular.toJson($scope.usuariosAdicionados).indexOf(angular.toJson($scope.usuariosDisponiveisJaAssociadosAoGrupo[i]));
            var index = $scope.usuariosAdicionados.indexOf($scope.usuariosDisponiveisJaAssociadosAoGrupo[i]);
            $scope.usuariosAdicionados.splice(index,1);            
        }
        removeLoader();
    }

    function getItemUsuarioDisponiveis(item) {
        var selecionado = {};
        for (var i = 0; i < $scope.usuariosDisponiveis.length; i++) {
            if ($scope.usuariosDisponiveis[i].Codigo === item)
                selecionado = $scope.usuariosDisponiveis[i];
        }
        return selecionado;
    }

    function getItemUsuarioJaAdd(item) {
        var selecionado = {};
        for (var i = 0; i < $scope.usuariosAdicionados.length; i++) {
            if ($scope.usuariosAdicionados[i].Codigo === item)
                selecionado = $scope.usuariosAdicionados[i];
        }
        return selecionado;
    }

    $scope.selecionandoUsuariosDisponiveis = function (item) {
        if ($scope.usuariosDisponiveisSelecionados.length > 0) {
            $scope.usuariosDisponiveisSelecionados.length = 0;
        }

        for (var i = 0; i < item.length; i++) {
            $scope.usuariosDisponiveisSelecionados.push(getItemUsuarioDisponiveis(item[i]));
        }
    }

    $scope.selecionandoUsuariosJaAdicionadosNoGrupo = function (item) {
        if ($scope.usuariosDisponiveisJaAssociadosAoGrupo.length > 0) {
            $scope.usuariosDisponiveisJaAssociadosAoGrupo.length = 0;
        }

        for (var i = 0; i < item.length; i++) {
            $scope.usuariosDisponiveisJaAssociadosAoGrupo.push(getItemUsuarioJaAdd(item[i]));
        }
        
    }

    $scope.cancel = function () {
        resetArrays();
    };

}]);