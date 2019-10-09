'use strict';
Globalsys.controller('dispositivoMasterController', ['$scope', '$rootScope', 'dispositivoService', 'admissaoService', '$uibModal', '$timeout', 'utilService', '$location', '$stateParams', '$filter', function ($scope, $rootScope, dispositivoService, admissaoService, $uibModal, $timeout, utilService, $location, $stateParams, $filter) {
    function Init() {
        addLoader();

        //:::::::::::::::::::::::::::::::::::::::::::Rotas::::::::::::::::::::::::::::::::::::::::::::::::

        $scope.internacao = $stateParams.id;

        $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));
        if ($scope.moduloID !== null) {
            $scope.cliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.cliente = 0;
        }

        $scope.abrirMasterAdmissao = function () {
            $location.path('/app/admissao/master/');
        };

        $scope.abrirMasterDispositivo = function () {
            $location.path('/app/dispositivo/master/');
        };


        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

        


        


        //:::::::::::::::::::::::::::::::::::::::::::Formulário ::::::::::::::::::::::::::::::::::::::::::::::::
        $scope.consultarcategoria = function () {
            dispositivoService.consultarcategoria().then(function (response) {
                $scope.categorias = response.data;
            });
        };
        $scope.consultarcategoria();

        $scope.consultartipo = function (data) {
            dispositivoService.consultartipo(data.Codigo).then(function (response) {
                $scope.tiposDispositivos = response.data;
            });
        };

        $scope.consultarsitio = function (data) {
            dispositivoService.consultarsitio(data.Codigo).then(function (response) {
                $scope.sitios = response.data;
            });
        };

        $scope.consultarlateralidade = function () {
            dispositivoService.consultarlateralidade().then(function (response) {
                $scope.lateralidades = response.data;
            });
        };
        $scope.consultarlateralidade();

        $scope.openDataImplante = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $timeout(function () {
                $scope.datePickerDataImplante.opened = !$scope.datePickerDataImplante.opened;
            });
        };
        $scope.datePickerDataImplante = {
            dateOptions: { formatYear: 'yyyy', startingDay: 1 },
            format: 'dd/MM/yyyy HH:mm',
            opened: false
        };

        $scope.validaDataImplante = function (data) {
            var datetimeImplante = $filter('date')(data, "dd/MM/yyyy hh:mm");
            if (datetimeImplante === undefined || $filter('date')($scope.dataAtual, "yyyy-MM-dd") < $filter('date')(data, "yyyy-MM-dd")){
                $scope.dispositivoImplantacao.DataImplante = $scope.mytime;
                swal("Atenção", "Data de Implante Inválida.", "warning");
            }
        };

        $scope.openedRetirada = [];
        $scope.openDataRetirada = function ($event, index) {
            
            var isOpen = $scope.openedRetirada.filter(function (item) { return item === true; })[0];

            if (isOpen === undefined)
            {
                $event.preventDefault();
                $event.stopPropagation();
            }

            $timeout(function () {
                $scope.openedRetirada[index] = true;
            });
        }; 

        $scope.dateCurrent = function () {
            $scope.dataAtual = new Date();
        };
        $scope.dateCurrent();

        var today = new Date();
        $scope.mytime = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 0, 0, 0);
        $scope.displayTime = 'n/a';

        $scope.$watch('mytime', function () {
            var hour1 = $scope.mytime.getHours() - ($scope.mytime.getHours() >= 12 ? 12 : 0),
                hour = hour1 < 10 ? '0' + hour1 : hour1,
                minutes = ($scope.mytime.getMinutes() < 10 ? '0' : '') + $scope.mytime.getMinutes(),
                period = $scope.mytime.getHours() >= 12 ? 'PM' : 'AM';            
            $scope.displayTime = hour + ':' + minutes + ' ' + period;
        });

        $scope.hstep = 1;
        $scope.mstep = 15;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = false;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.mytimeRetirada = new Date();
        $scope.displayTimeRetirada = 'n/a';

        $scope.$watch('mytimeRetirada', function () {
            var hour1Retirada = $scope.mytimeRetirada.getHours() - ($scope.mytimeRetirada.getHours() >= 12 ? 12 : 0),
                hourRetirada = hour1Retirada < 10 ? '0' + hour1Retirada : hour1Retirada,
                minutesRetirada = ($scope.mytimeRetirada.getMinutes() < 10 ? '0' : '') + $scope.mytimeRetirada.getMinutes(),
                periodRetirada = $scope.mytimeRetirada.getHours() >= 12 ? 'PM' : 'AM';
            $scope.displayTimeRetirada = hourRetirada + ':' + minutesRetirada + ' ' + periodRetirada;
        });

        $scope.hstepRetirada = 1;
        $scope.mstepRetirada = 15;

        $scope.optionsRetirada = {
            hstepRetirada: [1, 2, 3],
            mstepRetirada: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridianRetirada = true;
        $scope.toggleModeRetirada = function () {
            $scope.ismeridianRetirada = !$scope.ismeridianRetirada;
        };



        $scope.implantar = function (data) {
            if ($scope.dispositivoImplantacao.Codigo === undefined) {
                addLoader();
                $scope.dispositivoImplantacao.CategoriaDispositivo = data.CategoriaDispositivo.Codigo;
                $scope.dispositivoImplantacao.SitioDispositivo = data.SitioDispositivo.Codigo;
                $scope.dispositivoImplantacao.TipoDispositivo = data.TipoDispositivo.Codigo;
                $scope.dispositivoImplantacao.Lateralidade = data.Lateralidade;
                $scope.dispositivoImplantacao.CodigoInternacao = localStorage.CodigoInternacao;
                $timeout(function () {
                    $scope.dispositivoImplantacao.DataImplante = new Date(data.DataImplante);
                });

                dispositivoService.implantar($scope.dispositivoImplantacao).then(function (response) {
                    removeLoader();
                    if (response.data) {                        
                        response.data.DataImplante = new Date(response.data.DataImplante);
                        add($scope.dispositivos, response.data);
                        $scope.dispositivoImplantacao = {};
                        $scope.cancelDetalhe();
                    }
                }, function () {
                });
            }
        };      

        $scope.editar = function (e, data) {            
            if (e.target.value !== "" && e.target.value.length === 16)
            {
                $scope.dateValid = true;
                $scope.implanteScope = new Date(data.DataImplante);
                var parts = e.target.value.replace(" ", "/").replace(":", "/").split('/');
                $scope.retiradaScope = new Date(parts[2], parts[1] - 1, parts[0], parts[3], parts[4], 0);

                if (parts[3] > "23" || parts[3] < "0" || parts[4] > "59" || parts[4] < "0")
                    $scope.dateValid = false;

                var userTimezoneOffset = $scope.retiradaScope.getTimezoneOffset() * 60000;
                data.DataRetirada = new Date($scope.retiradaScope.getTime() - userTimezoneOffset);
                               
                if ($filter('date')($scope.retiradaScope, "yyyy-MM-dd") === $filter('date')(data.DataImplante, "yyyy-MM-dd")) {
                    var horaRetirada = $scope.retiradaScope.getHours() + "" + $scope.retiradaScope.getMinutes();
                    var horaImplante = $scope.implanteScope.getHours() + "" + $scope.implanteScope.getMinutes();
                    if (horaRetirada < horaImplante)
                        $scope.dateValid = false;
                }
                debugger
                if ($filter('date')($scope.retiradaScope, "dd/MM/yyyy hh:mm") !== undefined && $filter('date')($scope.retiradaScope, "yyyy-MM-dd") >= $filter('date')(data.DataImplante, "yyyy-MM-dd") && $scope.dateValid) {
                    dispositivoService.atualizardispositivointernacao(data.Codigo, data).then(function (response) {
                        removeLoader();
                        if (response.data) {
                            $scope.dispositivoImplantacao = response.data;
                            var list = [response.data];     
                            update($scope.dispositivos, getItem(getNewDataRetirada(list), response.data.Codigo));
                        }
                    }, function () {
                    });
                }
                else
                {
                    dispositivoService.consultardispositivointernacao(localStorage.CodigoInternacao).then(function (response) {
                        if (response.data) {
                            $scope.oldValue = getItem(response.data, data.Codigo);
                            $scope.oldValue.DataRetirada = $scope.oldValue.DataRetirada !== null ? new Date($scope.oldValue.DataRetirada) : null;
                            update($scope.dispositivos, $scope.oldValue);
                            swal("Atenção", "Data de Retirada Inválida.", "warning");
                        }
                    }, function () {
                    });
                }
            }
            else
            {
                data.DataRetirada = "";
                dispositivoService.atualizardispositivointernacao(data.Codigo, data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        $scope.dispositivoImplantacao = response.data;
                        update($scope.dispositivos, response.data);
                    }
                }, function () {
                });
            }            
        };

        $scope.deletar = function (data) {
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
                    dispositivoService.deletardispositivointernacao(data).then(function (response) {
                        removeLoader();
                        if (response.data) {
                            remover($scope.dispositivos, response.data);
                            swal("", "Registro excluído com sucesso.", "success");
                        }
                    });
                } else {
                    swal("Atenção", "Ação cancelada.", "success");
                }
            });
        };

        function getItem(lista, item) {
            var selecionado = {};
            for (var i = 0; i < lista.length; i++) {
                if (lista[i].Codigo === item)
                    selecionado = lista[i];
            }
            return selecionado;
        }

        function getNewDataRetirada(lista) {
            for (var i = 0; i < lista.length; i++) {
                if (lista[i].DataRetirada !== undefined && lista[i].DataRetirada !== null)
                    lista[i].DataRetirada = new Date(lista[i].DataRetirada);
            }
            return lista;
        }
        

        //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::




        $scope.abrirModalDispositivo = function () {
            $scope.dispositivoImplantacao = {};
            $scope.dispositivoImplantacao.DataImplante = $scope.mytime;
            $scope.tituloModal = "Implantar Novo Dispositivo";
            $scope.$modalInstanceDetalhe = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalDispositivoDetails',
                size: 'md',
                scope: $scope
            });
            $scope.$modalInstanceDetalhe.result.then(function () {
            }, function () {
                $scope.dispositivoImplantacao = {};
            });
        };

        if (localStorage) {
            $scope.tituloModal = "MedNote - " + localStorage.NomeEstabelecimento + " - " + localStorage.NomeSetor;
            $scope.leitoModal = "Leito: " + localStorage.Identificador + " - ";
            $scope.pacienteModal = "Novo";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalDispositivo',
                windowClass: 'app-modal-window',
                scope: $scope
            });
            $scope.$modalInstance.result.catch(function () {
                if ($location.$$path === '/app/dispositivo/master/' || $location.$$path === '/app/painel')
                    $location.path('/app/painel');
                else
                    $location.path($location.$$path);
            }, function () {
                $scope.dispositivo = {};
            });
            angular.element("#editor").summernote("disable");

            if (localStorage.CodigoInternacao != 0) {
                addLoader();
                dispositivoService.consultardispositivointernacao(localStorage.CodigoInternacao)
                    .then(function (response) {
                        $scope.dispositivos = getNewDataRetirada(response.data);                
                    }, function () {
                    }).finally(function () {
                        removeLoader();
                    });
                if (!$scope.dispositivos)
                {
                    admissaoService.consultarPacienteInternacao(localStorage.CodigoInternacao)
                        .then(function (response) {
                            $scope.pacienteModal = response.data.Nome;
                        }, function () {
                        }).finally(function () { });
                }
            } else {
                $scope.admissao.Codigo = 0;
                $scope.admissao.CodigoLeito = localStorage.CodigoLeito;
                $scope.admissao.CodigoSetor = localStorage.CodigoSetor;
            }
        }

        $scope.cancel = function () {
            $location.path('/app/painel');
        };

        $scope.cancelDetalhe = function () {
            $scope.$modalInstanceDetalhe.dismiss('cancel');
        };   
    }

    Init();

}]);