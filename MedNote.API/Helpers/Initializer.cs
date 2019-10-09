using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra;
using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VIX.API.Helpers
{
    public class Initializer
    {
        public class Modulo
        {
            public string Nome { get; set; }

            public string Icon { get; set; }

            public string Descricao { get; set; }

            public List<Pagina> Paginas { get; set; }

            public Modulo()
            {
                this.Paginas = new List<Pagina>();
            }
        }
        
        public class Pagina
        {
            public List<Acao> Acoes { get; set; }

            public string Nome { get; set; }

            public string Ref { get; set; }

            public string Descricao { get; set; }

            public Pagina()
            {
                this.Acoes = new List<Acao>();
            }
        }

        public class Acao
        {
            public string Nome { get; set; }

            public string Descricao { get; set; }

            public List<Recurso> Recursos { get; set; }

            public Acao()
            {
                Recursos = new List<Recurso>();
            }
        }

        public class Recurso
        {
            public string Acao { get; set; }

            public string Pagina { get; set; }

        }

        private static List<Modulo> GetResources()
        {
            List<Modulo> list = new List<Modulo>();

            #region MODULOS
            /**********MODULO SEGURANCA***************/
            Modulo moduloSeguranca = new Modulo();
            moduloSeguranca.Nome = "Segurança";
            moduloSeguranca.Descricao = "Segurança";
            /**********MODULO SEGURANCA***************/


            /**********MODULO SISTEMA***************/
            Modulo moduloSistema = new Modulo();
            moduloSistema.Nome = "Sistema";
            moduloSistema.Descricao = "Sistema";
            /**********MODULO SISTEMA***************/

            /**********MODULO CADASTRO INICIAL***************/
            Modulo moduloCadastroInicial = new Modulo();
            moduloCadastroInicial.Nome = "Cadastro Inicial";
            moduloCadastroInicial.Descricao = "Cadastro Inicial";
            /**********MODULO CADASTRO INICIAL***************/

            /**********MODULO OIL & GAS***************/
            Modulo moduloOilEGas = new Modulo();
            moduloOilEGas.Nome = "ESTOQUE";
            moduloOilEGas.Descricao = "ESTOQUE";
            moduloOilEGas.Icon = "../app/img/Oil & Gas.png";
            /**********MODULO OIL & GAS***************/

            #endregion MODULOS

            #region MODULO DE SEGURANÇA - PAGINAS
            /********************************MODULO DE SEGURANÇA *********************************************/
            Pagina usuario = new Pagina { Nome = "Usuario", Descricao = "Usuário", Ref = "app.seguranca-usuario" };
            usuario.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Usuário do sistema" });
            usuario.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Usuário do sistema",
                Recursos = new List<Recurso>() 
                { 
                    new Recurso {Acao = "ConsultarTipoColaborador", Pagina = "Colaborador"}

                }
            });
            usuario.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Usuário do sistema" });
            usuario.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Usuário do sistema" });
            usuario.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Usuário do sistema" });
            usuario.Acoes.Add(new Acao { Nome = "ConsultarUsuarioPorTipoColaborador", Descricao = "ConsultarUsuarioPorTipoColaborador - Usuário do sistema" });
            usuario.Acoes.Add(new Acao { Nome = "ObterNomeDoUsuarioLogado", Descricao = "ObterNomeDoUsuarioLogado - Usuário do sistema" });
            usuario.Acoes.Add(new Acao { Nome = "Detalhe", Descricao = "Detalhe - Usuário do sistema" });
            usuario.Acoes.Add(new Acao { Nome = "MeusDados", Descricao = "Deletar - Usuário do sistema" });
            usuario.Acoes.Add(new Acao { Nome = "ObterUsuarioLogado", Descricao = "Deletar - Usuário do sistema" });
            usuario.Acoes.Add(new Acao { Nome = "TrocarSenha", Descricao = "Deletar - Usuário do sistema" });
            moduloSeguranca.Paginas.Add(usuario);

            Pagina colaborador = new Pagina { Nome = "Colaborador", Descricao = "Colaborador" };
            colaborador.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Colaborador" });
            colaborador.Acoes.Add(new Acao { Nome = "Get/id", Descricao = "Editar - Colaborador" });
            colaborador.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Colaborador" });
            colaborador.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Colaborador" });
            colaborador.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Colaborador" });
            colaborador.Acoes.Add(new Acao { Nome = "ConsultarTipoColaborador", Descricao = "Consultar por tipo - Colaborador" });
            moduloSeguranca.Paginas.Add(colaborador);

            Pagina grupo = new Pagina { Nome = "Grupo", Descricao = "Grupos de Usuário", Ref = "app.seguranca-grupo" };
            grupo.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Grupo" });
            grupo.Acoes.Add(new Acao { Nome = "Get/id", Descricao = "Editar - Grupo" });
            grupo.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Grupo" });
            grupo.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Grupo" });
            grupo.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Grupo" });
            moduloSeguranca.Paginas.Add(grupo);

            Pagina membro = new Pagina { Nome = "Membro", Descricao = "Membros", Ref = "app.seguranca-membro" };
            membro.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Membro" });
            membro.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Membro",
                Recursos = new List<Recurso>() 
                { 
                    new Recurso {Acao = "Get", Pagina = "Grupo"},
                    new Recurso {Acao = "ConsultarTipoColaborador", Pagina = "Colaborador"},
                    new Recurso {Acao = "ConsultarUsuarioPorTipoColaborador", Pagina = "Usuario"}

                }
            });
            membro.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Membro" });
            membro.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Membro" });
            membro.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Membro" });
            moduloSeguranca.Paginas.Add(membro);

            Pagina permissoes = new Pagina { Nome = "Permissoes", Descricao = "Permissões de Grupos", Ref = "app.seguranca-permissoes-grupos" };
            permissoes.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Permissoes" });
            permissoes.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Permissoes",
                Recursos = new List<Recurso>() 
                { 
                    new Recurso {Acao = "Get", Pagina = "Grupo"},
                    new Recurso {Acao = "Get", Pagina = "Funcao"},
                    new Recurso {Acao = "ConsultarPorTipo", Pagina = "Funcao"},
                    new Recurso {Acao = "ConsultarTipo", Pagina = "Funcao"},
                    new Recurso {Acao = "ConsultarCanal", Pagina = "Funcao"},
                    new Recurso {Acao = "ConsultarPaginasPorModulo", Pagina = "Funcao"},
                    new Recurso {Acao = "Get", Pagina = "Acao"},
                    new Recurso {Acao = "ConsultarAcoesPorPagina", Pagina = "Acao"},
                    new Recurso {Acao = "ConsultarTipoColaborador", Pagina = "Colaborador"},
                    new Recurso {Acao = "ConsultarUsuarioPorTipoColaborador", Pagina = "Usuario"}

                }
            });
            permissoes.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Permissoes" });
            permissoes.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Permissoes" });
            permissoes.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Permissoes" });
            moduloSeguranca.Paginas.Add(permissoes);


            Pagina permissoesUsuarios = new Pagina { Nome = "PermissoesUsuarios", Descricao = "Permissões de Usuários", Ref = "app.seguranca-permissoes-usuarios" };
            permissoesUsuarios.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - PermissoesUsuarios" });
            permissoesUsuarios.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - PermissoesUsuarios",
                Recursos = new List<Recurso>() 
                { 
                    new Recurso {Acao = "Get", Pagina = "Grupo"},
                    new Recurso {Acao = "Get", Pagina = "Funcao"},
                    new Recurso {Acao = "ConsultarPorTipo", Pagina = "Funcao"},
                    new Recurso {Acao = "ConsultarTipo", Pagina = "Funcao"},
                    new Recurso {Acao = "ConsultarCanal", Pagina = "Funcao"},
                    new Recurso {Acao = "ConsultarPaginasPorModulo", Pagina = "Funcao"},
                    new Recurso {Acao = "Get", Pagina = "Acao"},
                    new Recurso {Acao = "ConsultarAcoesPorPagina", Pagina = "Acao"},
                    new Recurso {Acao = "ConsultarTipoColaborador", Pagina = "Colaborador"},
                    new Recurso {Acao = "ConsultarUsuarioPorTipoColaborador", Pagina = "Usuario"}

                }
            });
            permissoesUsuarios.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - PermissoesUsuarios" });
            permissoesUsuarios.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - PermissoesUsuarios" });
            permissoesUsuarios.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - PermissoesUsuarios" });
            moduloSeguranca.Paginas.Add(permissoesUsuarios);

            /********************************MODULO DE SEGURANÇA *********************************************/
            #endregion MODULO DE SEGURANÇA - PAGINAS

            #region MODULO DE SISTEMA - PAGINAS

            /******************************************AUDITORIA***********************************/
            Pagina auditoria = new Pagina { Nome = "Auditoria", Descricao = "Auditoria", Ref = "app.sistema-auditoria" };
            auditoria.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Auditoria" });
            moduloSistema.Paginas.Add(auditoria);
            /******************************************AUDITORIA***********************************/

            /******************************************LOG ERRO***********************************/
            Pagina logErro = new Pagina { Nome = "LogErro", Descricao = "LogErro", Ref = "app.sistema-logerros" };
            logErro.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - LogErro" });
            moduloSistema.Paginas.Add(logErro);
            /******************************************LOG ERRO***********************************/

            /******************************************Menu***********************************/
            Pagina menu = new Pagina { Nome = "Menu", Descricao = "Menu" };
            menu.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Menu" });
            moduloSistema.Paginas.Add(menu);
            /******************************************Menu***********************************/

            /******************************************Modulo***********************************/
            Pagina modulo = new Pagina { Nome = "Modulo", Descricao = "Modulo" };
            modulo.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Modulo" });
            moduloSistema.Paginas.Add(modulo);
            /******************************************Modulo***********************************/

            /******************************************Modulo***********************************/
            Pagina notificacao = new Pagina { Nome = "Notificacao", Descricao = "Notificacao", Ref = "app.notificacoes" };
            notificacao.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Consultar" });
            notificacao.Acoes.Add(new Acao { Nome = "QntNotificacoes", Descricao = "Consultar - QntNotificacoes" });
            notificacao.Acoes.Add(new Acao { Nome = "MarcarComoVisualizado", Descricao = "Consultar - MarcarComoVisualizado" });
            notificacao.Acoes.Add(new Acao { Nome = "SetPlayerId", Descricao = "Consultar - SetPlayerId" });
            moduloSistema.Paginas.Add(notificacao);
            /******************************************Modulo***********************************/

            /******************************************Modulo***********************************/
            Pagina permissions = new Pagina { Nome = "Permissions", Descricao = "Permissions" };
            permissions.Acoes.Add(new Acao { Nome = "IsMotorist", Descricao = "Consultar - IsMotorist" });
            moduloSistema.Paginas.Add(permissions);
            /******************************************Modulo***********************************/




            #endregion MODULO DE SISTEMA - PAGINAS

            #region CADASTRO INICIAL - PAGINAS

            /******************************************CENTRO DE CUSTO***********************************/
            Pagina centrosDeCusto = new Pagina { Nome = "CentrosDeCusto", Descricao = "Centro de Custo", Ref = "app.centrocusto" };
            centrosDeCusto.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Centro de custo" });
            centrosDeCusto.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Centro de custo",
                /* Recursos = new List<Recurso>() 
                 { 
                     new Recurso {Acao = "Get", Pagina = "Gerencia"}

                 }*/
            });
             centrosDeCusto.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Centro de custo" });
             centrosDeCusto.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Centro de custo" });
             centrosDeCusto.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Centro de custo" });
             moduloCadastroInicial.Paginas.Add(centrosDeCusto);
            /******************************************CENTRO DE CUSTO***********************************/
            /******************************************FORNECEDOR***********************************/
            Pagina fornecedor = new Pagina { Nome = "Fornecedor", Descricao = "Fornecedor", Ref = "app.fornecedor" };
            fornecedor.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Fornecedor" });
            fornecedor.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Fornecedor",
                /* Recursos = new List<Recurso>() 
                 { 
                     new Recurso {Acao = "Get", Pagina = "Gerencia"}

                 }*/
            });
            fornecedor.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Fornecedor" });
            fornecedor.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Fornecedor" });
            fornecedor.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Fornecedor" });
            moduloCadastroInicial.Paginas.Add(fornecedor);
            /******************************************FORNECEDOR***********************************/
            /******************************************CLIENTE***********************************/
            Pagina cliente = new Pagina { Nome = "Cliente", Descricao = "Cliente", Ref = "app.cliente" };
            cliente.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Cliente" });
            cliente.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Cliente",
            });
            cliente.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Cliente" });
            cliente.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Cliente" });
            cliente.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Cliente" });
            moduloCadastroInicial.Paginas.Add(cliente);
            /******************************************CLIENTE***********************************/
            /******************************************SETOR***********************************/
            Pagina setor = new Pagina { Nome = "Setor", Descricao = "Setor", Ref = "app.setor" };
            fornecedor.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Setor" });
            fornecedor.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Setor",
                /* Recursos = new List<Recurso>() 
                 { 
                     new Recurso {Acao = "Get", Pagina = "Gerencia"}

                 }*/
            });
            setor.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Setor" });
            setor.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Setor" });
            setor.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Setor" });
            moduloCadastroInicial.Paginas.Add(setor);
            /******************************************SETOR***********************************/
            /******************************************Cliente***********************************/
            Pagina Cliente = new Pagina { Nome = "Cliente", Descricao = "Cliente", Ref = "app.Cliente" };
            Cliente.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Cliente" });
            Cliente.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Cliente",
                /* Recursos = new List<Recurso>() 
                 { 
                     new Recurso {Acao = "Get", Pagina = "Gerencia"}

                 }*/
            });
            Cliente.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Cliente" });
            Cliente.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Cliente" });
            Cliente.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Cliente" });
            moduloCadastroInicial.Paginas.Add(Cliente);
            /******************************************Cliente***********************************/



            #endregion CADASTRO INICIAL - PAGINAS

            #region ESTOQUE

            /******************************************CONTA MATERIAL***********************************/
            Pagina contaDeMaterial = new Pagina { Nome = "CotaDeMaterias", Descricao = "Cota de materiais", Ref = "app.cotadematerial" };
            contaDeMaterial.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Cota de materiais" });
            contaDeMaterial.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Cota de materiais",
            });
            contaDeMaterial.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Cota de materiais" });
            contaDeMaterial.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Cota de materiais" });
            contaDeMaterial.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Cota de materiais" });
            moduloOilEGas.Paginas.Add(contaDeMaterial);
            /******************************************CONTA MATERIAL***********************************/

            /******************************************ENTRADA MATERIAL***********************************/
            Pagina entradaDeMateriais = new Pagina { Nome = "EntradaDeMateriais", Descricao = "Entrada De Materiais", Ref = "app.entradademateriais" };
            entradaDeMateriais.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Entrada De Materiais" });
            contaDeMaterial.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Entrada De Materiais",
            });
            entradaDeMateriais.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Entrada De Materiais" });
            entradaDeMateriais.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Entrada De Materiais" });
            entradaDeMateriais.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Entrada De Materiais" });
            moduloOilEGas.Paginas.Add(entradaDeMateriais);
            /******************************************ENTRADA MATERIAL***********************************/
            /******************************************MATERIAL***********************************/
            Pagina material = new Pagina { Nome = "Materiais", Descricao = "Materiais", Ref = "app.material" };
            material.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Materiais" });
            material.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Materiais",
            });
            material.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Materiais" });
            material.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Materiais" });
            material.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Materiais" });
            moduloOilEGas.Paginas.Add(material);
            /******************************************MATERIAL***********************************/

            /******************************************BaixaDeMateriais***********************************/
            Pagina baixaDeMateriais = new Pagina { Nome = "BaixaDeMateriais", Descricao = "Baixa de Materiais", Ref = "app.baixadematerial" };
            baixaDeMateriais.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Baixa de Materiais" });
            baixaDeMateriais.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Baixa de Materiais",
            });
            baixaDeMateriais.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Baixa de Materiais" });
            baixaDeMateriais.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Baixa de Materiais" });
            baixaDeMateriais.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Baixa de Materiais" });
            moduloOilEGas.Paginas.Add(baixaDeMateriais);
            /******************************************BaixaDeMateriais***********************************/

            /******************************************SolicitarMaterial***********************************/
            Pagina solicitarMateriais = new Pagina { Nome = "SolicitarMaterial", Descricao = "Solicitação de Materiais", Ref = "app.solicitarmaterial" };
            solicitarMateriais.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Solicitação de Materiais" });
            solicitarMateriais.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Solicitação de Materiais",
            });
            solicitarMateriais.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Solicitação de Materiais" });
            solicitarMateriais.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Solicitação de Materiais" });
            solicitarMateriais.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Solicitação de Materiais" });
            moduloOilEGas.Paginas.Add(solicitarMateriais);
            /******************************************SolicitarMaterial***********************************/



            /******************************************Liberação de Materiais***********************************/
            Pagina liberacaoDeMateriais = new Pagina { Nome = "LiberacaoDeMateriais", Descricao = "Liberação de Materiais", Ref = "app.liberacaodemateriais" };
            liberacaoDeMateriais.Acoes.Add(new Acao { Nome = "Get", Descricao = "Consultar - Liberação de Materiais" });
            liberacaoDeMateriais.Acoes.Add(new Acao
            {
                Nome = "Get/id",
                Descricao = "Editar - Liberação de Materiais",
            });
            liberacaoDeMateriais.Acoes.Add(new Acao { Nome = "Post", Descricao = "Cadastrar - Liberação de Materiais" });
            liberacaoDeMateriais.Acoes.Add(new Acao { Nome = "Put", Descricao = "Atualizar - Liberação de Materiais" });
            liberacaoDeMateriais.Acoes.Add(new Acao { Nome = "Delete", Descricao = "Deletar - Liberação de Materiais" });
            moduloOilEGas.Paginas.Add(liberacaoDeMateriais);
            /******************************************Liberação de Materiais***********************************/

            #endregion ESTOQUE


            list.Add(moduloCadastroInicial);
            list.Add(moduloSistema);
            list.Add(moduloSeguranca);
            list.Add(moduloOilEGas);


            return list;
        }

        public static void Run()
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                contexto.ExecuteSql<Object>("DELETE FROM SEGURANCA.PERM_GRUPO_ACAO");
                contexto.ExecuteSql<Object>("DELETE FROM SEGURANCA.SEG_PERM_USUARIO_ACAO");
                contexto.ExecuteSql<Object>("DELETE FROM SISTEMA.RECURSOS");
                contexto.ExecuteSql<Object>("DELETE FROM SISTEMA.ACAO");
                contexto.ExecuteSql<Object>("DELETE FROM SISTEMA.FUNCOES");
                var data = GetResources();

                #region CADASTRAR MODULO
                foreach (Modulo modulo in data)
                {
                    contexto.BeginTransaction();
                    MedNote.Infra.Dominio.Seguranca.Funcao funcao = new MedNote.Infra.Dominio.Seguranca.Funcao()
                    {
                        Canal = MedNote.Infra.Dominio.Seguranca.Canal.Ambos,
                        Tipo = MedNote.Infra.Dominio.Seguranca.TipoFuncao.Modulo,
                        Cor = "#FFFFFF",
                        Descricao = modulo.Descricao,
                        Nome = modulo.Nome,
                        Icone = modulo.Icon,
                        DataDeCadastro = DateTime.Today

                    };
                    contexto.Salvar<MedNote.Infra.Dominio.Seguranca.Funcao>(funcao);
                    contexto.Commit();
                }
                #endregion CADASTRAR MODULO

                #region CADASTRAR PAGINAS
                foreach (Modulo modulo in data)
                {
                    MedNote.Infra.Dominio.Seguranca.Funcao funcaoModulo = contexto.ObterTodos<MedNote.Infra.Dominio.Seguranca.Funcao>().Where(p => p.Nome.Equals(modulo.Nome) && p.Descricao.Equals(modulo.Descricao)).FirstOrDefault();
                    foreach (Pagina pagina in modulo.Paginas)
                    {
                        contexto.BeginTransaction();

                        MedNote.Infra.Dominio.Seguranca.Funcao funcao = new MedNote.Infra.Dominio.Seguranca.Funcao()
                    {
                        Canal = MedNote.Infra.Dominio.Seguranca.Canal.Ambos,
                        Tipo = MedNote.Infra.Dominio.Seguranca.TipoFuncao.Pagina,
                        Cor = "#FFFFFF",
                        Descricao = pagina.Descricao,
                        Nome = pagina.Nome,
                        Ref = pagina.Ref,
                        Icone = "#",
                        DataDeCadastro = DateTime.Today,
                        Pai = funcaoModulo

                    };
                        contexto.Salvar<MedNote.Infra.Dominio.Seguranca.Funcao>(funcao);
                        contexto.Commit();
                    }
                }
                #endregion CADASTRAR PAGINAS



                foreach (Pagina pagina in data.SelectMany(p => p.Paginas))
                {
                    Funcao funcao = contexto.ObterTodos<Funcao>().Where(p => p.Nome.Equals(pagina.Nome) && p.Tipo == MedNote.Infra.Dominio.Seguranca.TipoFuncao.Pagina).FirstOrDefault();
                    foreach (Acao acao in pagina.Acoes)
                    {
                        contexto.BeginTransaction();
                        MedNote.Infra.Dominio.Seguranca.Acao acaoPagina = new MedNote.Infra.Dominio.Seguranca.Acao()
                        {
                            Nome = acao.Nome,
                            Ref = acao.Nome + funcao.Nome,
                            Funcao = funcao,
                            DataDeCadastro = DateTime.Today

                        };
                        contexto.Salvar<MedNote.Infra.Dominio.Seguranca.Acao>(acaoPagina);
                        contexto.Commit();
                    }
                }


                foreach (Pagina pagina in data.SelectMany(p => p.Paginas))
                {

                    MedNote.Infra.Dominio.Seguranca.Funcao funcao = contexto.ObterTodos<MedNote.Infra.Dominio.Seguranca.Funcao>().Where(p => p.Nome.Equals(pagina.Nome) && p.Tipo == MedNote.Infra.Dominio.Seguranca.TipoFuncao.Pagina).FirstOrDefault();


                    foreach (Acao acao in pagina.Acoes)
                    {

                        MedNote.Infra.Dominio.Seguranca.Acao acaoPrincipal = contexto.ObterTodos<MedNote.Infra.Dominio.Seguranca.Acao>().Where(p => p.Nome.Equals(acao.Nome) && p.Funcao.Codigo == funcao.Codigo).FirstOrDefault();

                        foreach (Recurso recurso in acao.Recursos)
                        {
                            contexto.BeginTransaction();
                            var paginaFuncao = contexto.ObterTodos<MedNote.Infra.Dominio.Seguranca.Funcao>().Where(p => p.Nome.Equals(recurso.Pagina)).ToList().FirstOrDefault();
                            MedNote.Infra.Dominio.Seguranca.Acao acaoSecundaria = contexto.ObterTodos<MedNote.Infra.Dominio.Seguranca.Acao>().Where(p => p.Funcao.Codigo == paginaFuncao.Codigo && p.Nome.Equals(recurso.Acao)).ToList().FirstOrDefault();
                            //VIX.Dominio.Seguranca.Acao acaoSecundaria = contexto.ObterTodos<VIX.Dominio.Seguranca.Funcao>().Where(p => p.Nome.Equals(recurso.Pagina)).ToList().SelectMany(x => x.Acoes.Where(v => v.Nome.Equals(recurso.Acao))).FirstOrDefault();
                            MedNote.Infra.Dominio.Seguranca.Recurso recursos = new MedNote.Infra.Dominio.Seguranca.Recurso()
                            {
                                AcaoPrincipal = acaoPrincipal,
                                AcaoSecundaria = acaoSecundaria
                            };
                            contexto.Salvar<MedNote.Infra.Dominio.Seguranca.Recurso>(recursos);
                            contexto.Commit();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                String sucesso = "";
            }
        }
    }
}