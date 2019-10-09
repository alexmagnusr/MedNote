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
    public class AcaoModel : IModel
    {
        #region Atributos

        private static AcaoModel acaoModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static AcaoModel Instancia
        {
            get
            {
                if (acaoModel == null)
                    acaoModel = new AcaoModel();

                return acaoModel;
            }
        }
        #endregion

        #region Metódos CRUD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public IQueryable<Acao> Consultar()
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            IQueryable<Acao> query = contexto.ObterTodos<Acao>()
                .Where(f => f.DataDesativacao == null);            

            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public Acao Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            var Acao = contexto.ObterPorId<Acao>(codigo);

            Acao.DataDesativacao = DateTime.Now;

            contexto.BeginTransaction();
            contexto.Atualizar(Acao);
            contexto.Commit();

            return Acao;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public Acao Editar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            return contexto.ObterPorId<Acao>(codigo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AcaoNew"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        /// 
        public Acao Atualizar(Acao AcaoNew, int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            var AcaoOld = contexto.ObterPorId<Acao>(codigo);

            AcaoOld.Nome = AcaoNew.Nome;
            //AcaoOld.Descricao = AcaoNew.Descricao;

            contexto.BeginTransaction();
            contexto.Atualizar(AcaoOld);
            contexto.Commit();

            return AcaoOld;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Acao"></param>
        /// <returns></returns>
        public Acao Cadastrar(Acao acao)
        {
            ValidarCampos(acao);
            ValidarRegras(acao);

            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            acao.DataDeCadastro = DateTime.Now;
            contexto.Salvar(acao);

            return acao;
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
            //((Acao)objeto).DataDeCadastro = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="estadoObjeto"></param>
        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = default(EstadoObjeto?))
        {
            //this.verificaCamposObrigatorios((Acao)objeto);
            
        }
        #endregion

        #region Metódos Privados -  Regras
        private void verificaCamposObrigatorios(Acao Acao) {
            
            //if (Acao.Nome.IsEmpty())
            //    throw new CoreException("O campo \"Nome\" é obrigatório.");
            //else if (Acao.Descricao.IsEmpty())
            //    throw new CoreException("O campo \"Descrição\" é obrigatório.");
        }
        #endregion
    }
}