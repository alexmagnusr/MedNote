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
    public class PermissoesUsuariosModel : IModel
    {
        #region Atributos

        private static PermissoesUsuariosModel permissoesUsuariosModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static PermissoesUsuariosModel Instancia
        {
            get
            {
                if (permissoesUsuariosModel == null)
                    permissoesUsuariosModel = new PermissoesUsuariosModel();

                return permissoesUsuariosModel;
            }
        }
        #endregion

        #region Metódos CRUD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public IQueryable<PermisaoUsuarioAcao> Consultar(int parametro)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            try
            {
                IQueryable<PermisaoUsuarioAcao> query = contexto
                .ObterTodos<PermisaoUsuarioAcao>()
                .Where(p => p.Usuario.Codigo == parametro && p.DataDesativacao == null);

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
        public IList<PermisaoUsuarioAcao> Deletar(int[] acoes, int usuario)
        {
            List<PermisaoUsuarioAcao> permissoes = new List<PermisaoUsuarioAcao>();
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            try
            {
                foreach (var acao in acoes)
                {
                    var permissao = contexto.ObterTodos<PermisaoUsuarioAcao>()
                   .Where(p => p.Acao.Codigo == acao
                       && p.Usuario.Codigo == usuario
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
        public IList<PermisaoUsuarioAcao> Cadastrar(int[] acoes, int usuario)
        {
            List<PermisaoUsuarioAcao> permissoes = new List<PermisaoUsuarioAcao>();

            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            try
            {
                foreach (var acao in acoes)
                {
                    Acao acaoAdd = new Acao();
                    acaoAdd.Codigo = acao;

                    Usuario usuarioAdd = new Usuario();
                    usuarioAdd.Codigo = usuario;

                    PermisaoUsuarioAcao permissao = new PermisaoUsuarioAcao();
                    permissao.Acao = acaoAdd;
                    permissao.Usuario = usuarioAdd;
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
            //this.verificaCamposObrigatorios((PermisaoUsuarioAcao)objeto);
        }
        #endregion

        #region Metódos Privados -  Regras
        private void verificaCamposObrigatorios(PermisaoUsuarioAcao Permissoes)
        {

            //if (Permissoes.Nome.IsEmpty())
            //    throw new CoreException("O campo \"Nome\" é obrigatório.");
            //else if (Permissoes.Descricao.IsEmpty())
            //    throw new CoreException("O campo \"Descrição\" é obrigatório.");
        }
        #endregion
    }
}