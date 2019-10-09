'use strict';
Globalsys.controller('notificacoesController', ['$scope', 'notificacoesService', '$uibModal', '$timeout', '$notification', '$location', function ($scope, notificacoesService, $uibModal, $timeout, $notification, $location) {
    $scope.listaDeNotificacoesNaoVistas = [];
    $scope.notificacoes = [];
    $scope.showBadge = false;

    $scope.getpermission = function () {
        $notification.requestPermission().then(function (permission) {
            // default, granted, denied
            $scope.showwindowpermission = permission == "default"
                || $notification.getPermission() == "denied" ? true : false;

        });
    }


    $scope.exportData = function () {
        alasql('SELECT * INTO XLSX("notificacoes.xlsx",{headers:true}) FROM ?', [$scope.notificacoes]);
    };

    $scope.inicializarHub = function () {
        $scope.getpermission();
        loadNotificacoes();
    }
    function loadNotificacoes() {
        notificacoesService.qntNotificacoes(false).then(function (response) {
            setQntNotificacos(response.data);
        });
    }
    $scope.showAudio = function () {
        $('<audio id="chatAudio"><source src="../app/audio/notificacao.mp3" type="audio/mpeg"></audio><source src="../app/audio/notificacao.mp3" type="audio/wav"></audio>')
            .appendTo('body');
        $('#chatAudio')[0].play();
    }
    $scope.$on('exibirNotificacao', function (event, message) {
        var notification = $notification('Vix',
            {
                body: message,
                dir: 'auto',
                lang: 'en',
                tag: 'my-tag',
                icon: '../app/img/logo-Vix.png',
                delay: 5000,
                focusWindowOnClick: true
                // focus the window on clicked
            });
        $scope.showAudio();
        var deregister = notification.$on('click',
            function () {
                notification.close();
                document.location.href = message.Url
                    + suburl;
            });
    });
    $scope.doTruncarStr = function (str, size) {
        if (str == undefined || str == 'undefined' || str == '' || size == undefined || size == 'undefined' || size == '') {
            return str;
        }

        var shortText = str;
        if (str.length >= size + 3) {
            shortText = str.substring(0, size).concat('...');
        }
        return shortText;
    }
    $scope.$watch('tipo', function (data) {
        if (data != null && data != "") {
            addLoader();
            notificacoesService.consultar(data).then(function (response) {
                $scope.notificacoes = [];
                $scope.notificacoes = response.data;
                removeLoader()
            });
        }
    });
    function setQntNotificacos(valor) {
        if (valor > 0) {
            $scope.showBadge = true;
            $timeout(function () { $("#badgeNotificacao").attr("data-badge", valor); }, 1000);
        } else {
            $scope.showBadge = false;
            $timeout(function () { $scope.showBadge = false; $("#badgeNotificacao").attr("data-badge", ""); }, 1000);
        }
    }
    $scope.listarTodasAsNotificacoes = function () {
        notificacoesService.consultar(false).then(function (response) {
            $scope.notificacoes = response.data;

        });
    }
    $scope.listarNotificacoes = function () {
        $('#notifacoes-navbar').addClass('loading').loader('show', {
            overlay: true
        });
        notificacoesService.consultar(false).then(function (response) {
            $scope.listaDeNotificacoesNaoVistas = response.data;
            if ($('#notifacoes-navbar').hasClass('loading')) {
                $('#notifacoes-navbar').removeClass('loading').loader('hide');
            }
        });
    }
    $scope.telaNotificacoes = function () {
        $location.path('/app/notificacoes');
    }

    $scope.salvar = function (data) {

        
        addLoader();
        data.Visualizado = true;
        notificacoesService.marcarComoLido(data.Codigo).then(function (response) {
            removeLoader();
            if (response.data) {
                update($scope.notificacoes, response.data);
                $scope.notificacoes = {};
                loadNotificacoes();
            }

        }, function (error) {

        });

    }
}]);