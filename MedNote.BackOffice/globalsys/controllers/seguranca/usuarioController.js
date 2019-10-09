
'use strict';
Globalsys.controller('usuarioController', ['$rootScope', '$scope', 'usuarioService', 'setorUsuarioService', 'setorService', 'contratoService', '$uibModal', '$timeout', 'clienteService', 'especialidadeService', 'grupoService', 'estabelecimentoService', function ($rootScope, $scope, usuarioService, setorUsuarioService, setorService, contratoService, $uibModal, $timeout, clienteService, especialidadeService, grupoService, estabelecimentoService) {
    $scope.LoginDigitado;
    $scope.usuarios = [];
    $scope.especialidades = [];
    $scope.estabelecimentos = [];
    $scope.grupos = [];
    $scope.Clientes = [];
    $scope.setoresUsuario = [];
    $scope.setores = [];
    $scope.setorUsuario = {};   
    $scope.setor = {};

    $scope.usuario = {
        Nome: "", Login: "", Email: "", NumDocumento: "", LoginAd: false, Admin: false, Especialidade: {}, Grupo: {}, Cliente: {}, EstabelecimentoSaude: {}
    };

    $scope.tituloModal = "";
    $scope.usuDetalhe = {};
    $scope.contratos = [];
    $scope.selecionado = [];
    $scope.contratosSelecionados = [];
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    $scope.cancel = function () {
        $scope.usuario = {};
        $scope.selecionado = [];
        $scope.contratos = [];
        $scope.itens_Clientes = [];
        $scope.$modalInstance.dismiss('cancel');
    };

    function consultarCliente() {
        addLoader();
        clienteService.consultar().then(function (response) {
            $scope.Clientes = response.data;
            removeLoader();
        });
    }

    function consultarGrupo() {
        
        //if ($scope.moduloID != null) {
        //    $scope.codCliente = $scope.moduloID.Codigo;
        //}
        //else {
        //    $scope.codCliente = 0;
        //}
        grupoService.consultar(0).then(function (response) {
            $scope.grupos = response.data;
            //removeLoader();
        });
    }

    function consultarEspecialidades() {
        //if ($scope.moduloID != null) {
        //    $scope.codCliente = $scope.moduloID.Codigo;
        //}
        //else {
        //    $scope.codCliente = 0;
        //}
        especialidadeService.consultar(0).then(function (response) {
            $scope.especialidades = response.data;

        });
    }

    $scope.consultarEstabelecimento = function consultarEstabelecimento(codigo) {
        if (codigo) {
            addLoader();
            estabelecimentoService.consultar(codigo).then(function (response) {
                $scope.estabelecimentos = response.data;
                removeLoader();
            });
        }
    };


    consultarCliente();
    consultarGrupo();
    consultarEspecialidades();

    $scope.abrirModalAlterarSenha = function () {
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalAlterarSenha',
            size: 'md',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {

        });
    }

    $scope.recuperarSenha = function (event) {
        event.preventDefault()

        addLoader();
        usuarioService.recuperarSenha($scope.usuario).then(function (response) {
            removeLoader();
            if (response.data.Success) {
                $scope.$modalInstance.dismiss('cancel');
                sweetAlert("Atenção", "Sua senha foi recuperada com sucesso, favor verifique sua caixa de entrada ;) ", "success");
            }
        });
        event.preventDefault()
    }

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("usuarios.xlsx",{headers:true}) FROM ?', [$scope.usuarios]);
    };

    $scope.$watch('usuario.LoginAd', function (data) {
        if ($scope.usuario.LoginAd)
            $scope.usuario.Login = $scope.LoginDigitado;
        else
            $scope.usuario.Login = $scope.usuario.NumDocumento;
    });

    $scope.salvar = function () {
        
        if ($scope.usuario.Codigo === undefined) {
            addLoader();
            usuarioService.cadastrar($scope.usuario).then(function (response) {
                
                if (response.data) {
                    add($scope.usuarios, response.data);
                    swal("", "Registro salvo com sucesso.", "success");
                    $scope.usuario = response.data;
                }
                removeLoader();

            }, function (error) {

            });
        } else {
            addLoader();
            usuarioService.atualizar($scope.usuario).then(function (response) {
                
                if (response.data) {
                    update($scope.usuarios, response.data);
                    swal("", "Registro salvo com sucesso.", "success");
                    $scope.usuario = response.data;
                }
                removeLoader();

            }, function (error) {

            });
        }
    }

    $scope.obterNomeDoUsuarioLogado = function () {

        usuarioService.obterNomeDoUsuarioLogado().then(function (response) {
            $rootScope.nomeusuario = response.data.nome;
            $scope.estabelecimento = response.data.estabelecimento;
            $rootScope.usuarioMaster = response.data.usuarioMaster;
        });
    }

    $scope.consultar = function () {
        addLoader();
        if ($scope.moduloID != null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }
        usuarioService.consultar($scope.codCliente).then(function (response) {
            $scope.usuarios = response.data;
            removeLoader();
        });
    }

    $scope.abrirModal = function () {
      
        $scope.tituloModal = "Usuário - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalUsuario',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.usuario = {};
            $scope.selecionado = [];
            $scope.contratos = [];
        });
    }
    $scope.limpaUsuarioCliente = function () {
        if ($scope.usuario.Admin) {
            $scope.usuario.AdminCliente = false;
            $scope.usuario.Cliente = 0;
            $scope.usuario.EstabelecimentoSaude = 0;
            $scope.usuario.Especialidade = 0;
        }
        $scope.usuario
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
                usuarioService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.usuarios, response.data);
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
        usuarioService.editar(data).then(function (response) {
            
            
            $scope.usuario = response.data;
            $scope.LoginDigitado = $scope.usuario.Login;
            $scope.consultarEstabelecimento(response.data.Cliente);
            $scope.tituloModal = "Usuário - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalUsuario',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {

            });
        });
        removeLoader();
    }

    //AREA SETOR DO USUARIO ######################################################

    $scope.loadSetorUsuario = function () {
        addLoader();
        setorUsuarioService.consultar($scope.usuario.Codigo).then(function (response) {
            $scope.setoresUsuario = response.data;
            removeLoader();
        });
    };

    $scope.alertaSetor = function (usuario) {
        if ($scope.usuario.Admin || $scope.usuario.AdminCliente) {
            swal("Atenção", "Não é necessário cadastrar Setores de Acesso para este tipo de usuário.", "warning");
        } else if ($scope.usuario.EstabelecimentoSaude && $scope.usuario.Codigo) {
            this.selectedTab = 2;
            this.$parent.loadSetorUsuario();
        } else {
            swal("Atenção", "Salve os dados do formulário antes de continuar.", "warning");
        }
    }

    
    $scope.loadSetor = function () {
        addLoader();
        setorService.ConsultarPorEstabelecimento($scope.usuario.EstabelecimentoSaude).then(function (response) {
            $scope.setores = response.data;
            removeLoader();
        });
    };
    
    $scope.abrirModalSetorUsuario = function () {
        $scope.setorUsuario = {};
        $scope.tituloModal = "Setores de acesso - " + $scope.usuario.EstabelecimentoDesc;
        $scope.$modalInstanceItem = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalSetorUsuario',
            size: 'md',
            scope: $scope
        });

        $scope.$modalInstanceItem.result.then(function () {
        }, function () {
            $scope.setorUsuario = {};
        });
    }

    $scope.fecharModalSetorUsuario = function () {
        $scope.setorUsuario = {};
        $scope.$modalInstanceItem.dismiss('cancel');
    }

    $scope.salvarSetorUsuario = function () {
        addLoader();
        //ALTERAR AQUI
        $scope.setorUsuario.Setor = $scope.setor;
        $scope.setorUsuario.Usuario = $scope.usuario;
        setorUsuarioService.cadastrar($scope.setorUsuario).then(function (response) {
            $scope.setorUsuario = response.data;
            removeLoader();
            add($scope.setoresUsuario, response.data);
            $scope.setorUsuario = {};
            $scope.setor = {};
            $scope.$modalInstanceItem.dismiss('cancel');
        });
    }

    $scope.removeSetorUsuario = function (data) {
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
                setorUsuarioService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.setoresUsuario, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

    //############################################################################
    $scope.detalhar = function (data) {
        addLoader();
        usuarioService.detalhar(data.Codigo).then(function (response) {
            removeLoader();
            $scope.usuDetalhe = response.data;

            $scope.abrirModalDetalhe();
        });
    }

    $scope.abrirModalDetalhe = function () {
        $scope.tituloModalDetalhe = "Detalhe Usuário";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: '../globalsys/views/seguranca/meusDados.html',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.usuDetalhe = {};
        });
    }

    $scope.meusDados = function () {
        addLoader();
        usuarioService.meusDados().then(function (response) {
            removeLoader();
            $scope.usuDetalhe = response.data;
            $scope.abrirModalDetalhe();
        });
    }

    function getItem(lista, item) {
        var selecionado = {};
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].Codigo === item)
                selecionado = lista[i];
        }
        return selecionado;
    }

    $scope.toggleSelection = function toggleSelection(selecionado) {
        var idx = $scope.selecionado.indexOf(selecionado);
        if (idx > -1) {
            $scope.selecionado.splice(idx, 1);
        }
        else {
            $scope.selecionado.push(selecionado);
        }
    };

}]);