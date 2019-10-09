'use strict';
Globalsys.controller('admissaoMasterController', ['$scope', '$rootScope', 'admissaoService', '$uibModal', '$timeout', 'utilService', '$location', '$stateParams', function ($scope, $rootScope, admissaoService, $uibModal, $timeout, utilService, $location, $stateParams) {
    //Daqui até a linha "FIM" o código precisa ser identitico em todas as páginas master do menu

    $scope.admissao = {};
    $scope.convenios = [];
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    function Init() {
        if (localStorage) {
            $scope.tituloModal = "MedNote - " + localStorage.NomeEstabelecimento + " - " + localStorage.NomeSetor;
            $scope.leitoModal = "Leito: " + localStorage.Identificador + " - ";
            $scope.pacienteModal = "Novo";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                windowClass: 'app-modal-window',
                templateUrl: 'modalAdmissao',
                scope: $scope
            });
            $scope.$modalInstance.result.catch(function () {
                if ($location.$$path === '/app/admissao/master/' || $location.$$path === '/app/painel')
                    $location.path('/app/painel');
                else
                    $location.path($location.$$path);
            }, function () {
            });
            if (localStorage.CodigoAdmissao !== "0") {
                addLoader();
                admissaoService.editar(localStorage.CodigoAdmissao)
                    .then(function (response) {
                        $scope.admissao = response.data;
                        $scope.admissao.DataNascimento = new Date(response.data.DataNascimento);
                        $scope.pacienteModal = response.data.NomePaciente;
                        localStorage.CodigoInternacao = response.data.CodigoInternacao;
                        $scope.internacao = response.data.CodigoInternacao;
                    }, function () {
                    }).finally(function () {
                        removeLoader();
                    });
            } else {
                $scope.admissao.Codigo = 0;
                $scope.admissao.CodigoLeito = localStorage.CodigoLeito;
                $scope.admissao.CodigoSetor = localStorage.CodigoSetor;
                removeLoader();
            }
        }

        angular.element("#editor").summernote("disable");

        $scope.cancel = function () {
            addLoader();
            $location.path('/app/painel');
        };

        //FIM ====================================================================================

        $scope.paciente = { Codigo: $scope.admissao.CodigoPaciente, Nome: $scope.admissao.NomePaciente };
        $scope.pacientes = [];

        $scope.updateIdade = function () {
            var dtAtual = new Date();
            var anoNasc = $scope.admissao.DataNascimento.getYear();
            var anoNow = dtAtual.getYear();
            var idade = anoNow - anoNasc;
            dtAtual.setYear(idade - 1);
            if (idade > dtAtual.getYear())
                idade--;
            if (idade == -1)
                idade = 0;
            $scope.admissao.Idade = idade;

        };

        $scope.datePickerSetting = {
            dateOptions: {
                formatYear: 'yyyy',
                startingDay: 1
            },
            format: 'dd/MM/yyyy',
            opened: false

        };

        $scope.openDataNascimento = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $timeout(function () {
                $scope.datePickerSetting.opened = !$scope.datePickerSetting.opened;
            });
        };
        $scope.mudaMascaraRG = function () {
            var rg = $scope.admissao.Documento;
            rg  = rg.replace(/[^\dX]/g, "")
            rg = rg.replace(/(\d{2})(\d)/, "$1.$2")
            rg = rg.replace(/(\d{3})(\d)/, "$1.$2")
            rg = rg.replace(/(\d{3})([\dX]{1,2})$/, "$1-$2")

            $scope.admissao.Documento = rg;
        }

        $scope.consultarPacientes = function (nome) {
            if (nome.length > 2) {
                admissaoService.consultarPacientes(nome, $scope.admissao.CodigoSetor).then(function (response) {
                    $scope.pacientes = response.data;
                });
                return $scope.pacientes;
            }
        };

        $scope.setPaciente = function (paciente) {
            this.admissao.CodigoPaciente = paciente.Codigo;
            this.admissao.DataNascimento = new Date(paciente.DataNascimento);
            this.admissao.Documento = paciente.Documento;
            this.admissao.TipoDocumento = paciente.TipoDocumento;
            this.admissao.NumProntuario = paciente.NumProntuario;
            this.admissao.Genero = paciente.Genero;
            this.pacienteModal = paciente.Nome;
            this.updateIdade();
        };
        
        admissaoService.consultarConvenios($scope.moduloID.Codigo).then(function (response) {
            return $scope.convenios = response.data;
        });

        $scope.salvar = function () {
            if ($scope.admissao.Codigo == 0) {
                addLoader();
                admissaoService.cadastrar($scope.admissao).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        $scope.admissao = response.data;
                        $scope.pacienteModal = response.data.NomePaciente;
                        $scope.admissao = response.data;
                        localStorage.CodigoInternacao = response.data.CodigoInternacao;
                        localStorage.CodigoAdmissao = response.data.Codigo;
                        $scope.internacao = response.data.CodigoInternacao;
                        swal("", "Admissão salva com sucesso.", "success");
                    }
                    
                }, function (error) {

                });
            } else {
                addLoader();
                admissaoService.atualizar($scope.admissao).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        $scope.admissao = response.data;
                        $scope.pacienteModal = response.data.NomePaciente;
                        $scope.admissao = response.data;
                        localStorage.CodigoInternacao = response.data.CodigoInternacao;
                        localStorage.CodigoAdmissao = response.data.Codigo;
                        $scope.internacao = response.data.CodigoInternacao;
                        swal("", "Admissão salva com sucesso.", "success");
                    }

                }, function (error) {

                });
            }
        };
    }

    function alertaTrocaMenu() {
        swal("Atenção", "Salve os dados do formulário antes de continuar.", "warning");
    }

    //:::::::::::::::::::::::::::::::::::::::::::Rotas::::::::::::::::::::::::::::::::::::::::::::::::

    $scope.abrirMasterAdmissao = function (data) {
        if ($scope.admissao.CodigoAdmissao)
            $location.path('/app/admissao/master/');
    };


    $scope.abrirMasterDispositivo = function (data) {
        if ($scope.admissao.Codigo != 0) {
            addLoader();
            $location.path('/app/dispositivo/master/');
        } else {
            alertaTrocaMenu();
        }
    };

    //::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

    Init();

}]);