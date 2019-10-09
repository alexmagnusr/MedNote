using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using LinqKit;
using MedNote.Dominio.MedNote;

namespace MedNote.API.Models.MedNote
{
    public class InternacaoModel : IModel
    {

        private static InternacaoModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static InternacaoModel Instancia
        {
            get
            {
                if (model == null)
                    model = new InternacaoModel();

                return model;
            }
        }

        public Internacao Cadastrar(Internacao internacao)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

                Internacao internacaoNew = new Internacao();

                return internacaoNew;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<Internacao> Consultar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var predicate = PredicateBuilder.New<Internacao>();
                predicate = predicate.And(p => p.Desativado == false);
                IQueryable<Internacao> query = contexto.ObterTodos<Internacao>().Where(predicate);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Internacao Atualizar(Internacao internacao, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                var internacaoOld = contexto.ObterPorId<Internacao>(codigo);
                //internacaoOld.CodigoConvenio = internacao.CodigoConvenio;
                internacaoOld.DataInternacao = internacao.DataInternacao;
                internacaoOld.NumeroAtendimento = internacao.NumeroAtendimento;
                internacaoOld.Paciente = contexto.ObterPorId<Paciente>(internacao.Paciente.Codigo);

                contexto.Atualizar(internacaoOld);
                contexto.Commit();
                return internacaoOld;

            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public Internacao Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                return contexto.ObterPorId<Internacao>(codigo);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Internacao Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                Internacao internacao = contexto.ObterPorId<Internacao>(codigo);
                internacao.DataDesativacao = DateTime.Now;
                internacao.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(internacao);
                contexto.Commit();

                return internacao;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }

        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Internacao item = (Internacao)objeto;
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