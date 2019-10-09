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
    public class DispositivoInternacaoModel : IModel
    {
        private static DispositivoInternacaoModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static DispositivoInternacaoModel Instancia
        {
            get
            {
                if (model == null)
                    model = new DispositivoInternacaoModel();

                return model;
            }
        }

        public DispositivoDaInternacao Cadastrar(DispositivoDaInternacao dispositivoInternacao)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                dispositivoInternacao.DataCadastro = DateTime.Now;
                contexto.Salvar<DispositivoDaInternacao>(dispositivoInternacao);
                return dispositivoInternacao;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<DispositivoDaInternacao> Consultar(int internacao)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            IQueryable<DispositivoDaInternacao> query = contexto.ObterTodos<DispositivoDaInternacao>().Where(x => x.DataDesativacao == null && x.Internacao.Codigo == internacao);

            return query;
        }

      
        public SiglaDispositivo Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                return contexto.ObterPorId<SiglaDispositivo>(codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public DispositivoDaInternacao IncluirDispositivoInternacao(DispositivoDaInternacao dispositivoInternacao)
        {
            try
            {
                var contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                contexto.BeginTransaction();
                SiglaDispositivo sigla;

                if (dispositivoInternacao.SiglaDispositivo != null)
                    sigla = contexto.ObterPorId<SiglaDispositivo>(dispositivoInternacao.SiglaDispositivo.Codigo);
                else
                    sigla = null;

                var itemEmEdicao = new DispositivoDaInternacao();
                itemEmEdicao.DataCadastro = DateTime.Now;
                itemEmEdicao.SiglaDispositivo = sigla;
                itemEmEdicao.DataImplante = dispositivoInternacao.DataImplante;
                itemEmEdicao.DataRetirada = dispositivoInternacao.DataRetirada;
                itemEmEdicao.Internacao = dispositivoInternacao.Internacao;

                if (itemEmEdicao.SiglaDispositivo != null)
                    contexto.Salvar<DispositivoDaInternacao>(itemEmEdicao);
             
                contexto.Commit();

                return itemEmEdicao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DispositivoDaInternacao DeletarDispositivoInternacao(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

                contexto.BeginTransaction();
                var dispositivo = contexto.ObterPorId<DispositivoDaInternacao>(codigo);

                contexto.Remover<DispositivoDaInternacao>(dispositivo);
                contexto.Commit();

                return dispositivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DispositivoDaInternacao EditarDispositivoInternacao(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var dispositivo = contexto.ObterPorId<DispositivoDaInternacao>(codigo);
                return dispositivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DispositivoDaInternacao AtualizarDispositivoInternacao(DispositivoDaInternacao dispositivo, int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                contexto.BeginTransaction();
                DispositivoDaInternacao dispositivoInternacao = contexto.ObterPorId<DispositivoDaInternacao>(codigo);

                dispositivoInternacao.DataCadastro = DateTime.Now;
                dispositivoInternacao.SiglaDispositivo = dispositivoInternacao.SiglaDispositivo;
                dispositivoInternacao.DataImplante = dispositivoInternacao.DataImplante;
                dispositivoInternacao.DataRetirada = dispositivoInternacao.DataRetirada;
                dispositivoInternacao.Internacao = dispositivoInternacao.Internacao ?? null;

                contexto.Atualizar(dispositivoInternacao);
                contexto.Commit();
                return dispositivoInternacao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Admissao item = (Admissao)objeto;
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