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
    public class FuncaoModel : IModel
    {
        #region Atributos

        private static FuncaoModel funcaoModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static FuncaoModel Instancia
        {
            get
            {
                if (funcaoModel == null)
                    funcaoModel = new FuncaoModel();

                return funcaoModel;
            }
        }
        #endregion

        #region Metódos CRUD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public IQueryable<Funcao> Consultar()
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            IQueryable<Funcao> query = contexto.ObterTodos<Funcao>()
                .Where(f => f.DataDesativacao == null);

            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public Funcao Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            var Funcao = contexto.ObterPorId<Funcao>(codigo);

            Funcao.DataDesativacao = DateTime.Now;

            contexto.BeginTransaction();
            contexto.Atualizar(Funcao);
            contexto.Commit();

            return Funcao;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public Funcao Editar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            return contexto.ObterPorId<Funcao>(codigo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FuncaoNew"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        /// 
        public Funcao Atualizar(Funcao FuncaoNew, int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            var FuncaoOld = contexto.ObterPorId<Funcao>(codigo);

            FuncaoOld.Nome = FuncaoNew.Nome;
            FuncaoOld.Descricao = FuncaoNew.Descricao;
            FuncaoOld.Ref = FuncaoNew.Ref;
            FuncaoOld.Cor = FuncaoNew.Cor;
            FuncaoOld.Canal = FuncaoNew.Canal;
            FuncaoOld.Pai = FuncaoNew.Pai;
            FuncaoOld.Tipo = FuncaoNew.Tipo;

            contexto.BeginTransaction();
            contexto.Atualizar(FuncaoOld);
            contexto.Commit();

            return FuncaoOld;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Funcao"></param>
        /// <returns></returns>
        public Funcao Cadastrar(Funcao funcao)
        {
            //ValidarCampos(Funcao);
            //ValidarRegras(Funcao);
            funcao.DataDeCadastro = DateTime.Now;

            IUnidadeTrabalho contexto = UnidadeDeTrabalho;

            contexto.Salvar(funcao);

            return funcao;
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
            //((Funcao)objeto).DataDeCadastro = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="estadoObjeto"></param>
        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = default(EstadoObjeto?))
        {
            //this.verificaCamposObrigatorios((Funcao)objeto);

        }
        #endregion

        #region Metódos Privados -  Regras
        private void verificaCamposObrigatorios(Funcao Funcao)
        {

            //if (Funcao.Nome.IsEmpty())
            //    throw new CoreException("O campo \"Nome\" é obrigatório.");
            //else if (Funcao.Descricao.IsEmpty())
            //    throw new CoreException("O campo \"Descrição\" é obrigatório.");
        }
        #endregion
    }
}