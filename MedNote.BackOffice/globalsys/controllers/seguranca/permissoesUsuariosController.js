'use strict';
Globalsys.controller('permissoesUsuariosController', ['$scope', 'toaster', 'permissoesUsuariosService', 'usuarioService', 'funcaoService', 'acaoService', 'grupoService', '$uibModal', '$rootScope', function ($scope, toaster, permissoesUsuariosService, usuarioService, funcaoService, acaoService, grupoService, $uibModal, $rootScope) {
    $scope.usuario = [];
    $scope.usuarios = [];
    $scope.modulos = [];
    $scope.paginas = [];
    $scope.permissoesUsuario = [];
    $scope.tiposColaborador = [];

    $scope.acoesDisponiveis = [];
    $scope.acoesAdicionadas = [];

    $scope.acoesDisponiveisSelecionadas = [];
    $scope.acoesDisponiveisJaAssociadosAoUsuario = [];

    $scope.removerPermissoesUsuarioUsuarioDoUsuario = [];
    $scope.novosPermissoesUsuarioUsuarioDoUsuario = [];

    $scope.permissaoUsuario = {};
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    function resetArrays() {
        $scope.usuario = [];
        $scope.usuarios = [];
        $scope.modulos = [];
        $scope.paginas = [];
        $scope.permissoesUsuario = [];
        //Manter os perfis carregados
        //$scope.tiposColaborador = [];

        $scope.acoesDisponiveis = [];
        $scope.acoesAdicionadas = [];

        $scope.acoesDisponiveisSelecionadas = [];
        $scope.acoesDisponiveisJaAssociadosAoUsuario = [];

        $scope.removerPermissoesUsuarioUsuarioDoUsuario = [];
        $scope.novosPermissoesUsuarioUsuarioDoUsuario = [];

        $scope.permissaoUsuario.TpColaborador = undefined;
        $scope.permissaoUsuario.Usuario = undefined;
        $scope.permissaoUsuario.Modulo = undefined;
        $scope.permissaoUsuario.Pagina = undefined;
               
    }

    function loadTipoColaboradores() {
        if ($scope.moduloID != null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }
        grupoService.consultar($scope.codCliente).then(function (response) {
            $scope.tiposColaborador = response.data;
        });
    }

    loadTipoColaboradores();

    $scope.$watch('permissaoUsuario.TpColaborador', function (data) {
        if (data !== null && data !== "" && data !== undefined) {
            addLoader();
            $scope.acoesAdicionadas = [];
            loadUsuariosDisponiveis(data);
            removeLoader();
        }
    });

    $scope.$watch('permissaoUsuario.Usuario', function (idUsuario) {
        if (idUsuario !== null && idUsuario !== "" && idUsuario !== undefined) {
            addLoader();
            $scope.acoesAdicionadas = [];
            consultar(idUsuario);
            $scope.usuario = idUsuario;
            $scope.loadModulo();
            removeLoader();
        }
    });

    function loadUsuariosDisponiveis(tipoColaborador) {
        usuarioService.consultarComFiltroDeTipoDeColaborador(tipoColaborador).then(function (response) {
            $scope.usuariosDisponiveis = response.data;
        });
    }

 
    $scope.salvar = function () {
        addLoader();

        for (var i = 0; i < $scope.permissoesUsuario.length; i++) {
            if ($scope.acoesAdicionadas.length <= 0) {
                $scope.removerPermissoesUsuarioUsuarioDoUsuario.push($scope.permissoesUsuario[i].Codigo);
            }
            else if (angular.toJson($scope.acoesAdicionadas).indexOf(angular.toJson($scope.permissoesUsuario[i])) === -1) {
                $scope.removerPermissoesUsuarioUsuarioDoUsuario.push($scope.permissoesUsuario[i].Codigo);
            }
        }

        for (var j = 0; j < $scope.acoesAdicionadas.length; j++) {
            if ($scope.permissoesUsuario.length <= 0) {
                $scope.novosPermissoesUsuarioUsuarioDoUsuario.push($scope.acoesAdicionadas[j].Codigo);
            }
            else if (angular.toJson($scope.permissoesUsuario).indexOf(angular.toJson($scope.acoesAdicionadas[j])) === -1) {
                $scope.novosPermissoesUsuarioUsuarioDoUsuario.push($scope.acoesAdicionadas[j].Codigo);
            }
        }

        if ($scope.removerPermissoesUsuarioUsuarioDoUsuario.length > 0) {
            permissoesUsuariosService.deletar($scope.removerPermissoesUsuarioUsuarioDoUsuario, $scope.usuario).then(function (response) {
                //if (response.data) {
                //   // console.log('passou');
                //}
            }, function (error) { });
        }

        if ($scope.novosPermissoesUsuarioUsuarioDoUsuario.length > 0) {
            permissoesUsuariosService.cadastrar($scope.novosPermissoesUsuarioUsuarioDoUsuario, $scope.usuario).then(function (response) {
                //if (response.data) {
                //}
            }, function (error) { });
        }

        removeLoader();

        toaster.pop('success', 'Sucesso', 'Operação realizada com sucesso!', 2000);

        resetArrays()
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

    function consultar(idUsuario) {
        addLoader();
        permissoesUsuariosService.consultar(idUsuario).then(function (response) {
            $scope.permissoesUsuario = response.data;

            for (var i = 0; i < $scope.permissoesUsuario.length; i++) {
                if (angular.toJson($scope.acoesAdicionadas).indexOf(angular.toJson($scope.permissoesUsuario[i])) === -1) {
                    $scope.acoesAdicionadas.push($scope.permissoesUsuario[i]);
                }
            }

            removeLoader()
        });
    }

    $scope.addAcoesSelecionadosAoUsuario = function () {
        addLoader();
        for (var i = 0; i < $scope.acoesDisponiveisSelecionadas.length; i++) {
            if (angular.toJson($scope.acoesAdicionadas).indexOf(angular.toJson($scope.acoesDisponiveisSelecionadas[i])) === -1) {
                $scope.acoesAdicionadas.push($scope.acoesDisponiveisSelecionadas[i]);
            }
        }
        removeLoader();
    }

    $scope.removeAcoesSelecionadosAoUsuario = function () {
        addLoader();
        for (var i = 0; i < $scope.acoesDisponiveisJaAssociadosAoUsuario.length; i++) {
            var index = $scope.acoesAdicionadas.indexOf($scope.acoesDisponiveisJaAssociadosAoUsuario[i]);
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

    $scope.selecionandoAcoesJaAdicionadosNoUsuario = function (item) {
        if ($scope.acoesDisponiveisJaAssociadosAoUsuario.length > 0) {
            $scope.acoesDisponiveisJaAssociadosAoUsuario.length = 0;
        }

        for (var i = 0; i < item.length; i++) {
            $scope.acoesDisponiveisJaAssociadosAoUsuario.push(getItemAcaoJaAdd(item[i]));
        }

    }

    $scope.cancel = function () {
        resetArrays();
    };

}]);