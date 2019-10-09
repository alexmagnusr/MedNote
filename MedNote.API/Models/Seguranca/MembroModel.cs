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

namespace VIX.API.Models.Seguranca
{
    /// <summary>
    /// 
    /// </summary>
    ///   
    public class MembroModel : IModel
    {
        #region Atributos

        private static MembroModel membroModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static MembroModel Instancia
        {
            get
            {
                if (membroModel == null)
                    membroModel = new MembroModel();

                return membroModel;
            }
        }
        #endregion

        #region Metódos CRUD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public IQueryable<Membro> Consultar(int parametro)
        {
            try
            {
                IUnidadeTrabalho contexto = UnidadeDeTrabalho;

                IQueryable<Membro> query = contexto.ObterTodos<Membro>()
                    .Where(m => m.Grupo.Codigo == parametro && m.DataDesativacao == null);

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
        public IList<Membro> Deletar(int[] usuarios, int grupo)
        {
            List<Membro> membros = new List<Membro>();
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            try
            {
                foreach (var usu in usuarios)
                {
                    var membro = contexto.ObterTodos<Membro>()
                   .Where(m => m.Usuario.Codigo == usu
                       && m.Grupo.Codigo == grupo
                       && m.DataDesativacao == null)
                   .FirstOrDefault();

                    membro.DataDesativacao = DateTime.Now;

                    contexto.BeginTransaction();
                    contexto.Salvar(membro);
                    contexto.Commit();

                    membros.Add(membro);
                }

                return membros;
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
        /// <param name="Membro"></param>
        /// <returns></returns>
        public IList<Membro> Cadastrar(int[] usuarios, int grupo)
        {
            List<Membro> membros = new List<Membro>();

            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            try
            {
                foreach (var usu in usuarios)
                {
                    Usuario usuarioAdd = new Usuario();
                    usuarioAdd.Codigo = usu;

                    Grupo grupoAdd = new Grupo();
                    grupoAdd.Codigo = grupo;

                    Membro membro = new Membro();
                    membro.Usuario = usuarioAdd;
                    membro.Grupo = grupoAdd;
                    membro.DataDeCadastro = DateTime.Now;

                    contexto.BeginTransaction();
                    contexto.Salvar(membro);
                    contexto.Commit();

                    membros.Add(membro);
                }

                return membros;
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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="estadoObjeto"></param>
        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = default(EstadoObjeto?))
        {

        }
        #endregion

        #region Metódos Privados -  Regras

        #endregion
    }
}