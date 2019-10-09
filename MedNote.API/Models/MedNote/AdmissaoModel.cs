using Globalsys.Model;
using System;
using System.Linq;
using Globalsys.Validacao;
using Globalsys;
using MedNote.Infra;
using LinqKit;
using MedNote.Dominio.MedNote;
using MedNote.Dominio.DTOs;
using System.Collections.Generic;
using Globalsys.Exceptions;
using Globalsys.Extensoes;
using MedNote.Dominio.MedNote.Enums;
using MedNote.Infra.Dominio.Seguranca;
using VIX.Persistencia.Repositorio.Seguranca;

namespace MedNote.API.Models.MedNote
{
    public class AdmissaoModel : IModel
    {
        private static AdmissaoModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static AdmissaoModel Instancia
        {
            get
            {
                if (model == null)
                    model = new AdmissaoModel();

                return model;
            }
        }

        public IQueryable<Setor> ConsultarSetores(Usuario usuario)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                //Usuario usuario = new RepositorioUsuario(UnidadeDeTrabalho).ObterUsuarioLogado();
                //Busca apenas setores de acesso do Estabelecimento
                if (!usuario.AdminCliente && !usuario.Admin)
                {
                    IQueryable<SetorUsuario> setoresUsuario = contexto.ObterTodos<SetorUsuario>().Where(s => !s.Desativado && s.Usuario.Codigo == usuario.Codigo);
                    return setoresUsuario.Select(s => s.Setor);
                }
                //Busca todos os Setores do Cliente
                else if (usuario.AdminCliente)
                {
                    IQueryable<Setor> setores = contexto.ObterTodos<Setor>().Where(s => !s.Desativado
                    && s.Estabelecimento.Cliente.Codigo == usuario.Cliente.Codigo);

                    return setores;
                }
                else if (usuario.Admin)
                {
                    IQueryable<Setor> setores = contexto.ObterTodos<Setor>().Where(s => !s.Desativado);
                    return setores;
                }

                else
                    throw new CoreException("Nenhum setor disponível para este usuário.");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public IQueryable<Admissao> Consultar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var predicate = PredicateBuilder.New<Admissao>();
                predicate = predicate.And(p => p.Desativado == false);
                predicate = predicate.And(p => p.Setor.Estabelecimento.Cliente.Codigo == codigo);
                IQueryable<Admissao> query = contexto.ObterTodos<Admissao>().Where(predicate);

                return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IQueryable<Paciente> ConsultarPacienteInternacao(int internacao)
        {
            try
            {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            IQueryable<Paciente> query = contexto.ObterTodos<Admissao>().Where(x => x.Internacao.Codigo == internacao).Select(g => g.Internacao.Paciente);
            return query;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<PainelDTO> ConsultarLeitosDoSetor(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                IQueryable<Admissao> admissoes = contexto.ObterTodos<Admissao>().Where(p => p.Desativado == false && p.Setor.Codigo == codigo);
                IQueryable<Leito> leitos = contexto.ObterTodos<Leito>().Where(p => p.Desativado == false && p.Setor.Codigo == codigo);

                return montaDTO(admissoes, leitos).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IEnumerable<PainelDTO> montaDTO(IQueryable<Admissao> admissoes, IQueryable<Leito> leitos)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            IList<PainelDTO> painel = new List<PainelDTO>();
            decimal ocupacao = leitos.Where(x => x.Bl_Liberado == false).Count() * 100 / leitos.Count();


            foreach (Leito l in leitos)
            {
                PainelDTO item = new PainelDTO
                {
                    CodigoLeito = l.Codigo,
                    CodigoSetor = l.Setor.Codigo,
                    Identificador = l.Identificador,
                    Liberado = l.Bl_Liberado,
                    NomeSetor = l.Setor.Nome,
                    NomeEstabelecimento = l.Setor.Estabelecimento.Nome
                };

                Admissao ad = new Admissao();
                List<string> infoDisp = new List<string>();
                ad = admissoes.Where(a => a.Leito.Codigo == l.Codigo).FirstOrDefault();

                if (ad != null)
                {
                    item.Admissao = ad;
                    item.DataAdmissao = ad.DataAdmissao;
                    item.CodigoAdmissao = ad.Codigo;
                    item.DataAdmissaoFormat = ad.DataAdmissao.ToString("dd/MM/yyyy");
                    item.NomePaciente = ad.Internacao.Paciente.Nome;
                    item.InfoSetor = "D" + Convert.ToInt16((DateTime.Now - ad.DataAdmissao).TotalDays) + " " + l.Setor.Nome.Substring(0, Math.Min(l.Setor.Nome.Length, 18));
                    var dispositivo = contexto.ObterTodos<DispositivoDaInternacao>()
                                          .Where(p => p.Internacao.Codigo == ad.Internacao.Codigo
                                                      && p.DataDesativacao == null
                                                      && p.DataRetirada == null).ToList().OrderByDescending(x => x.DataImplante);
                    foreach (DispositivoDaInternacao d in dispositivo)
                    {
                        infoDisp.Add("D" + Convert.ToInt16((DateTime.Now - d.DataImplante).TotalDays) + " " + d.SiglaDispositivo.Sigla.Substring(0, Math.Min(d.SiglaDispositivo.Sigla.Length, 18)));
                    }
                    item.Dispositivos = infoDisp;
                }

                item.TaxaOcupacao = ocupacao;

                painel.Add(item);
            }
            return painel.Take(12).OrderBy(x => x.Identificador);
        }

        //O sistema não possui uma tela cadastro de Clientes, logo a inclusão/edição será feita na Admissão
        public Admissao Cadastrar(AdmissaoPacienteDTO admissao)
        {

            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            try
            {
                contexto.BeginTransaction();

                Paciente pacienteNew = contexto.ObterPorId<Paciente>(admissao.CodigoPaciente);
                Internacao internacaoNew = contexto.ObterPorId<Internacao>(admissao.CodigoInternacao);
                Admissao admissaoNew = new Admissao();

                if (pacienteNew == null)
                    pacienteNew = new Paciente();


                if (internacaoNew == null)
                    internacaoNew = new Internacao();


                //PACIENTE
                pacienteNew.Nome = admissao.NomePaciente;
                pacienteNew.Documento = admissao.Documento;
                pacienteNew.Genero = admissao.Genero;
                pacienteNew.DataNascimento = admissao.DataNascimento;
                pacienteNew.NumProntuario = admissao.NumProntuario;
                pacienteNew.TipoDocumento = admissao.TipoDocumento;

                Setor setor = contexto.ObterPorId<Setor>(admissao.CodigoSetor);
                pacienteNew.EstabelecimentoSaude = contexto.ObterPorId<EstabelecimentoSaude>(setor.Estabelecimento.Codigo);

                if (pacienteNew.Codigo == 0)
                {
                    pacienteNew.Desativado = false;
                    pacienteNew.DataCadastro = DateTime.Now;

                    ValidarCamposPaciente(pacienteNew, EstadoObjeto.Novo);
                    ValidarRegrasPaciente(pacienteNew, EstadoObjeto.Novo);

                    contexto.Salvar<Paciente>(pacienteNew);
                }
                else
                {
                    ValidarCamposPaciente(pacienteNew, EstadoObjeto.Alterado);
                    ValidarRegrasPaciente(pacienteNew, EstadoObjeto.Alterado);

                    contexto.Atualizar<Paciente>(pacienteNew);
                }

                //INTERNACAO
                //internacaoNew.CodigoConvenio = admissao.CodigoConvenio;
                var convenio = contexto.ObterPorId<ParametrosBase>(admissao.CodigoConvenio);
                internacaoNew.Convenio = convenio;//new ParametrosBase { Codigo = internacaoNew.CodigoConvenio };
                internacaoNew.NumeroAtendimento = admissao.NumeroAtendimento;
                //internacaoNew.CodigoPaciente = pacienteNew.Codigo;
                internacaoNew.Paciente = new Paciente { Codigo = pacienteNew.Codigo };

                if (internacaoNew.Codigo == 0)
                {
                    internacaoNew.Desativado = false;
                    internacaoNew.DataCadastro = DateTime.Now;
                    internacaoNew.DataInternacao = DateTime.Now;

                    ValidarCamposInternacao(internacaoNew, EstadoObjeto.Novo);

                    contexto.Salvar<Internacao>(internacaoNew);
                }
                else
                {
                    ValidarCamposInternacao(internacaoNew, EstadoObjeto.Alterado);

                    contexto.Atualizar<Internacao>(internacaoNew);
                }

                //ADMISSAO
                admissaoNew.Desativado = false;
                admissaoNew.DataCadastro = DateTime.Now;
                admissaoNew.DataAdmissao = DateTime.Now;
                admissaoNew.Editar = true;


                var internacao = contexto.ObterPorId<Internacao>(internacaoNew.Codigo);
                admissaoNew.Internacao = new Internacao { Codigo = internacao.Codigo };
                admissaoNew.Internacao.Paciente = new Paciente { Codigo = pacienteNew.Codigo };

                admissaoNew.Setor = setor;

                var leito = contexto.ObterPorId<Leito>(admissao.CodigoLeito);
                leito.Bl_Liberado = false;
                admissaoNew.Leito = leito;

                ValidarCampos(admissaoNew, EstadoObjeto.Novo);
                ValidarRegras(admissaoNew, EstadoObjeto.Novo);

                contexto.Salvar<Admissao>(admissaoNew);

                internacaoNew.Paciente = pacienteNew;

                admissaoNew.Internacao = internacaoNew;
                contexto.Commit();
                return admissaoNew;

            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        //O sistema não possui uma tela cadastro de Clientes, logo a inclusão/edição será feita na Admissão
        public Admissao Atualizar(AdmissaoPacienteDTO admissao)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();

            try
            {
                contexto.BeginTransaction();

                Paciente pacienteNew = contexto.ObterPorId<Paciente>(admissao.CodigoPaciente);
                Internacao internacaoNew = contexto.ObterPorId<Internacao>(admissao.CodigoInternacao);
                Admissao admissaoNew = contexto.ObterPorId<Admissao>(admissao.Codigo);

                //PACIENTE
                pacienteNew.Nome = admissao.NomePaciente;
                pacienteNew.Documento = admissao.Documento;
                pacienteNew.Genero = admissao.Genero;
                pacienteNew.DataNascimento = admissao.DataNascimento;
                pacienteNew.NumProntuario = admissao.NumProntuario;
                pacienteNew.TipoDocumento = admissao.TipoDocumento;

                Setor setor = contexto.ObterPorId<Setor>(admissao.CodigoSetor);
                pacienteNew.EstabelecimentoSaude = contexto.ObterPorId<EstabelecimentoSaude>(setor.Estabelecimento.Codigo);

                ValidarCamposPaciente(pacienteNew, EstadoObjeto.Alterado);
                ValidarRegrasPaciente(pacienteNew, EstadoObjeto.Alterado);
                contexto.Atualizar(pacienteNew);

                //INTERNACAO
                var convenio = contexto.ObterPorId<ParametrosBase>(admissao.CodigoConvenio);
                internacaoNew.Convenio = convenio;//new ParametrosBase { Codigo = internacaoNew.CodigoConvenio };
                internacaoNew.NumeroAtendimento = admissao.NumeroAtendimento;
                internacaoNew.Paciente = new Paciente { Codigo = pacienteNew.Codigo };

                ValidarCamposInternacao(internacaoNew, EstadoObjeto.Alterado);

                contexto.Atualizar(internacaoNew);

                //ADMISSAO
                admissaoNew.DataAdmissao = admissao.DataAdmissao;
                var internacao = contexto.ObterPorId<Internacao>(admissao.CodigoInternacao);
                admissaoNew.Internacao = new Internacao { Codigo = internacao.Codigo };
                admissaoNew.Internacao.Paciente = new Paciente { Codigo = pacienteNew.Codigo };

                admissaoNew.Setor = setor;

                var leito = contexto.ObterPorId<Leito>(admissao.CodigoLeito);
                admissaoNew.Leito = leito;

                ValidarCampos(admissaoNew, EstadoObjeto.Alterado);
                ValidarRegras(admissaoNew, EstadoObjeto.Alterado);

                contexto.Atualizar(admissaoNew);

                contexto.Commit();

                //Load de Objetos
                internacaoNew.Paciente = pacienteNew;
                admissaoNew.Internacao = internacaoNew;
                return admissaoNew;

            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public Admissao Editar(int codigo)
        {
            try
            {
                IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                return contexto.ObterPorId<Admissao>(codigo);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Admissao Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                Admissao admissao = contexto.ObterPorId<Admissao>(codigo);
                admissao.DataDesativacao = DateTime.Now;
                admissao.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(admissao);
                contexto.Commit();

                return admissao;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
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
                    if (item.DataAdmissao == null)
                        throw new CoreException("A Data de Adimissão é obrigatória.");
                    break;

            }
        }

        public void ValidarCamposPaciente(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Paciente item = (Paciente)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:
                    if (item.TipoDocumento == (int)EnumTipoDocumento.CPF && !item.Documento.IsValidCPF())
                        throw new CoreException("CPF informado inválido.");
                    if (item.DataNascimento.IsEmpty()
                        || item.Documento.IsEmpty()
                        || item.Genero.IsEmpty()
                        || item.NumProntuario.IsEmpty()
                        || item.Nome.IsEmpty()
                        || item.TipoDocumento.IsEmpty())
                        throw new CoreException("O Paciente possui campos obrigatórios que não foram preenchidos.");
                    break;

            }
        }

        public void ValidarCamposInternacao(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Internacao item = (Internacao)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:
                    if (item.NumeroAtendimento.IsEmpty()
                        || item.Convenio.IsEmpty())
                        throw new CoreException("A Internação possui campos obrigatórios que não foram preenchidos.");
                    break;

            }
        }


        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Admissao item = (Admissao)objeto;
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            IQueryable<Admissao> admissoes = contexto.ObterTodos<Admissao>().Where(a => !a.Desativado
            && a.Internacao.Paciente.Codigo == item.Internacao.Paciente.Codigo
            && a.Codigo != item.Codigo);

            IQueryable<Admissao> admissoesLeito = contexto.ObterTodos<Admissao>().Where(a => !a.Desativado
            && a.Leito.Codigo == item.Leito.Codigo
            && a.Codigo != item.Codigo);

            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:

                    if (admissoes.Count() > 0)
                        throw new CoreException("O paciente já está alocado a outro leito.");
                    if (admissoesLeito.Count() > 0)
                        throw new CoreException("O leito encontra-se ocupado por outro paciente.");
                    break;
            }
        }
        public void ValidarRegrasPaciente(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Paciente item = (Paciente)objeto;
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();


            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:

                    IQueryable<Paciente> pacientes = contexto.ObterTodos<Paciente>().Where(p =>
                        !p.Desativado
                        && p.EstabelecimentoSaude.Codigo == item.EstabelecimentoSaude.Codigo
                        && ((p.Documento.Equals(item.Documento) && p.TipoDocumento == item.TipoDocumento)
                        || p.NumProntuario == item.NumProntuario)
                        );


                    if (pacientes.Count() > 0)
                    {
                        if (pacientes.Any(p => p.Documento.Equals(item.Documento) && p.TipoDocumento == item.TipoDocumento))
                            throw new CoreException("Já existe um paciente cadastrado com o Documento informado.");
                        if (pacientes.Any(p => p.NumProntuario == item.NumProntuario))
                            throw new CoreException("Já existe um paciente cadastrado com o Número de Prontuário informado.");
                    }
                    break;
                case EstadoObjeto.Alterado:

                    pacientes = contexto.ObterTodos<Paciente>().Where(p =>
                        !p.Desativado
                        && p.Codigo != item.Codigo
                        && p.EstabelecimentoSaude.Codigo == item.EstabelecimentoSaude.Codigo
                        && ((p.Documento.Equals(item.Documento) && p.TipoDocumento == item.TipoDocumento)
                            || p.NumProntuario == item.NumProntuario)
                    );


                    if (pacientes.Count() > 0)
                    {
                        if (pacientes.Any(p => p.Documento.Equals(item.Documento) && p.TipoDocumento == item.TipoDocumento))
                            throw new CoreException("Já existe um paciente cadastrado com o Documento informado.");
                        if (pacientes.Any(p => p.NumProntuario == item.NumProntuario))
                            throw new CoreException("Já existe um paciente cadastrado com o Número de Prontuário informado.");
                    }
                    break;
            }
        }
    }
}