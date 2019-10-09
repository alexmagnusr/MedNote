'use strict';
Globalsys.controller('trocarSenhaController', ['$scope', '$uibModal', 'usuarioService', function ($scope, $uibModal, usuarioService) {
    $scope.usuDetalhe = {};

    $scope.ObterDadosUsuario = function () {
        usuarioService.obterUsuarioLogado().then(function (response) {
            $scope.usuDetalhe = response.data;
        }, function (error) {

        });
    }

    $scope.showModal = function () {

        usuarioService.obterUsuarioLogado().then(function (response) {
            $scope.usuDetalhe = response.data;
            if (!$scope.usuDetalhe.LoginAd) {
                $scope.$modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: '../app/views/trocasenha.html',
                    scope: $scope
                });
                $scope.$modalInstance.result.then(function () {
                }, function () {
                    $scope.registro = {};
                });
            } else {
                swal("Advertência", "Não é possível alterar sua senha por este sistema", "warning");
            }
        }, function (error) {

        });
    }

    $scope.hideModal = function () {
        $scope.$modalInstance.dismiss('cancel');
    }

    $scope.AlterarSenha = function () {
        if ($scope.validaSenha()) {
            addLoader();
            usuarioService.trocarSenha($scope.usuDetalhe).then(function (response) {
                removeLoader();
                swal("Sucesso", "Operação realizada com sucesso!", "success");
                $scope.cancel();
            }, function (error) {
                removeLoader();
            });
        }
    }

    $scope.validaSenha = function () {
        if ($scope.usuDetalhe.SenhaNova != $scope.usuDetalhe.ConfirmacaoSenha) {
            swal("Erro", "Os campos senha e confirmação de senha não conferem.", "error");
            return false;
        }
        else {
            return true;
        }
    }

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

}]);