using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VIX.API.Helpers.Globalsys;
using Globalsys.Extensoes;
using Globalsys;
using System.Threading.Tasks;
using Globalsys.Model;
using Globalsys.Validacao;
using Globalsys.Exceptions;
using Globalsys.Mvc;
using MedNote.Infra.Dominio.Seguranca;

namespace MedNote.API.Models.Seguranca
{
    /// <summary>
    /// 
    /// </summary>
    ///   
    public class PermissoesModel : IModel
    {
        #region Atributos

        private static PermissoesModel permissoesModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static PermissoesModel Instancia
        {
            get
            {
                if (permissoesModel == null)
                    permissoesModel = new PermissoesModel();

                return permissoesModel;
            }
        }
        #endregion

        #region Metódos CRUD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public IQueryable<PermissaoGrupoAcao> Consultar(int parametro)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            try
            {
                IQueryable<PermissaoGrupoAcao> query = contexto
                .ObterTodos<PermissaoGrupoAcao>()
                .Where(p => p.Grupo.Codigo == parametro && p.DataDesativacao == null);

                return query;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public IList<PermissaoGrupoAcao> Deletar(int[] acoes, int grupo)
        {
            List<PermissaoGrupoAcao> permissoes = new List<PermissaoGrupoAcao>();
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            try
            {
                foreach (var acao in acoes)
                {
                    var permissao = contexto.ObterTodos<PermissaoGrupoAcao>()
                   .Where(p => p.Acao.Codigo == acao
                       && p.Grupo.Codigo == grupo
                       && p.DataDesativacao == null)
                   .FirstOrDefault();

                    permissao.DataDesativacao = DateTime.Now;

                    contexto.BeginTransaction();
                    contexto.Salvar(permissao);
                    contexto.Commit();

                    permissoes.Add(permissao);
                }

                return permissoes;

            }

            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Permissoes"></param>
        /// <returns></returns>
        public IList<PermissaoGrupoAcao> Cadastrar(int[] acoes, int grupo)
        {
            List<PermissaoGrupoAcao> permissoes = new List<PermissaoGrupoAcao>();
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            try
            {
                foreach (var acao in acoes)
                {
                    Acao acaoAdd = new Acao();
                    acaoAdd.Codigo = acao;

                    Grupo grupoAdd = new Grupo();
                    grupoAdd.Codigo = grupo;

                    PermissaoGrupoAcao permissao = new PermissaoGrupoAcao();
                    permissao.Acao = acaoAdd;
                    permissao.Grupo = grupoAdd;
                    permissao.DataDeCadastro = DateTime.Now;


                    contexto.BeginTransaction();
                    contexto.Salvar(permissao);
                    contexto.Commit();

                    permissoes.Add(permissao);
                }

                return permissoes;
            }

            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }
        #endregion

        #region Validações
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="estadoObjeto"></param>
        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = default(EstadoObjeto?))
        {
            //Atribuir data de cadastro
            //((Permissoes)objeto).DataDeCadastro = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="estadoObjeto"></param>
        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = default(EstadoObjeto?))
        {
            this.verificaCamposObrigatorios((PermissaoGrupoAcao)objeto);
        }
        #endregion

        #region Metódos Privados -  Regras
        private void verificaCamposObrigatorios(PermissaoGrupoAcao Permissoes)
        {

            //if (Permissoes.Nome.IsEmpty())
            //    throw new CoreException("O campo \"Nome\" é obrigatório.");
            //else if (Permissoes.Descricao.IsEmpty())
            //    throw new CoreException("O campo \"Descrição\" é obrigatório.");
        }
        #endregion
    }
}