'use strict';
Globalsys.controller('moduloController', ['$rootScope', '$scope', 'moduloService', 'clienteService', 'usuarioService', '$uibModal', '$timeout', '$location', 'menuService', function ($rootScope, $scope, moduloService, clienteService, usuarioService, $uibModal, $timeout, $location, menuService) {
    $rootScope.modulos = [];
    $rootScope.moduloGlobal = {};
    $rootScope.clientes = [];
    $rootScope.usuarioMaster = false;
    $scope.ehMaster = false;

    setMenu();
    function setMenu() {
        var moduloID = JSON.parse(window.localStorage.getItem('moduloID'));
        if (moduloID != null)
            $rootScope.moduloGlobal = parseInt(moduloID.Codigo);
    }
    $scope.update = function (data) {
        
        if (data != null) {
            
            var item_selecionado = JSON.stringify(getItem($rootScope.clientes, { Codigo: data }));
            window.localStorage.setItem('moduloID', item_selecionado);
            var scope = angular.element($("#vixsidebar")).scope();
            window.location.reload(true);
            /* $timeout(function () {
                 menuService.carregarMenu(data).then(function (response) {
                     scope.$parent.menuItems = [];
                     //$rootScope.modulos = response.data;
                     $timeout(function () {
                         scope.$parent.menuItems = response.data;
                     }, 1000);
                 });
 
             }, 0);*/
        }
    }

    $scope.consultarCliente = function () {
        addLoader();
        clienteService.consultar().then(function (response) {
            
            removeLoader();
            $scope.clientes = response.data;

        });
    };

    if ($scope.ehMaster)
        $scope.consultarCliente();

    $scope.usuarioMaster = function () {
        usuarioService.obterNomeDoUsuarioLogado().then(function (response) {
            $rootScope.usuarioMaster = response.data.usuarioMaster;
            $scope.ehMaster = response.data.usuarioMaster;
        });
    };
  

    function setDefault() {
        
        var moduloID = JSON.parse(window.localStorage.getItem('moduloID'));

        if ($scope.ehMaster) {
            if ($rootScope.clientes.length > 0 && moduloID == null) {
                var item_selecionado = JSON.stringify($rootScope.clientes[0]);
                window.localStorage.setItem('moduloID', item_selecionado);
                setMenu();
            }
        } else {
            usuarioService.ObterClienteUsuario().then(function (response) {
                window.localStorage.setItem('moduloID', JSON.stringify(response.data));
            })
        }
    }
  //  $scope.setDefault();

    $scope.loadModulos = function () {
      
        addLoader();
        moduloService.consultar().then(function (response) {
            $rootScope.modulos = response.data;
            setDefault();
            $scope.usuarioMaster();
            removeLoader();
        });
    }
    
    $scope.loadClientes = function () {
        addLoader();
        clienteService.consultar().then(function (response) {
            $rootScope.clientes = response.data;
            setDefault();
            removeLoader()
        });
    }
    $scope.loadClientes();

    function getItem(lista, item) {
        var selecionado = {};
        for (var i = 0; i < lista.length; i++) {
            if (lista[i].Codigo == item.Codigo)
                selecionado = lista[i];
        }
        return selecionado;
    }
    $scope.selecionarModulo = function (data) {
        if (data != null) {
            window.localStorage.setItem('moduloID', data.Codigo);
            $location.path('/app/singleview');
        }
    }

}]);