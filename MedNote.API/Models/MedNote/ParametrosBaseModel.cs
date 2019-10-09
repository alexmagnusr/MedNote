using Globalsys;
using Globalsys.Model;
using Globalsys.Validacao;
using LinqKit;
using MedNote.Dominio.MedNote;
using MedNote.Infra;
using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Linq;

namespace MedNote.API.Models.MedNote
{
    public class ParametrosBaseModel : IModel
    {
        private static ParametrosBaseModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static ParametrosBaseModel Instancia
        {
            get
            {
                if (model == null)
                    model = new ParametrosBaseModel();

                return model;
            }
        }

        public ParametrosBase Cadastrar(ParametrosBase parametrosBase)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                ParametrosBase parametrosBaseNew = new ParametrosBase();
                parametrosBaseNew.Descricao = parametrosBase.Descricao;
                parametrosBaseNew.Desativado = false;
                if(parametrosBase.Cliente != null)
                {
                    var cliente = contexto.ObterPorId<Cliente>(parametrosBase.Cliente.Codigo);
                    parametrosBaseNew.Cliente = cliente;
                }
                
                parametrosBaseNew.DataCadastro = DateTime.Now;
                parametrosBaseNew.Tipo = parametrosBase.Tipo;

                ValidarCampos(parametrosBaseNew, EstadoObjeto.Novo);
                contexto.Salvar<ParametrosBase>(parametrosBaseNew);

                return parametrosBaseNew;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public IQueryable<ParametrosBase> Consultar(int codigo, int tipo, string descricao = "")
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var predicate = PredicateBuilder.New<ParametrosBase>();
                predicate = predicate.And(p => p.Desativado == false);
                predicate = predicate.And(p => p.Tipo == tipo);
                //predicate = predicate.And(p => p.Cliente.Codigo == codigo);

                if (!string.IsNullOrWhiteSpace(descricao))
                {
                    descricao = descricao.Trim().ToLower();
                    predicate = predicate.And(p => p.Descricao.Trim().ToLower().Contains(descricao));
                }

                IQueryable<ParametrosBase> query = contexto.ObterTodos<ParametrosBase>().Where(predicate);
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ParametrosBase Atualizar(ParametrosBase parametrosBase, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                var parametrosBaseOld = contexto.ObterPorId<ParametrosBase>(codigo);
                parametrosBaseOld.Descricao = parametrosBase.Descricao;
                if (parametrosBase.Cliente != null)
                {
                    var cliente = contexto.ObterPorId<Cliente>(parametrosBase.Cliente.Codigo);
                    parametrosBaseOld.Cliente = cliente;
                }
                contexto.Atualizar(parametrosBaseOld);
                contexto.Commit();

                return parametrosBaseOld;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public ParametrosBase Editar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            return contexto.ObterPorId<ParametrosBase>(codigo);
        }

        public ParametrosBase Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                ParametrosBase parametrosBase = contexto.ObterPorId<ParametrosBase>(codigo);
                parametrosBase.DataDesativacao = DateTime.Now;
                parametrosBase.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(parametrosBase);
                contexto.Commit();

                return parametrosBase;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            ParametrosBase item = (ParametrosBase)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:
                    break;
            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {

        }
    }
}