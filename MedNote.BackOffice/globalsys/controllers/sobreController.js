'use strict';
Globalsys.controller('sobreController', ['$scope', '$uibModal', function ($scope, $uibModal) {

    $scope.showModal = function () {

        $scope.$modalInstance = $uibModal.open({
            animation: $scope.animationsEnabled,
            templateUrl: '../app/views/sobre.html',
            scope: $scope
        });
        $scope.$modalInstance.result.then(function () {
        }, function () {
            $scope.registro = {};
        });
    }

    $scope.hideModal = function(){
        $scope.$modalInstance.dismiss('cancel');
    }

}]);