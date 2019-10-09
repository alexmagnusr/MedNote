
var Globalsys = angular.module('globalsys', ['angle', 'ngMask', 'base64', 'LocalStorageModule', 'tmh.dynamicLocale', 'ui.utils.masks', 'ui.bootstrap', 'angularUtils.directives.dirPagination', 'notification', 'timepickerPop', 'pascalprecht.translate', 'rw.moneymask']);
Globalsys.directive('datepickerPopup', function (dateFilter, uibDatepickerPopupConfig) {
    return {
        restrict: 'A',
        priority: 1,
        require: 'ngModel',
        link: function (scope, element, attr, ngModel) {

            var dateFormat = attr.datepickerPopup || uibDatepickerPopupConfig.datepickerPopup;
            ngModel.$formatters.push(function (value) {
                return dateFilter(value, dateFormat);
            });
        }
    };
});
Globalsys.directive('expand', function () {
    function link(scope, element, attrs) {
        scope.$on('onExpandAll', function (event, args) {
            scope.expanded = args.expanded;
        });
    }
    return {
        link: link
    };
});
function add(arr, data) {
    var item = {};
    if (!exist(arr, data)) {
        arr.push(data);
    }
    return item;
}

String.format = function () {
    var s = arguments[0];
    for (var i = 0; i < arguments.length - 1; i++) {
        var reg = new RegExp("\\{" + i + "\\}", "gm");
        s = s.replace(reg, arguments[i + 1]);
    }

    return s;
}

function update(arr, data) {    
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
function exist(arr, data) {
    var exist = false;
    if (arr.length > 0) {
        for (var index = 0; index < arr.length; index++) {
            if (arr[index].Codigo == data.Codigo) {
                exist = true;
            }
        }
    }
    return exist;
}
function remover(arr, item) {
    for (var i = arr.length; i--;) {
        if (arr[i].Codigo === item.Codigo) {
            arr.splice(i, 1);
        }
    }
}
function parsleyFieldDirective($timeout) {
    return {
        restrict: 'E',
        require: '^?form',
        link: function (scope, elm, attrs, formController) {
            if (formController != null) {

                if (formController.parsley) {
                    $timeout(function () {
                        // formController.parsley.validate()
                    }, 150); // Need to validate after the data is in the dom.
                }
            }
        }
    };
}

var parsleyOptions = {
    priorityEnabled: false
    // ,
    // errorsWrapper: '<ul class="parsley-error-list"></ul>'

};
Globalsys.directive('parsleyValidate', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        require: '?form',
        link: function (scope, elm, attrs, formController) {
            elm.bind('$destroy', function () {

                formController.parsley.destroy();
            });
            if (!formController.parsley) {


                formController.parsley = new Parsley(elm[0], parsleyOptions);
                // $timeout(function () { formController.parsley.validate() }, 100);
            }

            scope.$on('feedReceived', function () {
                if (!formController.parsley) {
                    formController.parsley = new Parsley(elm[0], parsleyOptions);
                }
                // formController.parsley.validate();
            });
        }
    };
}]);
Globalsys.directive('input', parsleyFieldDirective);
Globalsys.directive('textarea', parsleyFieldDirective);
Globalsys.directive('select', parsleyFieldDirective);
Globalsys.directive('parsleyValidate', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        require: '?form',
        link: function (scope, elm, attrs, formController) {
            elm.bind('$destroy', function () {

                formController.parsley.destroy();
            });
            if (!formController.parsley) {
                formController.parsley = new Parsley(elm[0], parsleyOptions);
                //$timeout(function () { formController.parsley.validate() }, 100);
            }



            scope.$on('feedReceived', function () {
                if (!formController.parsley) {
                    formController.parsley = new Parsley(elm[0], parsleyOptions);
                }
                //formController.parsley.validate();
            });
            if (window.ParsleyValidator)
                window.ParsleyValidator.setLocale('pt-BR');
        }
    };
}]);
Globalsys.directive('filestyle', filestyle);

function filestyle() {
    var directive = {
        link: link,
        restrict: 'A'
    };
    return directive;

    function link(scope, element) {
        var options = element.data();

        // old usage support
        options.classInput = element.data('classinput') || options.classInput;

        element.filestyle(options);
    }
}

Globalsys.directive('hasPermission', function (permissionService, $timeout, $compile, $rootScope) {
    return {
        link: function (scope, element, attrs) {
            $rootScope.permissoes = [];
            var controllerName = scope.$resolve.$$controller;
            var objPermisao = {
                Controller: controllerName,
                Actions: []
            }

            if ($rootScope.permissoes.length > 0) {
                verificarPermissao();
            } else {
                /*permissionService.verify(JSON.stringify(objPermisao)).then(function (response) {
                    
                    $rootScope.permissoes.push(response.data);
                    verificarPermissao();
                }, function (error) {

                });*/
            }
            function verificarPermissao() {
                $rootScope.permissoes[0].Actions.forEach(function (item, index, array) {


                    var fn = $compile(angular.element(element).attr('ng-disabled', !item.HasPermission));

                    fn(scope)
                    // $(element.Id).prop('disabled', element.HasPermission);

                }, 2000);
            }
            /*$(':button').prop('disabled', true);
            var objPermisao = {
                Controller: "",
                Actions: []
            }
            var str = attrs.hasPermission.trim();
            var res = [];
            str.replace(/\{(.+?)\}/g, function (_, m) { res.push(m) });
            res.forEach(function (element, index, array) {
                if (index == 0)
                    objPermisao.Controller = element;
                else {
                    var item = { Ref: element.split("|")[0], Id: element.split("|")[1], HasPermission: false };
                    objPermisao.Actions.push(item);
                }
            });

            if (objPermisao.Controller != "") {
                permissionService.verify(JSON.stringify(objPermisao)).then(function (response) {
                    $timeout(function () {
                        response.data.Actions.forEach(function (element, index, array) {
                           
                            
                            var fn = $compile(angular.element($(element.Id)).attr('ng-disabled', !element.HasPermission));
                           // fn(scope)
                           // $(element.Id).prop('disabled', element.HasPermission);
                            
                        }, 2000);

                    });
                }, function (error) {

                });
            }
            */
            /* 
             var matches = attrs.hasPermission.trim().match(/\[(.*?)\]/);
             var aa = attrs.hasPermission.trim().match("\\[.*?]");
             var bb = attrs.hasPermission.trim().match("\\[[^\\]]*]");
             var items = attrs.hasPermission.trim().split(",");
             var controller = angular.element(element).controller().constructor.name;
             */
            /*if (!_.isString(attrs.hasPermission)) {
                throw 'hasPermission value must be a string'
            }
            var value = attrs.hasPermission.trim();
            var notPermissionFlag = value[0] === '!';
            if (notPermissionFlag) {
                value = value.slice(1).trim();
            }

            function toggleVisibilityBasedOnPermission() {
                var hasPermission = permissions.hasPermission(value);
                if (hasPermission && !notPermissionFlag || !hasPermission && notPermissionFlag) {
                    element[0].style.display = 'block';
                }
                else {
                    element[0].style.display = 'none';
                }
            }
            */
            // toggleVisibilityBasedOnPermission();
            // scope.$on('permissionsChanged', toggleVisibilityBasedOnPermission);
        }
    };
});
Globalsys.directive('onFileChange', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var onChangeHandler = scope.$eval(attrs.onFileChange);

            element.bind('change', function () {
                scope.$apply(function () {
                    var files = element[0].files;
                    if (files) {
                        onChangeHandler(files);
                    }
                });
            });

        }
    };
});



Globalsys.config(["$httpProvider", '$translateProvider', "$stateProvider", '$urlRouterProvider', "RouteHelpersProvider", '$controllerProvider', '$compileProvider', '$filterProvider', '$provide', function ($httpProvider, $translateProvider, $stateProvider, $urlRouterProvider, RouteHelpersProvider, $controllerProvider, $compileProvider, $filterProvider, $provide) {
    $httpProvider.interceptors.push('authInterceptorService');
    $urlRouterProvider.otherwise('/app/singleview');

    $stateProvider
        // app routes
        .state('app.global', {
            url: '/global',
            abstract: true,
            template: '<div ui-view="" autoscroll="false" ng-class="app.viewAnimation" class="content-wrapper" style="padding: 0px;"></div>',
            data: {
                requireLogin: true
            } // this property will apply to all children of 'app'
        })
        
        .state('app.sistema-logerro', {
            url: '/logerro',
            title: 'logerro',
            controller: 'logErroController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'usuario', 'localytics.directives'),
            templateUrl: 'globalsys/views/sistema/logerro.html',

        })

        /// SEGURANÇA
        .state('app.seguranca-usuario', {
            url: '/usuario',
            title: 'usuario',
            controller: 'usuarioController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'usuario', 'localytics.directives'),
            templateUrl: 'globalsys/views/seguranca/usuario.html',

        })
        .state('app.seguranca-cliente', {
            url: '/Cliente',
            title: 'Cliente',
            controller: 'clienteController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'usuario', 'localytics.directives'),
            templateUrl: 'globalsys/views/seguranca/cliente.html',

        })
        .state('app.seguranca-especialidade', {
            url: '/especialidade',
            title: 'Especialidade',
            controller: 'especialidadeController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'usuario', 'localytics.directives'),
            templateUrl: 'globalsys/views/seguranca/especialidade.html',

        })
        .state('app.seguranca-tipoSetorCliente', {
            url: '/tipoSetorCliente',
            title: 'TipoSetorCliente',
            controller: 'tipoSetorClienteController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'usuario', 'localytics.directives'),
            templateUrl: 'globalsys/views/seguranca/tipoSetorCliente.html',

        })
        .state('app.seguranca-grupo', {
            url: '/grupo',
            title: 'grupo',
            controller: 'grupoController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'usuario', 'localytics.directives'),
            templateUrl: 'globalsys/views/seguranca/grupo.html',

        })
       
        .state('app.seguranca-permissoes-grupos', {
            url: '/permissoes-grupos',
            title: 'permissoes-grupos',
            controller: 'permissoesController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'usuario'),
            templateUrl: 'globalsys/views/seguranca/permissoes-grupos.html',

        })
        .state('app.seguranca-permissoes-usuarios', {
            url: '/permissoes-usuarios',
            title: 'permissoes-usuarios',
            controller: 'permissoesUsuariosController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'usuario'),
            templateUrl: 'globalsys/views/seguranca/permissoes-usuarios.html',

        })
        .state('app.seguranca-funcao', {
            url: '/funcao',
            title: 'funcao',
            controller: 'funcaoController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley'),
            templateUrl: 'globalsys/views/seguranca/funcao.html',

        })
        .state('app.seguranca-acao', {
            url: '/acao',
            title: 'acao',
            controller: 'acaoController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'localytics.directives'),
            templateUrl: 'globalsys/views/seguranca/acao.html',
        })

        .state('app.seguranca-estabelecimento', {
            url: '/estabelecimento',
            title: 'estabelecimento',
            controller: 'estabelecimentoController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'localytics.directives'),
            templateUrl: 'globalsys/views/seguranca/estabelecimentoSaude.html',
        })

        /// SISTEMA
       
        .state('page', {
            url: '/page',
            templateUrl: 'app/pages/page.html',
            resolve: RouteHelpersProvider.resolveFor('modernizr', 'icons'),
            controller: ['$rootScope', function ($rootScope) {
                $rootScope.app.layout.isBoxed = false;
            }]
        })

        .state('page.login', {
            url: '/login',
            title: 'Login',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'login'),
            templateUrl: 'app/pages/login.html'
        })


        //Cadastros basicos
        .state('app.mednote-tiposetor', {
            url: '/tipoSetor',
            title: 'tipoSetor',
            controller: 'tipoSetorController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'localytics.directives'),
            templateUrl: 'globalsys/views/MedNote/tipoSetor.html',
        })

        .state('app.mednote-setor', {
            url: '/setor',
            title: 'setor',
            controller: 'setorController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'localytics.directives'),
            templateUrl: 'globalsys/views/MedNote/Setor.html',
        })

        .state('app.mednote-parametros-base', {
            url: '/cadastro-basico',
            title: 'ParametrosBase',
            controller: 'parametrosBaseController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'localytics.directives'),
            templateUrl: 'globalsys/views/MedNote/parametrosBase.html',
        })

        .state('app.mednote-painel', {
            url: '/admissao',
            title: 'Setores',
            controller: 'admissaoController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'summernote'),
            templateUrl: 'globalsys/views/MedNote/Admissao.html',
        })

        .state('app.mednote-admissao', {
            url: '/painel',
            title: 'Painel de Internações',
            controller: 'painelController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'summernote'),
            templateUrl: 'globalsys/views/MedNote/Painel.html',
        })

        .state('app.mednote-admissao-master', {
            url: '/admissao/master/:id',
            title: 'Admissao Master',
            controller: 'admissaoMasterController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'summernote'),
            templateUrl: 'globalsys/views/MedNote/Admissao-master.html',
        })

        .state('app.mednote-dispositivo-master', {
            url: '/dispositivo/master/:id',
            title: 'Dispositivo Master',
            controller: 'dispositivoMasterController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'summernote'),
            templateUrl: 'globalsys/views/MedNote/Dispositivo-master.html',
        })

        .state('vix', {
            url: '/vix',
            templateUrl: 'globalsys/views/vix.html',
            resolve: RouteHelpersProvider.resolveFor('modernizr', 'icons'),
            controller: ['$rootScope', function ($rootScope) {
                $rootScope.app.layout.isBoxed = false;
            }]
        })

        .state('vix.modulos', {
            url: '/modulos',
            title: 'modulos',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley'),
            templateUrl: 'globalsys/views/modulos.html',
            controller: 'moduloController'
        })

     
      
        .state('app.setor', {
            url: '/setor',
            title: 'setor',
            controller: 'setorController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'localytics.directives'),
            templateUrl: 'globalsys/views/cadastroinicial/setor.html'
        })
       
        .state('app.trocarSenha', {
            url: '/trocarsenha',
            title: 'trocarsenha',
            controller: 'trocarSenhaController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'localytics.directives', 'moment', 'ui.calendar'),
            templateUrl: 'app/views/trocasenha.html'
        })

        .state('app.meusdados', {
            url: '/meusdados',
            title: 'meusdados',
            controller: 'usuarioController',
            resolve: RouteHelpersProvider.resolveFor('toaster', 'filestyle', 'modernizr', 'icons', 'parsley', 'localytics.directives', 'moment', 'ui.calendar'),
            templateUrl: 'app/views/seguranca/meusDados.html'
        })
        ;
    // initialize get if not there
    if (!$httpProvider.defaults.headers.get) {
        $httpProvider.defaults.headers.get = {};
    }

    // Answer edited to include suggestions from comments
    // because previous version of code introduced browser-related errors

    // disable IE ajax request caching
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Mon, 26 Jul 1997 05:00:00 GMT';
    // extra
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';

}]);

Globalsys.run(["$rootScope", "$translate", "$locale", "$state", "$log", "$injector", '$http', 'AUTHSETTINGS', '$location', 'authService', 'uibDatepickerPopupConfig', 'notificacoesService', function ($rootScope, $translate, $locale, $state, $log, $injector, $http, AUTHSETTINGS, $location, authService, uibDatepickerPopupConfig, notificacoesService) {
    authService.fillAuthData();
    //if (authService.authentication.isAuth)
    //    notificacoesService.initialize();

    //authService.authentication.isAuth
    uibDatepickerPopupConfig.currentText = 'Hoje';
    uibDatepickerPopupConfig.clearText = 'Limpar';
    uibDatepickerPopupConfig.closeText = 'Fechar';
    var isAutenticTeste = false;
    $rootScope.$on('$stateChangeStart', function (event, toState) {
        if (!authService.authentication.isAuth) {
            $location.path('/page/login');
        } else {
            if ($location.$$path == "/page/login") {
                $location.path('/app/singleview');

            }
        }
    });
    //

}]);

