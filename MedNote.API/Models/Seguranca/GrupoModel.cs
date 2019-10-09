using System;
using System.Linq;
using Globalsys.Extensoes;
using Globalsys;
using Globalsys.Model;
using Globalsys.Validacao;
using Globalsys.Exceptions;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra;
using MedNote.Repositorios.Seguranca;
using LinqKit;

namespace MedNote.API.Models.Seguranca
{
    /// <summary>
    /// 
    /// </summary>
    ///   
    public class GrupoModel : IModel
    {
        #region Atributos

        private static GrupoModel grupoModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public static GrupoModel Instancia
        {
            get
            {
                if (grupoModel == null)
                    grupoModel = new GrupoModel();

                return grupoModel;
            }
        }
        #endregion

        #region Metódos CRUD
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public IQueryable<Grupo> Consultar(int? codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = UnidadeDeTrabalho;
                //Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(contexto).ObterUsuarioLogado();

                var predicate = PredicateBuilder.New<Grupo>();
                predicate = predicate.And(p => p.Desativado == false);

                //if (!usuario.bl_login_ad)
                //{
                //predicate = predicate.And();
                //}
                //if (codigo != 0)
                //{
                //    predicate = predicate.And(p => p.Cliente.Codigo == codigo);
                //}

                IQueryable<Grupo> query = contexto.ObterTodos<Grupo>().Where(predicate);

                var lista = query.ToList();
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
        public Grupo Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            try
            {

                var grupo = contexto.ObterPorId<Grupo>(codigo);

                grupo.DataDesativacao = DateTime.Now;
                grupo.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(grupo);
                contexto.Commit();

                return grupo;
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
        /// <param name="codigo"></param>
        /// <returns></returns>
        public Grupo Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = UnidadeDeTrabalho;
                return contexto.ObterPorId<Grupo>(codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grupoNew"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        /// 
        public Grupo Atualizar(Grupo grupoNew, int codigo)
        {
            IUnidadeTrabalho contexto = UnidadeDeTrabalho;
            try
            {

                var grupoOld = contexto.ObterPorId<Grupo>(codigo);
                grupoOld.Descricao = grupoNew.Descricao;
                //grupoOld.Cliente = grupoNew.Cliente;
                //var cliente = contexto.ObterPorId<Cliente>(grupoNew.Cliente.Codigo);
                //grupoOld.Cliente = cliente;
                contexto.BeginTransaction();
                contexto.Atualizar(grupoOld);
                contexto.Commit();

                return grupoOld;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public Grupo Cadastrar(Grupo grupo)
        {

            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            Grupo gruponew = new Grupo();
            try
            {
                contexto.BeginTransaction();
                //var cliente = contexto.ObterPorId<Cliente>(grupo.Cliente.Codigo);
                //gruponew.Cliente = cliente;
                //gruponew.Cliente = grupo.Cliente;
                gruponew.Descricao = grupo.Descricao;
                
                gruponew.DataDeCadastro = DateTime.Now;
               
                ValidarCampos(gruponew);
                ValidarRegras(gruponew, EstadoObjeto.Novo);

                contexto.Salvar<Grupo>(gruponew);
                contexto.Commit();
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
            return gruponew;
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
            ((Grupo)objeto).DataDeCadastro = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objeto"></param>
        /// <param name="estadoObjeto"></param>
        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = default(EstadoObjeto?))
        {
            this.verificaCamposObrigatorios((Grupo)objeto);

        }
        #endregion

        #region Metódos Privados -  Regras
        private void verificaCamposObrigatorios(Grupo grupo)
        {

     
            if (grupo.Descricao.IsEmpty())
                throw new CoreException("O campo \"Descrição\" é obrigatório.");

        }
        #endregion
    }
}