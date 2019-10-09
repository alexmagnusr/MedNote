'use strict';
Globalsys.controller('setorController', ['$scope', '$rootScope', 'setorService', 'estabelecimentoService', 'tipoSetorService', 'tipoSetorClienteService', 'leitoService', 'dadosGeraisService', '$uibModal', '$timeout', 'utilService', '$location', function ($scope, $rootScope, setorService, estabelecimentoService, tipoSetorService, tipoSetorClienteService, leitoService, dadosGeraisService, $uibModal, $timeout, utilService, $location) {
    $scope.setores = [];
    $scope.setor = { Leitos: {} };
    $scope.leitos = [];
    $scope.leito = {};
    $scope.leitoOld = {};
    $scope.tipoSetores = [];
    $scope.estabelecimentos = [];
    $scope.tituloModal = "";
    $scope.currentPage = 1; 
    $scope.pageSize = 10;
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
        $scope.setor = { Leitos: {} };
        $scope.leitos = [];
        $scope.leito = {};
        $scope.leitoOld = {};
        $scope.tipoSetores = [];
        $scope.estabelecimentos = [];
    };

    $scope.loadEstabelecimento = function () {
        var codCliente = $scope.setor.Cliente;
        if (!codCliente)
            codCliente = $scope.moduloID.Codigo;

        estabelecimentoService.consultar(codCliente).then(function (response) {
            $scope.estabelecimentos = response.data;
        });
    }

    $scope.loadTipoSetor = function () {
        var codCliente = $scope.setor.Cliente;
        if (!codCliente)
            codCliente = $scope.moduloID.Codigo;

        tipoSetorClienteService.ConsultarTipoSetorPorCliente(codCliente).then(function (response) {
            $scope.tiposDeSetor = response.data;
        });
    }

    $scope.gerarLeito = function () {
        
        var inicio = 1;
        var fim = 0;

        // se a lista de setores está vazia, inicializa com array.
        if ($scope.setor.Leitos === undefined) $scope.setor.Leitos = [];

        inicio = parseInt($scope.setor.QtdLeitoInicio) || 0;
        fim = parseInt($scope.setor.QtdLeitoFim) || 0;

        if (inicio < 1 || inicio > 99) {
            swal({
                title: "Atenção",
                text: "A quantidade \"De\" deve estar entre 1 e 99.",
                type: "warning",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "OK",
                closeOnConfirm: true
            });
        } else if (fim < 1 || fim > 100) {
            swal({
                title: "Atenção",
                text: "A quantidade \"Até\" deve estar entre 1 e 100.",
                type: "warning",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "OK",
                closeOnConfirm: true
            });
        } else {
            for (var index = inicio; index <= fim; index++) {
                var leito = { Identificador: index, Bl_Liberado: true };
                if ($scope.validaIdentificadoresLeito(leito, $scope.setor.Leitos)) {
                    $scope.setor.Leitos.push(leito);
                }

            }
        }
    }

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("setores.xlsx",{headers:true}) FROM ?', [$scope.setores]);
    };

    $scope.datePickerSetting = {
        dateOptions: {
            formatYear: 'yy',
            startingDay: 1
        },
        format: 'dd/MM/yyyy',
        opened: false
    };

    $scope.abrirModal = function () {
        $scope.setor = {};


        $scope.tituloModal = "Setor - Novo";

        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalSetor',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.setor = {};
        });
    }

    $scope.salvar = function () {
        
        $scope.setor.Estabelecimento = { Codigo: $scope.setor.Estabelecimento };
        $scope.setor.TipoSetor = { Codigo: $scope.setor.TipoSetor };
        var leitosErro = [];

        if ($scope.setor.Codigo == undefined) {
            addLoader();
            setorService.cadastrar($scope.setor).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.setores, response.data);
                    $scope.setor = {};
                    $scope.$modalInstance.dismiss('cancel');
                }
            }, function (error) {

            });
        } else {
            addLoader();
            setorService.atualizar($scope.setor).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.setores, response.data);
                    $scope.setor = {};
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }


    function consultar() {
        
        addLoader();
        if ($scope.moduloID != null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }
        setorService.consultar($scope.codCliente).then(function (response) {
            $scope.setores = response.data;
            $scope.setor.Leitos = response.data.Leitos;
            removeLoader();
        });
    }


    consultar();

    $scope.editar = function (data) {
        addLoader();
        
        setorService.editar(data).then(function (response) {
            removeLoader();
            $scope.setor = response.data;

            //mapeando estas duas propriedades, pois elas não existem na Entidade do banco.
            $scope.setor.QtdLeitoInicio = 1;
            $scope.setor.QtdLeitoFim = $scope.setor.QtdLeitos;

            $timeout(function () {

                if (response.data.Estabelecimento !== null) {
                    //var estabelecimento = $scope.estabelecimentos
                    //    .filter(est => est.Codigo == response.data.Estabelecimentos)[0];
                    
                    $scope.setor.Estabelecimento = data.Estabelecimento;
                }

                if (response.data.TipoSetor !== null) {
                    //var tipoSetor = $scope.setor.TipoSetor = $scope.tipoSetores
                    // .filter(tipoSetor => tipoSetor.Codigo == response.data.TipoSetor)[0];
                    
                    $scope.setor.TipoSetor = data.TipoSetor;
                }
            }, 500);

            $scope.tituloModal = "Setor - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalSetor',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.setor = {};
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
            confirmButtonText: "Sim",
            cancelButtonText: "Não",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {

                addLoader();
                setorService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.setores, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

    $scope.editarLeito = function (data) {
        $scope.leito = data;
        $scope.leitoOld = data;
        $scope.leito.Editar = true;
        if ($scope.leito.Codigo === undefined) {
            $scope.leito.Editar = false;
            $scope.tituloModalItem = "Leito - Editar";
            $scope.$modalInstanceItem = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalLeito',
                size: 'md',
                scope: $scope
            });
            $scope.$modalInstanceItem.result.then(function () {
            }, function () {
                $scope.leito = {};
            });
        }
        else {
            leitoService.editar(data).then(function (response) {
                removeLoader();
                $scope.leito = response.data;

                $scope.tituloModalItem = "Leito - Editar";
                $scope.$modalInstanceItem = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'modalLeito',
                    size: 'md',
                    scope: $scope
                });
                $scope.$modalInstanceItem.result.then(function () {
                }, function () {
                    $scope.leito = {};
                });
            });
        };
    }

    $scope.salvarLeito = function () {

        if ($scope.validaIdentificadoresLeito($scope.leito, $scope.setor.Leitos)) {
            addLoader();
            $scope.leito.Editar = true;
            if ($scope.leito.Codigo !== undefined) {
                $scope.setor.Leitos = $scope.setor.Leitos.filter(leito => leito.Codigo != $scope.leito.Codigo);
                $scope.setor.Leitos.push(
                    {
                        Identificador: $scope.leito.Identificador,
                        Bl_Liberado: $scope.leito.Bl_Liberado,
                        Editar: true,
                        Codigo: $scope.leito.Codigo
                    });

                $scope.leito = {};
                $scope.leitoOld = {};

                removeLoader();
                $scope.$modalInstanceItem.dismiss('cancel');
            } else {

                $scope.setor.Leitos = $scope.setor.Leitos.filter(leito => leito.Identificador != $scope.leitoOld.Identificador);
                $scope.setor.Leitos.push(
                    {
                        Identificador: $scope.leito.Identificador,
                        Bl_Liberado: $scope.leito.Bl_Liberado
                    });

                $scope.leito = {};
                $scope.leitoOld = {};

                removeLoader();
                $scope.$modalInstanceItem.dismiss('cancel');
            }
        }
        else {
            swal({
                title: "Atenção",
                text: "Já existe um leito cadastrado com esse Identificador",
                type: "warning",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "OK",
                closeOnConfirm: true
            });
        }



    }

    $scope.validaIdentificadoresLeito = function(leito, leitos) {
        for (var i = 0; i < leitos.length; i++) {
            if (leito.Identificador == leitos[i].Identificador)
                return false;
        }
        return true;
    }

    $scope.fecharModalLeito = function () {
        $scope.leito.Editar = false;
        $scope.$modalInstanceItem.dismiss('cancel');
    }


    $scope.deletarLeito = function (data) {
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

                if (data.Codigo === undefined) {
                    $timeout(function () {
                        $scope.setor.Leitos = $scope.setor.Leitos.filter(leito => leito.Identificador != data.Identificador);
                    });

                    removeLoader();
                    swal("", "Registro removido com Sucesso.", "success");
                }
                else {
                    leitoService.deletar(data).then(function (response) {
                        removeLoader();
                        if (response.data) {
                            $timeout(function () {
                                $scope.setor.Leitos = $scope.setor.Leitos.filter(leito => leito.Codigo != data.Codigo);
                            });

                            swal("", "Registro removido com Sucesso.", "success");
                        }
                    });
                }
            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }


    $scope.abrirPainelLeitos = function (data) {
        localStorage.NomeEstabelecimento = data.EstabelecimentosNome;
        localStorage.NomeSetor = data.Nome;

        localStorage.CodigoSetor = data.Codigo;
        addLoader();
        $location.path('/app/painel');
    };

}]);