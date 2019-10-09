'use strict';
Globalsys.controller('especialidadeController', ['$scope', 'especialidadeService', 'clienteService', 'dadosGeraisService', '$uibModal', '$timeout', 'utilService', function ($scope, especialidadeService, clienteService, dadosGeraisService, $uibModal, $timeout, utilService) {
    $scope.especialidades = [];
    $scope.especialidade = {};
    $scope.clientes = [];
    $scope.tituloModal = "";
    $scope.currentPage = 1;
    $scope.pageSize = 10;
    $scope.moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    $scope.loadCliente = function () {
        
        clienteService.consultar().then(function (response) {
            $scope.clientes = response.data;
        });
    }


    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("especialidades.xlsx",{headers:true}) FROM ?', [$scope.especialidades]);
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
        $scope.especialidade = {};
        
       
        $scope.tituloModal = "Especialidade - Nova";
        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: 'modalEspecialidade',
            scope: $scope
        });

        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.especialidade = {};
        });
    }

    $scope.salvar = function () {
        
        $scope.especialidade.Cliente = $scope.moduloID.Codigo;
        $scope.especialidade.Cliente = { Codigo: $scope.especialidade.Cliente };
        if ($scope.especialidade.Codigo == undefined) {
            addLoader();
            especialidadeService.cadastrar($scope.especialidade).then(function (response) {
                removeLoader();
                if (response.data) {
                    add($scope.especialidades, response.data);
                    $scope.especialidade = {};
                    $scope.$modalInstance.dismiss('cancel');

                }
            }, function (error) {

            });
        } else {
            addLoader();
            especialidadeService.atualizar($scope.especialidade).then(function (response) {
                removeLoader();
                if (response.data) {
                    update($scope.especialidades, response.data);
                    $scope.especialidade = {};
                    $scope.$modalInstance.dismiss('cancel');
                }

            }, function (error) {

            });
        }
    }

    consultar();
    function consultar() {
        addLoader();
        if ($scope.moduloID != null) {
            $scope.codCliente = $scope.moduloID.Codigo;
        }
        else {
            $scope.codCliente = 0;
        }

        especialidadeService.consultar($scope.codCliente).then(function (response) {
            $scope.especialidades = response.data;
            removeLoader();
        });
    }
   
    $scope.editar = function (data) {
        addLoader();
        especialidadeService.editar(data).then(function (response) {
            removeLoader();
            $scope.especialidade = response.data;
            $scope.tituloModal = "Especialidade - Editar";
            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalEspecialidade',
                scope: $scope

            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.especialidade = {};
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
            confirmButtonText: "Sim.",
            cancelButtonText: "Não",
            closeOnConfirm: false,
            closeOnCancel: false
        }, function (isConfirm) {
            if (isConfirm) {
                addLoader();
                especialidadeService.deletar(data).then(function (response) {
                    removeLoader();
                    if (response.data) {
                        remover($scope.especialidades, response.data);
                        swal("", "Registro removido com Sucesso.", "success");
                    }
                });

            } else {
                swal("Atenção", "Ação cancelada.", "success");
            }
        });
    }

}]);