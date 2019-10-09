'use strict';
Globalsys.controller('clienteController', ['$scope', 'clienteService', 'estabelecimentoService', 'tipoSetorClienteService', 'tipoSetorService', '$uibModal', '$timeout', 'utilService', function ($scope, clienteService, estabelecimentoService, tipoSetorClienteService, tipoSetorService, $uibModal, $timeout, utilService) {
    $scope.Clientes = [];
    $scope.Cliente = {};
    $scope.estabelecimentos = [];
    $scope.estabelecimento = { Editar: false };
    //Do cliente
    $scope.tiposDeSetor = [];
    $scope.tipoDeSetor = {};
    //Do cadastro
    $scope.tipoSetores = [];
    $scope.tipoSetor = {};
    
    $scope.estabelecimentoOld = {};
    $scope.estabelecimentosAdicionados = [];
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
        $scope.Cliente = {};
        $scope.estabelecimentos = [];
        $scope.estabelecimento = { Editar: false };
        //Do cliente
        $scope.tiposDeSetor = [];
        $scope.tipoDeSetor = {};
        //Do cadastro
        $scope.tipoSetores = [];
        $scope.tipoSetor = {};
    };

    //$scope.exportData = function () {
    //    alasql('SELECT * INTO XLSX("Clientes.xlsx",{headers:true}) FROM ?', [$scope.Clientes]);
    //};

    $scope.today = function () {
        $scope.dt = new Date();
    };
    $scope.today();

    $scope.openDataFim = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $timeout(function () {
            $scope.datePickerDataFim.opened = !$scope.datePickerDataFim.opened;
        });
    };

    $scope.datePickerDataFim = {
        dateOptions: {
            formatYear: 'yyyy',
            startingDay: 1
        },
        format: 'dd/MM/yyyy',
        opened: false
    };

    $scope.openDataInicio = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $timeout(function () {
            $scope.datePickerDataInicio.opened = !$scope.datePickerDataInicio.opened;
        });
    };

    $scope.datePickerDataInicio = {
        dateOptions: {
            formatYear: 'yyyy',
            startingDay: 1
        },
        format: 'dd/MM/yyyy',
        opened: false
    };

    $scope.abrirModal = function () {
        $scope.Cliente = {};
        $scope.tituloModal = "Cliente - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalCliente',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.Cliente = {};
        });
    }

    $scope.salvar = function () {
        debugger
        if ($scope.Cliente.DataInicioContrato >= $scope.Cliente.DataTerminoContrato) {
            swal({
                title: "Atenção",
                text: "Data de início do contrato deve ser menor que a data Fim.",
                type: "warning",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ok",
                closeOnConfirm: true
            });
        } else if ($scope.estabelecimentos.length == 0) {
            swal({
                title: "Atenção",
                text: "Ao menos um Estabelecimento de Saúde deve ser cadastrado.",
                type: "warning",
                showCancelButton: false,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Ok",
                closeOnConfirm: true
            });
        } else {
            $scope.Cliente.Estabelecimento = $scope.estabelecimentos;
            if ($scope.Cliente.Codigo == undefined) {
                addLoader();
                $scope.estabelecimento.Editar = false;
                clienteService.cadastrar($scope.Cliente).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        add($scope.Clientes, response.data);
                        $scope.Cliente = {};
                        $scope.$modalInstance.dismiss('cancel');

                    }
                }, function (error) {

                });
            } else {
                addLoader();

                clienteService.atualizar($scope.Cliente).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        update($scope.Clientes, response.data);
                        $scope.Cliente = {};
                        $scope.$modalInstance.dismiss('cancel');
                    }

                }, function (error) {

                });
            }
        }
    }

    consultar();

    function consultar() {
        addLoader();
        clienteService.consultar().then(function (response) {
            $scope.Clientes = response.data;
            removeLoader()
        });
    }

    function consultarestabelecimento(data) {
        addLoader();
        estabelecimentoService.ConsultarEstabelecimentoPorCliente(data).then(function (response) {
            $scope.estabelecimentos = response.data;
            removeLoader()
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

    $scope.editar = function (data) {
        addLoader();

        clienteService.editar(data).then(function (response) {
            removeLoader();
            $scope.Cliente = response.data;
            $scope.estabelecimentos = response.data.Estabelecimento;

            $timeout(function () {
                $scope.Cliente.DataInicioContrato = new Date(response.data.DataInicioContrato);
                $scope.Cliente.DataTerminoContrato = new Date(response.data.DataTerminoContrato);
            });
            consultarTiposSetor($scope.Cliente.Codigo);
           // consultarestabelecimento($scope.Cliente.Codigo);
            $scope.tituloModal = "Cliente - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalCliente',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.Cliente = {};
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
                clienteService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.Clientes, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

    //dados dos estabelecimentos
    $scope.abrirModalEst = function () {
        $scope.estabelecimento = {};
        $scope.tituloModal = "Estabelecimento - Novo";
        $scope.$modalInstanceItem = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalEstabelecimento',
            size: 'md',
            scope: $scope
        });

        $scope.$modalInstanceItem.result.then(function () {
        }, function () {
            $scope.estabelecimento = {};
        });
    }

    $scope.fecharModalEst = function () {
        $scope.estabelecimento.Editar = false;
        $scope.$modalInstanceItem.dismiss('cancel');
    }

    //AREA TIPO SETOR DO CLIENTE ######################################################
    function consultarTiposSetor(data) {

        addLoader();
        tipoSetorClienteService.ConsultarTipoSetorPorCliente(data).then(function (response) {
            $scope.tiposDeSetor = response.data;
            removeLoader()
        });
    }

    $scope.loadTipoSetor = function () {

        tipoSetorService.consultar(true).then(function (response) {
            $scope.tipoSetores = response.data;
            removeLoader();
        });
    };

    function alertaTrocaMenu() {
        swal("Atenção", "Salve os dados do formulário antes de continuar.", "warning");
    }

    $scope.carregaTipoSetor = function (data) {

        if ($scope.Cliente.Codigo) {
            //consultarTiposSetor($scope.Cliente.Codigo);
        } else {
            alertaTrocaMenu();
        }
    };
    
    $scope.abrirModalTipoSet = function () {
        $scope.tipoDeSetor = {};
        $scope.tituloModal = "Tipo de Setor - Novo";
        $scope.$modalInstanceItem = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalTipoSetor',
            size: 'md',
            scope: $scope
        });

        $scope.$modalInstanceItem.result.then(function () {
        }, function () {
            $scope.estabelecimento = {};
        });
    }

    $scope.fecharModalSet = function () {
        $scope.tipoSetor = {};
        $scope.$modalInstanceItem.dismiss('cancel');
    }

    $scope.salvarItemSet = function () {
        addLoader();

        $scope.tipoDeSetor.TipoSetor = $scope.tipoSetor;
        $scope.tipoDeSetor.Cliente = $scope.Cliente;
        tipoSetorClienteService.cadastrar($scope.tipoDeSetor).then(function (response) {
            $scope.tipoSetorCliente = response.data;
            removeLoader();
            add($scope.tiposDeSetor, response.data);
            $scope.tipoSetorCliente = {};
            $scope.tipoSetor = {};
            $scope.$modalInstanceItem.dismiss('cancel');
        });
    }

    $scope.removeTipoSetor = function (data) {
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
                tipoSetorClienteService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.tiposDeSetor, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

    //############################################################################

    function update_item(arr, data) {
        var item = {};
        if (arr.length > 0) {
            for (var index = 0; index < arr.length; index++) {
                if (arr[index].Codigo == data.Codigo) {
                    arr[index] = data;
                }
            }
        }
        return item;
    }


    $scope.salvarItem = function () {
        
        if ($scope.Cliente.Codigo != undefined) {
            
            $scope.estabelecimento.Editar = true;
            removeLoader();
            if ($scope.estabelecimento.Codigo == undefined) {
                if ($scope.estabelecimentoOld != undefined && $scope.estabelecimentoOld != {}) { // Editar
                    $scope.estabelecimentos = $scope.estabelecimentos.filter(est => est.Nome != $scope.estabelecimentoOld.Nome);
                    $scope.estabelecimentos.push($scope.estabelecimento);

                    $scope.estabelecimento = {};
                    $scope.estabelecimentoOld = {};
                    $scope.$modalInstanceItem.dismiss('cancel');
                }
                else { // Novo
                    addItem($scope.estabelecimentos, $scope.estabelecimento);
                    $scope.estabelecimento = {};
                    $scope.$modalInstanceItem.dismiss('cancel');
                }
            }
            else {
                $timeout(function () {
                    update_item($scope.estabelecimentos, $scope.estabelecimento);
                    $scope.estabelecimento = {};
                    $scope.$modalInstanceItem.dismiss('cancel');
                });
            }
        } else {
            
            $timeout(function () {
                // if (existItem($scope.estabelecimentos, $scope.estabelecimento)) {
                //     sweetAlert("Atenção", "Item  já contido na lista", "warning");
                // }
                if ($scope.estabelecimentoOld != undefined && $scope.estabelecimentoOld != {}) { // Editar
                    $scope.estabelecimentos = $scope.estabelecimentos.filter(est => est.Nome != $scope.estabelecimentoOld.Nome);
                    $scope.estabelecimentos.push($scope.estabelecimento);

                    $scope.estabelecimento = {};
                    $scope.estabelecimentoOld = {};
                    $scope.$modalInstanceItem.dismiss('cancel');
                }
                else {
                    
                    $scope.estabelecimento.Editar = false;
                    addItem($scope.estabelecimentos, $scope.estabelecimento);
                    $scope.estabelecimento = {};
                    $scope.$modalInstanceItem.dismiss('cancel');
                }

            });
        }
    }

    function addItem(arr, data) {
        var item = {};
        if (!existItem(arr, data)) {
            arr.push(data);
        }
        return item;
    }

    function existItem(arr, data) {
        var exist = false;
        if (arr.length > 0) {
            for (var index = 0; index < arr.length; index++) {
                if (arr[index].Codigo === data.Codigo) {
                    exist = true;
                }
            }
        }
        return exist;
    }

    function removeItem(arr, item) {
        for (var i = arr.length; i--;) {
            if (arr[i].Nome === item.Nome) {
                arr.splice(i, 1);
            }
        }
    }

    $scope.removeItemTable = function (data) {
        if (data.Codigo != undefined) {
            addLoader();
            estabelecimentoService.deletar(data).then(function (response) {
                removeLoader();
                if (response.data) {
                    removeItem($scope.estabelecimentos, response.data);
                }
            });
        } else {
            removeItem($scope.estabelecimentos, data);
        }
    }

    $scope.editar_item = function (data) {
        //addLoader();
        
        $scope.estabelecimento = data;
        $scope.estabelecimentoOld = data;
        $scope.estabelecimento.Editar = true;
        if ($scope.estabelecimento.Codigo == undefined) {
            $scope.estabelecimento.Editar = false;
            $scope.tituloModalItem = "Estabelecimento - Editar";
            $scope.$modalInstanceItem = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalEstabelecimento',
                size: 'md',
                scope: $scope
            });
            $scope.$modalInstanceItem.result.then(function () {
            }, function () {
                $scope.estabelecimento = {};
            });
        }
        else {
            estabelecimentoService.editar(data).then(function (response) {
                removeLoader();
                $scope.estabelecimento = response.data;

                $scope.tituloModalItem = "Estabelecimento - Editar";
                $scope.$modalInstanceItem = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: 'modalEstabelecimento',
                    size: 'md',
                    scope: $scope
                });
                $scope.$modalInstanceItem.result.then(function () {
                }, function () {
                    $scope.estabelecimento = {};
                });
            });
        };
    }
}]);