'use strict';
Globalsys.controller('solicitacaoEquipamentoController', ['$scope', 'regimeService', 'centroDeCustoService', 'areasService', 'tipoEquipamentoService', 'solicitacaoEquipamentoService', '$uibModal', '$timeout', function ($scope, regimeService, centroDeCustoService, areasService, tipoEquipamentoService, solicitacaoEquipamentoService, $uibModal, $timeout) {
    $scope.registros = [];
    $scope.centrosdecusto = [];
    $scope.TipoEquipamentos = [];
    $scope.areas = [];
    $scope.regimes = [];
    $scope.registro = {};
    $scope.tituloModal = "";
    $scope.HoraInicio = "";
    $scope.HoraFim = "";
    $scope.visibilityDiv = false;
    $scope.implementoSelecionado = "";

    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("registros.xlsx",{headers:true}) FROM ?', [$scope.registros]);
    };


    $scope.$watch('registro.TipoEquipamento', function (data) {
        if (data != null && data != "") {
            $scope.visibilityDiv = data.Implementos != undefined ? data.Implementos.length > 0 : false;
        }
    });

    $scope.datePickerDataInicio = {
        dateOptions: {
            formatYear: 'yy',
            startingDay: 1
        },
        format: 'dd/MM/yyyy',
        opened: false
    };

    $scope.datePickerDataFim = {
        dateOptions: {
            formatYear: 'yy',
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

    $scope.openDataFim = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $timeout(function () {
            $scope.datePickerDataFim.opened = !$scope.datePickerDataFim.opened;
        });

    };

    $scope.loadTipoEquipamento = function () {
        if ($scope.TipoEquipamentos.length <= 0) {
            tipoEquipamentoService.consultar().then(function (response) {
                $scope.TipoEquipamentos = response.data;
            });
        }
    }

    $scope.loadRegime = function () {
        regimeService.consultar().then(function (response) {
            $scope.regimes = response.data;
        });
    }

    $scope.loadCentroCusto = function () {
        if ($scope.centrosdecusto.length <= 0) {
            centroDeCustoService.consultar().then(function (response) {
                $scope.centrosdecusto = response.data;
            });
        }
    }

    $scope.loadImplementos = function (tipoEquipamento) {

    }

    $scope.loadAreas = function () {
        if ($scope.areas.length <= 0) {
            areasService.consultar().then(function (response) {
                $scope.areas = response.data;
            });
        }
    }

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    $scope.salvar = function () {
        if ($scope.registro.Codigo == undefined) {

            var impl = { Codigo: $scope.registro.implementoSelecionado };
            $scope.registro.Implemento = impl;

            if ($scope.registro.TipoEquipamento != undefined)
                $scope.registro.TipoEquipamento.Imagem = "";
            addLoader();
            solicitacaoEquipamentoService.cadastrar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.registros, response.data);
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');

                }
            }, function (error) {

            });
        } else {
            addLoader();
            solicitacaoEquipamentoService.atualizar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.registros, response.data);
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    };

    $scope.montaObjeto = function () {
        $scope.implementosSelecionados = []
        for (var i = 0; i < $scope.selecionado.length; i++) {
            var impl = { Codigo: $scope.selecionado[i] };
            var TipoEquipamentoImplemento = { Implemento: impl };
            $scope.implementosSelecionados.push(TipoEquipamentoImplemento);
        }
        $scope.tipoEquipamento.Implementos = $scope.implementosSelecionados;
    }

    $scope.consultar = function () {
        addLoader();
        solicitacaoEquipamentoService.consultar().then(function (response) {
            $scope.registros = response.data;
            removeLoader()
        });
    };

    $scope.abrirModal = function () {
        $scope.tituloModal = "Solicitação Equipamento - Novo";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalSolicitacaoEquipamento',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.registro = {};
        });
    };

    $scope.deletar = function (data) {
        swal({
            title: "Atenção",
            text: "Você tem certeza que gostaria de cancelar essa solicitação?",
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
                solicitacaoEquipamentoService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        update($scope.registros, response.data);
                    }
                });

            }
        });
    }
    
    $scope.detalhar = function (data) {
        addLoader();
        solicitacaoEquipamentoService.detalhar(data).then(function (response) {
            removeLoader();
            $scope.registro = response.data;
            $scope.carregarCssStatus(response.data.ListaAlocacao);

            $scope.abrirModalDetalhe();

            //$scope.tituloModal = "Solicitação Equipamento - Editar";
            //$scope.$modalInstance = $uibModal.open({
            //    animation: $scope.animationsEnabled,
            //    templateUrl: 'modalSolicitacaoEquipamento',
            //    scope: $scope

            //});
            //$scope.$modalInstance.result.then(function () {
            //}, function () {
            //    $scope.registro = {};
            //});
        });
    }

    $scope.carregarCssStatus = function (ListaAlocacao) {

        for (var i = 0; i < ListaAlocacao.length; i++) {
            var css = solicitacaoEquipamentoService.getCssStatus(ListaAlocacao[i].UltimoStatus);
            //var status = String.format(css, ListaStatus[i].Status);
            //ListaStatus[i].Status = status;
            ListaAlocacao[i].StatusLinha = css;
        }
    }

    $scope.abrirModalDetalhe = function () {
        $scope.tituloModalDetalhe = "Detalhe Solicitação";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalDetalhe',
            //templateUrl: "../globalsys/views/oilegas/detalhesolicitacaoequipamento.html",
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.registro = {};
        });
    }

    $scope.executeScript = function() {
        var acc = document.getElementsByClassName("accordion ng-binding");
        var i;

        for (i = 0; i < acc.length; i++) {
            acc[i].onclick = function () {
                /* Toggle between adding and removing the "active" class,
                to highlight the button that controls the panel */
                this.classList.toggle("active");

                /* Toggle between hiding and showing the active panel */
                var panel = this.nextElementSibling;
                if (panel.style.display === "block") {
                    panel.style.display = "none";
                } else {
                    panel.style.display = "block";
                }
            }
        }
    }
   
}]);