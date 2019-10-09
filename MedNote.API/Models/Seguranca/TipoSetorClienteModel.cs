using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Dominio.MedNote;
using LinqKit;
using Globalsys.Exceptions;

namespace MedNote.API.Models.Seguranca
{
    public class TipoSetorClienteModel : IModel
    {

        private static TipoSetorClienteModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static TipoSetorClienteModel Instancia
        {
            get
            {
                if (model == null)
                    model = new TipoSetorClienteModel();

                return model;
            }
        }

        public TipoSetorCliente Cadastrar(TipoSetorCliente tipoSetor)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                TipoSetorCliente tipoSetorNew = new TipoSetorCliente();
                tipoSetorNew.Desativado = false;
                var cliente = contexto.ObterPorId<Cliente>(tipoSetor.Cliente.Codigo);
                tipoSetorNew.Cliente = cliente;
                var tipo = contexto.ObterPorId<TipoSetor>(tipoSetor.TipoSetor.Codigo);
                tipoSetorNew.TipoSetor = tipo;
                tipoSetorNew.DataDeCadastro = DateTime.Now;

                ValidarCampos(tipoSetorNew, EstadoObjeto.Novo);
                ValidarRegras(tipoSetorNew, EstadoObjeto.Novo);
                contexto.Salvar(tipoSetorNew);

                return tipoSetorNew;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<TipoSetorCliente> Consultar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

                var predicate = PredicateBuilder.New<TipoSetorCliente>();
                predicate = predicate.And(p => p.Desativado == false);
                predicate = predicate.And(p => p.Cliente.Codigo == codigo);

                IQueryable<TipoSetorCliente> query = contexto.ObterTodos<TipoSetorCliente>().Where(predicate);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IQueryable<TipoSetorCliente> ObterTipoSetorPorCliente(int codigoCliente)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                IQueryable<TipoSetorCliente> query = contexto.ObterTodos<TipoSetorCliente>().Where(x => x.Desativado == false && x.Cliente.Codigo == codigoCliente);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public TipoSetorCliente Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                TipoSetorCliente tipoSetor = contexto.ObterPorId<TipoSetorCliente>(codigo);
                tipoSetor.DataDesativacao = DateTime.Now;
                tipoSetor.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(tipoSetor);
                contexto.Commit();

                return tipoSetor;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            TipoSetorCliente item = (TipoSetorCliente)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:

                    break;

            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            TipoSetorCliente item = (TipoSetorCliente)objeto;
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                    if (contexto.ObterTodos<TipoSetorCliente>().Where(t => t.Cliente.Codigo == item.Cliente.Codigo && t.TipoSetor.Codigo == item.TipoSetor.Codigo && !t.Desativado).Any())
                        throw new CoreException("Este tipo de setor já está adicionado para o cliente.");
                    break;

            }
        }
    }
}