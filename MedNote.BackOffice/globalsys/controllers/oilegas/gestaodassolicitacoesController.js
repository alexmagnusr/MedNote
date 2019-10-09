
Globalsys.controller('gestaodassolicitacoesController', ['$scope', 'gestaodassolicitacoesService', '$uibModal', '$compile', '$timeout', 'solicitacaoEquipamentoService', '$filter', 'tipoEquipamentoService', 'regimeService', 'centroDeCustoService', 'areasService', 'toaster', function ($scope, gestaodassolicitacoesService, $uibModal, $compile, $timeout, solicitacaoEquipamentoService, $filter, tipoEquipamentoService, regimeService, centroDeCustoService, areasService, toaster) {
    $scope.registros = [];
    $scope.dataReferencia = new Date();
    $scope.registro = { Codigo: "" };
    $scope.centrosdecusto = [];
    $scope.TipoEquipamentos = [];
    $scope.TipoEquipamentoSelecionado = [];
    $scope.equipamentos = [];
    $scope.motoristas = [];
    $scope.areas = [];
    $scope.regimes = [];
    $scope.tituloModal = "";
    $scope.solicitacoes = [];
    $scope.disableTipoEquipamento = true;
    $scope.disableImplementos = false;
    $scope.disableEquipamentos = false;
    $scope.disableMotorista = false;
    $scope.disableDescricao = false;
    $scope.hideDescricao = false;
    var sections = [];
    $scope.agendas = [];
    $scope.tiposEquipamentos = [];
    $scope.firsTime = true;

    $scope.textoFiltro = "Teste de Filtro";

    $scope.cancel = function () {
        $scope.$modalInstance.dismiss('cancel');
    };

    $scope.visibilityDiv = false;

    $scope.$watch('registro.TipoEquipamento', function (data) {
        if (data != null && data != "") {
            $scope.visibilityDiv = data.Implementos != undefined ? data.Implementos.length > 0 : false;
        }
    });

    $scope.$watch('dataReferencia', function (data) {
        if (data != null && data != "") {
            $scope.dataReferencia = data;
            dia = data.getDate();
            mes = data.getMonth();
            ano = data.getFullYear();
            if ($scope.firsTime != true) {
                scheduler.setCurrentView(new Date(ano, mes, dia), scheduler._mode);
                $scope.UpdateScheduler(false);
            }

        }
    });


    $scope.datePickerDataInicio = {
        dateOptions: {
            formatYear: 'yy',
            startingDay: 1,
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

    function inveterData(data) {
        var item = data.split("/");
        var dia = item[0];
        var mes = item[1]
        var ano = data.split("/")[2].split(" ")[0];
        var hora = data.split("/")[2].split(" ")[1];
        return String.format("{0}-{1}-{2} {3}", ano, mes, dia, hora);
    }

    function saveAs(blob, fileName) {
        var url = window.URL.createObjectURL(blob);

        var doc = document.createElement("a");
        doc.href = url;
        doc.download = fileName;
        doc.click();
        window.URL.revokeObjectURL(url);
    }

    $scope.FilterByEquipment = function (filter) {
        
        sections = [];
        if (filter != null && filter.length > 0) {
            sections.push({ key: 99999, label: "<b>Serviços </b>" });
            for (var i = 0; i < filter.length; i++) {
                sections.push({ key: filter[i].Codigo, label: "<b>" + filter[i].Descricao + "</b>" });
            }
        } else {
            sections.push({ key: 99999, label: "<b>Serviços </b>" });
            $scope.tiposEquipamentos.forEach(function (valor, index, arr) {
                sections.push({ key: valor.Codigo, label: "<b>" + valor.Descricao + "</b>" });
            });
        }

        $scope.carregarSection();

        scheduler.clearAll();
        scheduler.parse($scope.agendas, "json");

    }

    $scope.init = function () {
        scheduler.clearAll();

        scheduler.locale.labels.timeline_tab = "Timeline";

        scheduler.locale.labels.section_custom = "Section";
        scheduler.config.details_on_create = false;
        scheduler.config.details_on_dblclick = false;
        scheduler.config.multi_day = true;
        scheduler.config.xml_date = "%Y-%m-%d %H:%i";
        scheduler.setLoadMode("week");
        brief_mode = true;

        hoje = $scope.dataReferencia;
        dia = hoje.getDate();
        mes = hoje.getMonth();
        ano = hoje.getFullYear();

        $('#caledario').addClass('loading').loader('show', {
            overlay: true
        });

        $scope.agendas = [];
        $scope.solicitacoes = [];

        gestaodassolicitacoesService.consultar($scope.dataReferencia).then(function (response) {


            $scope.CarregarElementos(response.data);

            scheduler.init('scheduler_here', new Date(ano, mes, dia), "timeline");
            scheduler.parse($scope.agendas, "json");

            if ($('#caledario').hasClass('loading')) {
                $('#caledario').removeClass('loading').loader('hide');
            }

            $scope.firsTime = false;
        });

    }

    var evtChanged = scheduler.attachEvent("onEventChanged", function (id, ev) {
        $scope.AbrirModal(id, ev, false);
    });

    var evtClick = scheduler.attachEvent("onClick", function (id, ev) {
        $scope.AbrirModal(id, ev, true);
    });

    $scope.UpdateScheduler = function (firstTime) {

        $('#caledario').addClass('loading').loader('show', {
            overlay: true
        });
        scheduler.clearAll();
        $scope.agendas = [];
        $scope.solicitacoes = [];

        gestaodassolicitacoesService.consultar($scope.dataReferencia).then(function (response) {

            $scope.CarregarElementos(response.data);

            scheduler.parse($scope.agendas, "json");

            if ($('#caledario').hasClass('loading')) {
                $('#caledario').removeClass('loading').loader('hide');
            }
        });
    }

    $scope.CarregarElementos = function (data) {
        
        $scope.solicitacoes = data;
        $scope.ExtrairTipoEquipamento(); 

        $scope.carregarSection();

        $scope.solicitacoes.forEach(function (valor, index, arr) {
            if (valor.SolicitacaoEquipamento.CodigoTipoEquipamento != 0) {
                var texto = ("Equipamento: " + (valor.Equipamento != null ? valor.Equipamento : "Não Alocado") + " Motorista: "
                       + (valor.Motorista != null ? valor.Motorista : "Não Alocado"));
                var item = {
                    start_date: inveterData(valor.DataInicio), end_date: inveterData(valor.DataFim), text: ("<b>Área: </b>" + valor.SolicitacaoEquipamento.NomeArea +
                    " <b>CC:</b> " + valor.SolicitacaoEquipamento.NomeCentroDeCusto) + " <b>Equip:</b> " + (valor.Equipamento != null ? valor.Equipamento : "Não Alocado"),
                    section_id: valor.SolicitacaoEquipamento.CodigoTipoEquipamento, id_solicitacao: valor.Codigo, color: getCor(valor.Status),
                    tooltip: texto
                };
                if (!existe($scope.agendas, item))
                    $scope.agendas.push(item);
            } else {
                var item = { start_date: inveterData(valor.DataInicio), end_date: inveterData(valor.DataFim), text: ("<b>Serviço: </b>" + valor.SolicitacaoEquipamento.ServicoDescricao), section_id: 99999, id_solicitacao: valor.Codigo, color: getCor(valor.Status) };
                if (!existe($scope.agendas, item))
                    $scope.agendas.push(item);
            }
        });
    }

    $scope.salvar = function () {
        
        var reg = $scope.registro;
        var age = $scope.agendas;

        var item = getItem($scope.solicitacoes, $scope.registro);
        if (item.UltimoStatus == "Em Atendimento" && item.CodigoEquipamento != $scope.registro.Equipamento.Codigo) {
            swal({
                title: "Atenção",
                text: "Alterar o equipamento irá gerar uma nova alocação. Deseja realmente continuar?",
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
                    gestaodassolicitacoesService.cadastrar($scope.registro).then(function (response) {
                        removeLoader();
                        if (response.data) {
                            $scope.registro = {};
                            $scope.$modalInstance.dismiss('cancel');
                            $scope.UpdateScheduler(false);

                        }
                    }, function (error) {

                    });
                }
            });

        } else {
            addLoader();
            gestaodassolicitacoesService.cadastrar($scope.registro).then(function (response) {
                removeLoader();
                if (response.data) {
                    $scope.registro = {};
                    $scope.$modalInstance.dismiss('cancel');
                    $scope.UpdateScheduler(false);

                }
            }, function (error) {

            });
        }

        //gestaodassolicitacoesService.cadastrar($scope.registro).then(function (response) {
        //    removeLoader();
        //    if (response.data) {
        //        $scope.registro = {};
        //        $scope.$modalInstance.dismiss('cancel');
        //        $scope.UpdateScheduler(false);

        //    }
        //}, function (error) {

        //});

    }

    $scope.consultar = function () {
        addLoader();
        gestaodassolicitacoesService.consultar().then(function (response) {
            $scope.registros = response.data;
            removeLoader()
        });
    }

    function getIndexItemAgenda(arr, data) {
        if (arr.length > 0) {
            for (var index = 0; index < arr.length; index++) {
                if (arr[index].id == data) {
                    return index;
                }
            }
        }
    }

    function getItem(arr, data) {
        var exist = {};
        if (arr.length > 0) {
            for (var index = 0; index < arr.length; index++) {
                if (arr[index].Codigo == data.Codigo) {
                    exist = arr[index];
                }
            }
        }
        return exist;
    }
    function desabilitarCampos(data) {
        $scope.hideDescricao = data.SolicitacaoEquipamento.ServicoDescricao == null ? false : true;
        switch (data.Status) {

            case 'Em Atendimento':
            case 'Refeição - Início':
            case 'Refeição - Fim':
            case 'Descanso - Início':
            case 'Descanso - Fim':
                $scope.disableTipoEquipamento = true;
                $scope.disableImplementos = true;
                $scope.disableEquipamentos = false;
                $scope.disableMotorista = false;
                $scope.disableDescricao = true;
                break;
            case 'Finalizada':
            case 'Recusada':
            case 'Cancelada':
                $scope.disableTipoEquipamento = true;
                $scope.disableImplementos = true;
                $scope.disableEquipamentos = true;
                $scope.disableMotorista = true;
                $scope.disableDescricao = true;
                break;
            default:
                $scope.disableTipoEquipamento = true;
                $scope.disableImplementos = false;
                $scope.disableEquipamentos = false;
                $scope.disableMotorista = false;
                $scope.disableDescricao = false;
                $scope.hideDescricao = false;
                break;
        }
    }

    $scope.carregarSection = function () {

        scheduler.createTimelineView({
            name: "timeline",
            x_unit: "minute",//measuring unit of the X-Axis.
            x_date: "%H:%i", //date format of the X-Axis
            x_step: 60,      //X-Axis step in 'x_unit's
            x_size: 24,      //numero de quadros
            x_start: 0,     //X-Axis offset in 'x_unit's
            x_length: 24,    //number of 'x_step's that will be scrolled at a time
            y_unit: sections,
            resize_events: false,
            fit_events: true,
            y_property: "section_id",
            render: "bar"
        });
    }

    $scope.init();

    $scope.ConvertStringToDateTime = function(dateString){
        //"28/07/2017 08:00:00"
        var dia = dateString.split("/")[0];
        var mes = dateString.split("/")[1];
        var ano = dateString.split("/")[2].split(" ")[0];
        var tempo = dateString.split("/")[2].split(" ")[1];
        var hora = tempo.split(":")[0];
        var minuto = tempo.split(":")[1];

        var newDate = new Date(ano, mes-1, dia, hora, minuto, 0, 0);

        return newDate;
        

    }

    $scope.AbrirModal = function (id, ev, onclick) {
        
        var record = scheduler.getEvent(id);
        var solicitacaoSelecionada = getItem($scope.solicitacoes, { Codigo: record.id_solicitacao });

        if (record.section_id == solicitacaoSelecionada.SolicitacaoEquipamento.CodigoTipoEquipamento) {

            if (onclick) {
                $scope.registro.HoraInicial = formatarHora(record.start_date);
                $scope.registro.HoraFinal = formatarHora(record.end_date);
            }
            else {
                $scope.registro.HoraInicial = formatarHora(ev.start_date);
                $scope.registro.HoraFinal = formatarHora(ev.end_date);
            }

            if ((solicitacaoSelecionada.Status == 'Finalizada') &&
                ((formatarData(record.start_date) != solicitacaoSelecionada.DataInicio) ||
                    (formatarData(record.end_date) != solicitacaoSelecionada.DataFim))) {
                sweetAlert("Atenção", "Não é possível alterar a alocação depois de finalizada!", "error");
                var index = getIndexItemAgenda($scope.agendas, id);
                $scope.agendas[index].start_date = $scope.ConvertStringToDateTime(solicitacaoSelecionada.DataInicio);
                $scope.agendas[index].end_date = $scope.ConvertStringToDateTime(solicitacaoSelecionada.DataFim);
                scheduler.clearAll();
                scheduler.parse($scope.agendas, "json");
                return;
            }

            $scope.registro.CodigoTipoEquipamento = solicitacaoSelecionada.SolicitacaoEquipamento.CodigoTipoEquipamento;

            var tipoEquip = { Codigo: record.section_id };
            $scope.registro.CodigoAlocacao = solicitacaoSelecionada.Codigo;
            $scope.registro.TipoEquipamento = getItem($scope.tiposEquipamentos, tipoEquip);

            $scope.registro.Area = getItem($scope.areas, { Codigo: (solicitacaoSelecionada.SolicitacaoEquipamento.CodigoArea == undefined ? "" : solicitacaoSelecionada.SolicitacaoEquipamento.CodigoArea) });
            $scope.registro.Codigo = solicitacaoSelecionada.Codigo;
            $scope.registro.CentroCusto = getItem($scope.centrosdecusto, { Codigo: solicitacaoSelecionada.SolicitacaoEquipamento.CodigoCentroDeCusto });
            $scope.registro.DataInicio = moment(record.start_date).toDate();
            $scope.registro.DataFim = moment(record.end_date).toDate();
            $scope.registro.SolicitacaoEquipamento = solicitacaoSelecionada;
            $scope.registro.NumeroPOS = solicitacaoSelecionada.NumeroPOS;
            $scope.registro.Descricao = solicitacaoSelecionada.SolicitacaoEquipamento.ServicoDescricao;
            $scope.registro.Regime = solicitacaoSelecionada.SolicitacaoEquipamento.Regime;
            $scope.registro.ImprimirPOS = solicitacaoSelecionada.ImprimirPOS;
            $scope.registro.DescricaoImplemento = solicitacaoSelecionada.DescricaoImplemento;
            $scope.registro.AlocarRecursos = false;

            if ($scope.registro.SolicitacaoEquipamento.UltimoStatus == "Finalizada") {
                $scope.registro.Equipamento = getItem($scope.equipamentos, { Codigo: solicitacaoSelecionada.CodigoEquipamento });
                $scope.registro.Motorista = getItem($scope.motoristas, { Codigo: solicitacaoSelecionada.CodigoMotorista });
            }
            else
            {
                gestaodassolicitacoesService.carregarComponentesModal($scope.registro).then(function (response) {
                    $scope.equipamentos = response.data.equipamentos;
                    $scope.motoristas = response.data.motoristas;
                    $scope.registro.Equipamento = getItem($scope.equipamentos, { Codigo: solicitacaoSelecionada.CodigoEquipamento });
                    $scope.registro.Motorista = getItem($scope.motoristas, { Codigo: solicitacaoSelecionada.CodigoMotorista });

                });

            }

            $scope.registro.Status = solicitacaoSelecionada.Status;
            desabilitarCampos(solicitacaoSelecionada);

            $scope.$modalInstance = $uibModal.open({
                animation: $scope.animationsEnabled,
                templateUrl: 'modalGestaoDasSolicitacoes',
                scope: $scope,
                backdrop: false
            });
            $scope.$modalInstance.result.then(function () {
            }, function () {
                $scope.registro = {};
            });

            return true;
        } else {
            sweetAlert("Atenção", "Não é possível alterar o tipo do equipamento!", "error");
            var index = getIndexItemAgenda($scope.agendas, id);
            $scope.agendas[index].section_id = solicitacaoSelecionada.SolicitacaoEquipamento.CodigoTipoEquipamento;
            scheduler.clearAll();
            scheduler.parse($scope.agendas, "json");
        }

    }

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

    $scope.loadMotoristas = function () {
        if ($scope.motoristas.length <= 0) {
            gestaodassolicitacoesService.consultarMotoristas().then(function (response) {
                $scope.motoristas = response.data;
            });
        }
    }

    $scope.loadAreas = function () {
        if ($scope.areas.length <= 0) {
            areasService.consultar().then(function (response) {
                $scope.areas = response.data;
            });
        }
    }
    $scope.loadRegime();
    $scope.loadCentroCusto();
    $scope.loadAreas();

    $scope.$on('$destroy', function iVeBeenDismissed() {
        scheduler.clearAll();
        scheduler.detachEvent(evtChanged);
        scheduler.detachEvent(evtClick);
        //window.location.reload();
    })

    function getCor(data) {
        var css = "";
        switch (data) {
            case 'Em Aprovação':
            case 'Programada':
                css = "#2fa1d6";
                break;
            case 'Em Programação':
                //css = "bg-primary-light";
                css = "#ffd600";
                break;

            case 'Recusada':
            case 'Cancelada':
                //css = "bg-danger-light";
                css = "#ef9a9a";
                break;
            case 'Em Atendimento':
                //css = "bg-success-light";
                css = "#58fc5f";
                break;
            case 'Refeição - Início':
            case 'Refeição - Fim':
            case 'Descanso - Início':
            case 'Descanso - Fim':
                //css = "bg-yellow-light";
                css = "#ffab91";
                break;
            case 'Finalizada':
                //css = "bg-gray-dark";
                css = "#9e9e9e";
                break;
        }
        return css;
    }

    function formatarData(record) {
        var resultado = "";;
        var dia = record.getDate();
        var mes = record.getMonth()+1;
        var ano = record.getFullYear();
        var horas = record.getHours();
        var minutos = record.getMinutes();
        if (mes < 10)
            mes = "0" + mes;
        if (horas < 10)
            horas = "0" + horas;
        if (minutos < 10)
            minutos = "0" + minutos;

        return String.format("{0}/{1}/{2} {3}:{4}:{5}", dia, mes, ano, horas, minutos, "00");

    }

    function formatarDataSemHora(record) {
        var resultado = "";;
        var dia = record.getDate();
        var mes = record.getMonth();
        var ano = record.getFullYear();
        var horas = record.getHours();
        var minutos = record.getMinutes();
        if (mes < 10)
            mes = "0" + mes;


        return String.format("{0}/{1}/{2}", dia, mes, ano);

    }
    
    scheduler.attachEvent("onBeforeViewChange", function (old_mode, old_date, mode, date) {
        if (formatarDataSemHora($scope.dataReferencia) != formatarDataSemHora(date) &&
            date != old_date) {
            $scope.dataReferencia = date;
            //$scope.UpdateScheduler(false);
        }
        return true;
    });

    $scope.ExtrairTipoEquipamento = function () {
        $scope.tiposEquipamentos = [];
        sections = [];
        sections.push({ key: 99999, label: "<b>Serviços </b>" });
        $scope.solicitacoes.forEach(function (valor, index, arr) {
            if (!existeItem($scope.tiposEquipamentos, valor.SolicitacaoEquipamento.CodigoTipoEquipamento)) {
                var item = { Codigo: valor.SolicitacaoEquipamento.CodigoTipoEquipamento, Descricao: valor.SolicitacaoEquipamento.NomeTipoEquipamento };
                $scope.tiposEquipamentos.push(item);
                sections.push({ key: item.Codigo, label: "<b>" + item.Descricao + "</b>" });
            }
        });
    }

    function existeItem(arr, codigo) {
        var exist = false;
        if (arr.length > 0) {
            for (var index = 0; index < arr.length; index++) {
                if (arr[index].Codigo == codigo) {
                    exist = true;
                }
            }
        }
        return exist;
    }

    function existe(arr, data) {
        var exist = false;
        if (arr.length > 0) {
            for (var index = 0; index < arr.length; index++) {
                if (arr[index].id_solicitacao == data.id_solicitacao) {
                    exist = true;
                }
            }
        }
        return exist;
    }

    function formatarHora(record) {
        if (record != undefined) {
            var minutos = record.getMinutes();
            var horas = record.getHours();
            if (horas < 10)
                horas = "0" + horas;
            if (minutos < 10)
                minutos = "0" + minutos;
            return String.format("{0}:{1}", horas, minutos);
        } else {
            return "";
        }

    }

    $scope.ImprimirPOS = function () {
        
        addLoader();
        gestaodassolicitacoesService.ImprimirPOS($scope.registro.CodigoAlocacao).then(function (response) {
            removeLoader();
            
            /*var fileName = 'error';

            try {
                fileName = response.headers()['content-disposition'].split(';')[1].split('=')[1]
            } catch (err) {

            }

            if (fileName == 'error')
                return;*/

            var blob = new Blob([response.data], {
                type: 'application/pdf'
            });
            //saveAs(blob, "POS.pdf");


            if (navigator.appVersion.toString().indexOf('.NET') > 0 || navigator.appVersion.toString().indexOf('Edge') > 0) {
                window.navigator.msSaveBlob(blob, "POS.pdf");
            } else {
                var fileURL = URL.createObjectURL(blob);
                window.open(fileURL);
            }


        }, function (response) {
            removeLoader();

            if (response.status === 0)
                toaster.pop('error', $translate.instant('pmotools.TITLEMSGERROR'), $translate.instant('pmotools.COMMUNICATIONFAILED'));
        });
    }

    //$scope.UpdateScheduler(true);
}]);
