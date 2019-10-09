using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using LinqKit;
using MedNote.Dominio.MedNote;
using MedNote.Dominio.MedNote.Enums;
using System.Collections.Generic;
using Globalsys.Extensoes;
using System.Globalization;

namespace MedNote.API.Models.MedNote
{
    public class SiglaDispositivoModel : IModel
    {
        private static SiglaDispositivoModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static SiglaDispositivoModel Instancia
        {
            get
            {
                if (model == null)
                    model = new SiglaDispositivoModel();

                return model;
            }
        }

        public DispositivoInternacaoFormatado Implantar(SiglaDispositivoFormatado siglaDispositivo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                SiglaDispositivo sigladispositivoNew = new SiglaDispositivo();
                Internacao internacao = contexto.ObterPorId<Internacao>(siglaDispositivo.CodigoInternacao);
                DispositivoDaInternacao dispositivoInternacaoNew = new DispositivoDaInternacao();

                sigladispositivoNew.CategoriaDispositivo = contexto.ObterPorId<CategoriaDispositivo>(siglaDispositivo.CategoriaDispositivo);
                sigladispositivoNew.SitioDispositivo = contexto.ObterPorId<SitioDispositivo>(siglaDispositivo.SitioDispositivo);
                sigladispositivoNew.TipoDispositivo = contexto.ObterPorId<TipoDispositivo>(siglaDispositivo.TipoDispositivo);
                sigladispositivoNew.Lateralidade = siglaDispositivo.Lateralidade;

                sigladispositivoNew.Sigla = siglaDispositivo.SiglaDispositivo.ToUpper();


                var sigladispositivoBase = contexto.ObterTodos<SiglaDispositivo>()
                                        .Where(x => x.Sigla == sigladispositivoNew.Sigla
                                                    && x.CategoriaDispositivo.Codigo == sigladispositivoNew.CategoriaDispositivo.Codigo
                                                    && x.SitioDispositivo.Codigo == sigladispositivoNew.SitioDispositivo.Codigo
                                                    && x.TipoDispositivo.Codigo == sigladispositivoNew.TipoDispositivo.Codigo).FirstOrDefault();

                if (sigladispositivoBase.IsNull())
                {
                    sigladispositivoNew.DataCadastro = DateTime.Now;
                    contexto.Salvar<SiglaDispositivo>(sigladispositivoNew);
                }
                else
                {
                    sigladispositivoNew = sigladispositivoBase;
                }

                dispositivoInternacaoNew.Internacao = internacao;
                dispositivoInternacaoNew.SiglaDispositivo = sigladispositivoNew;
                dispositivoInternacaoNew.DataCadastro = DateTime.Now;
                dispositivoInternacaoNew.DataImplante = siglaDispositivo.DataImplante;
                dispositivoInternacaoNew.DataRetirada = siglaDispositivo.DataRetirada;
                contexto.Salvar<DispositivoDaInternacao>(dispositivoInternacaoNew);


                DispositivoInternacaoFormatado retorno = new DispositivoInternacaoFormatado();
                retorno.Codigo = dispositivoInternacaoNew.Codigo;
                retorno.DataCadastroDispositivo = dispositivoInternacaoNew.DataCadastro;
                retorno.DataCadastroDispositivoFormatada = dispositivoInternacaoNew.Codigo != 0 ? dispositivoInternacaoNew.DataCadastro.ToString("dd/MM/yyyy") : null;
                retorno.DataImplante = dispositivoInternacaoNew.DataImplante;
                retorno.DataImplanteFormatada = dispositivoInternacaoNew.Codigo != 0 ? dispositivoInternacaoNew.DataImplante.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : null;
                retorno.DataRetirada = dispositivoInternacaoNew.DataRetirada;
                retorno.DataRetiradaFormatada = dispositivoInternacaoNew.DataRetirada != null ? Convert.ToDateTime(dispositivoInternacaoNew.DataRetirada).ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : null;
                retorno.Tempo = dispositivoInternacaoNew.DataRetirada == null ? "D" + Convert.ToInt16((DateTime.Now - dispositivoInternacaoNew.DataImplante).TotalDays) : null;
                retorno.DataCadastroSigla = dispositivoInternacaoNew.SiglaDispositivo.DataCadastro;
                retorno.DataCadastroSiglaFormatada = dispositivoInternacaoNew.SiglaDispositivo.Codigo != 0 ? dispositivoInternacaoNew.SiglaDispositivo.DataCadastro.ToString("dd/MM/yyyy") : null;
                retorno.TipoDispositivo = dispositivoInternacaoNew.SiglaDispositivo.TipoDispositivo.Codigo;
                retorno.TipoDispositivoDescricao = dispositivoInternacaoNew.SiglaDispositivo.TipoDispositivo.Descricao;
                retorno.CategoriaDispositivo = dispositivoInternacaoNew.SiglaDispositivo.TipoDispositivo.CategoriaDispositivo.Codigo;
                retorno.CategoriaDispositivoDescricao = dispositivoInternacaoNew.SiglaDispositivo.TipoDispositivo.CategoriaDispositivo.Descricao;
                retorno.SitioDispositivo = dispositivoInternacaoNew.SiglaDispositivo.SitioDispositivo.Codigo;
                retorno.SitioDispositivoDescricao = dispositivoInternacaoNew.SiglaDispositivo.SitioDispositivo.Descricao;
                retorno.SiglaDispositivo = dispositivoInternacaoNew.SiglaDispositivo.Sigla;
                retorno.Lateralidade = dispositivoInternacaoNew.SiglaDispositivo.Lateralidade;
                retorno.Internacao = dispositivoInternacaoNew.Internacao;
                retorno.CodigoInternacao = dispositivoInternacaoNew.Internacao.Codigo;

                contexto.Commit();

                return retorno;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public IQueryable<SiglaDispositivo> Consultar()
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            IQueryable<SiglaDispositivo> query = contexto.ObterTodos<SiglaDispositivo>().Where(x => x.DataDesativacao == null);

            return query;
        }

        public List<SiglaDispositivoFormatado> ConsultarFormatada(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                List<SiglaDispositivoFormatado> list = new List<SiglaDispositivoFormatado>();

                list = (from item in contexto.ObterTodos<DispositivoDaInternacao>().Where(t => t.Internacao.Codigo == codigo && t.DataDesativacao == null)

                        select new SiglaDispositivoFormatado()
                        {
                            Codigo = item.Codigo,
                            DataCadastro = item.SiglaDispositivo.DataCadastro,
                            DataCadastroFormatada = item.SiglaDispositivo.DataCadastro != null ? Convert.ToDateTime(item.SiglaDispositivo.DataCadastro).ToString("dd/MM/yyyy") : null,
                            TipoDispositivo = item.SiglaDispositivo.TipoDispositivo.Codigo,
                            TipoDispositivoDescricao = item.SiglaDispositivo.TipoDispositivo.Descricao,
                            SitioDispositivo = item.SiglaDispositivo.SitioDispositivo.Codigo,
                            SitioDispositivoDescricao = item.SiglaDispositivo.SitioDispositivo.Descricao,
                            CategoriaDispositivo = item.SiglaDispositivo.CategoriaDispositivo.Codigo,
                            CategoriaDispositivoDescricao = item.SiglaDispositivo.CategoriaDispositivo.Descricao,
                            Lateralidade = item.SiglaDispositivo.Lateralidade,
                            SiglaDispositivo = item.SiglaDispositivo.Sigla,
                            DataCadastroDipositivo = item.DataCadastro,
                            DataCadastroDipositivoFormatada = item.DataCadastro != null ? Convert.ToDateTime(item.DataCadastro).ToString("dd/MM/yyyy") : null,
                            DataImplante = item.DataImplante,
                            DataImplanteFormatada = item.DataImplante != null ? Convert.ToDateTime(item.DataImplante).ToString("dd/MM/yyyy HH:mm") : null,
                            DataRetirada = item.DataRetirada,
                            DataRetiradaFormatada = item.DataRetirada != null ? Convert.ToDateTime(item.DataRetirada).ToString("dd/MM/yyyy HH:mm") : null,
                            Internacao = item.Internacao,
                            CodigoInternacao = item.Internacao.Codigo

                        }).ToList();

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            var contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
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
                contexto.Rollback();
                throw ex;
            }
        }

        public DispositivoDaInternacao DeletarDispositivoInternacao(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {

                contexto.BeginTransaction();
                var dispositivo = contexto.ObterPorId<DispositivoDaInternacao>(codigo);

                contexto.Remover<DispositivoDaInternacao>(dispositivo);
                contexto.Commit();

                return dispositivo;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public DispositivoDaInternacao EditarDispositivoInternacao(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                var dispositivo = contexto.ObterPorId<DispositivoDaInternacao>(codigo);
                return dispositivo;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public List<DispositivoInternacaoFormatado> ConsultarDispositivoInternacao(int internacao, int dispInternacao)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var predicate = PredicateBuilder.New<DispositivoDaInternacao>();
                predicate = predicate.And(x => x.DataDesativacao == null && x.Internacao.Codigo == internacao && x.SiglaDispositivo.Codigo > 0);

                if (dispInternacao > 0)
                    predicate = predicate.And(p => p.Codigo == dispInternacao);

                var query = contexto.ObterTodos<DispositivoDaInternacao>().Where(predicate)
                    .Select(dispositivo => new DispositivoInternacaoFormatado
                    {
                        Codigo = dispositivo.Codigo,
                        DataCadastroDispositivo = dispositivo.DataCadastro,
                        DataCadastroDispositivoFormatada = dispositivo.Codigo != 0 ? dispositivo.DataCadastro.ToString("dd/MM/yyyy") : null,
                        DataImplante = dispositivo.DataImplante,
                        DataImplanteFormatada = dispositivo.Codigo != 0 ? dispositivo.DataImplante.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) : null,
                        DataRetirada = dispositivo.DataRetirada,
                        DataRetiradaFormatada = dispositivo.DataRetirada != null ? Convert.ToDateTime(dispositivo.DataRetirada).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null,
                        Tempo = dispositivo.DataRetirada == null ? "D" + Convert.ToInt16((DateTime.Now - dispositivo.DataImplante).TotalDays) : null,
                        DataCadastroSigla = dispositivo.SiglaDispositivo.DataCadastro,
                        DataCadastroSiglaFormatada = dispositivo.SiglaDispositivo.Codigo != 0 ? dispositivo.SiglaDispositivo.DataCadastro.ToString("dd/MM/yyyy") : null,
                        TipoDispositivo = dispositivo.SiglaDispositivo.TipoDispositivo.Codigo,
                        TipoDispositivoDescricao = dispositivo.SiglaDispositivo.TipoDispositivo.Descricao,
                        CategoriaDispositivo = dispositivo.SiglaDispositivo.TipoDispositivo.CategoriaDispositivo.Codigo,
                        CategoriaDispositivoDescricao = dispositivo.SiglaDispositivo.TipoDispositivo.CategoriaDispositivo.Descricao,
                        SitioDispositivo = dispositivo.SiglaDispositivo.SitioDispositivo.Codigo,
                        SitioDispositivoDescricao = dispositivo.SiglaDispositivo.SitioDispositivo.Descricao,
                        SiglaDispositivo = dispositivo.SiglaDispositivo.Sigla,
                        Lateralidade = dispositivo.SiglaDispositivo.Lateralidade,
                        Internacao = dispositivo.Internacao,
                        CodigoInternacao = dispositivo.Internacao.Codigo,
                        NomePaciente = dispositivo.Internacao.Paciente.Nome,
                        IsOpen = false
                    }).ToList();

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        public DispositivoInternacaoFormatado AtualizarDispositivoInternacao(int codigo, DispositivoDaInternacao dispositivo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();
                DispositivoDaInternacao dispositivoInternacao = contexto.ObterPorId<DispositivoDaInternacao>(codigo);

                dispositivoInternacao.DataRetirada = dispositivo.DataRetirada;

                contexto.Atualizar(dispositivoInternacao);
                contexto.Commit();

                var retorno = ConsultarDispositivoInternacao(dispositivoInternacao.Internacao.Codigo, dispositivoInternacao.Codigo).FirstOrDefault();

                return retorno;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public IQueryable<CategoriaDispositivo> ConsultarCategoria()
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            IQueryable<CategoriaDispositivo> query = contexto.ObterTodos<CategoriaDispositivo>().Where(x => x.DataDesativacao == null);

            return query;
        }

        public IQueryable<TipoDispositivo> ConsultarTipo(int categoria)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            IQueryable<TipoDispositivo> query = contexto.ObterTodos<TipoDispositivo>().Where(x => x.DataDesativacao == null && x.CategoriaDispositivo.Codigo == categoria);

            return query;
        }

        public IQueryable<SitioDispositivo> ConsultarSitio(int tipo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            IQueryable<SitioDispositivo> query = contexto.ObterTodos<SitioDispositivo>().Where(x => x.DataDesativacao == null && x.TipoDispositivo.Codigo == tipo);

            return query;
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