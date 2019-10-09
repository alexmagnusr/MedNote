using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Ative;
using MedNote.Infra;
using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Globalsys.Extensoes;
using MedNote.Dominio.MedNote;
using MedNote.API.Models.MedNote;
using MedNote.Dominio.DTOs;
using MedNote.Dominio.MedNote.Enums;

namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/admissao")]
    public class AdmissaoController : BaseApiController
    {

        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public AdmissaoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            AdmissaoModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(int codigo)
        {
            var admissoes = AdmissaoModel.Instancia.Consultar(codigo).ToList().OrderByDescending(u => u.DataAdmissao);
            var admissoesFormatada = admissoes.Select(x => formatarDados(x, false)).ToList();
            return Json(admissoesFormatada);
        }

        [Authorize]
        [Route("PacienteInternacao")]
        [HttpGet]
        public Object ConsultarPacienteInternacao(int id)
        {
            try
            {
                var paciente = AdmissaoModel.Instancia.ConsultarPacienteInternacao(id).FirstOrDefault();
                return Json(formatarDadosPaciente(paciente));
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        [Authorize]
        [Route("ConsultarPacientes")]
        [HttpGet]
        public Object ConsultarPacientes(string nome, int codigoSetor)
        {
            var pacientes = PacienteModel.Instancia.Consultar(nome, codigoSetor).ToList();
            var pacientesFormatada = pacientes.Select(x => formatarDadosPaciente(x)).ToList();
            return Json(pacientesFormatada);
        }

        [Authorize]
        [Route("ConsultarConvenios")]
        [HttpGet]
        public Object ConsultarConvenios(int codigo)
        {
            var convenios = ParametrosBaseModel.Instancia.Consultar(codigo, (int)EnumParametrosBase.Convenio).ToList();
            var formats = convenios.Select(x => formatarConvenio(x)).ToList();
            return Json(formats);
        }

        

        [Authorize]
        public Object Post([FromBody] AdmissaoPacienteDTO admissao)
        {
            admissao.Genero = admissao.Genero == "1" || admissao.Genero == "M" ? "M" : "F";

            return formatarDados(AdmissaoModel.Instancia.Cadastrar(admissao));
        }

        [Authorize]
        public Object Get(int id)
        {
            return formatarDados(AdmissaoModel.Instancia.Editar(id), true);
        }

        [Authorize]
        public Object Put([FromBody]AdmissaoPacienteDTO admissao)
        {
            admissao.Genero = admissao.Genero == "1" || admissao.Genero == "M" ? "M" : "F";

            return Json(formatarDados(AdmissaoModel.Instancia.Atualizar(admissao)));
        }

        [Authorize]
        public Object Delete(int id)
        {
            return Json(formatarDados(AdmissaoModel.Instancia.Deletar(id)));
        }

        private AdmissaoPacienteDTO formatarDados(Admissao data, bool editar = true)
        {

            AdmissaoPacienteDTO retorno = new AdmissaoPacienteDTO();
            retorno.Codigo = data.Codigo;
            retorno.CodigoPaciente = data.Internacao.Paciente.Codigo;
            retorno.CodigoInternacao = data.Internacao.Codigo;
            retorno.CodigoSetor = data.Setor.Codigo;
            retorno.CodigoLeito = data.Leito.IsNull() ? 0 : data.Leito.Codigo;
            retorno.CodigoEstabelecimeto = data.Setor.Estabelecimento.Codigo;
            retorno.NomeEstabelecimeto = data.Setor.Estabelecimento.Nome;
            retorno.NumeroAtendimento = data.Internacao.NumeroAtendimento;
            retorno.NomePaciente = data.Internacao.Paciente.Nome;
            retorno.DataNascimento = data.Internacao.Paciente.DataNascimento;
            retorno.DataNascimentoFormat = data.Internacao.Paciente.DataNascimento.ToString("dd/MM/yyyy");
            retorno.Idade = calculaIdade(data.Internacao.Paciente.DataNascimento);
            retorno.Documento = data.Internacao.Paciente.Documento;
            retorno.TipoDocumento = data.Internacao.Paciente.TipoDocumento;
            retorno.Genero = data.Internacao.Paciente.Genero;
            retorno.NumProntuario = data.Internacao.Paciente.NumProntuario;
            retorno.CodigoConvenio = data.Internacao.Convenio.Codigo;
            retorno.NomeConvenio = data.Internacao.Convenio.Descricao;
            retorno.NomeSetor = data.Setor.Nome;
            retorno.NomeLeito = data.Leito.IsNull() ? "" : data.Leito.Identificador;
            retorno.DataAdmissao = data.DataAdmissao;
            retorno.DataCadastro = data.DataCadastro;
            retorno.DataInternacao = data.Internacao.DataInternacao;
            retorno.DataCadastroInternacao = data.Internacao.DataCadastro;
            retorno.DataDesativacao = data.DataDesativacao;
            retorno.DataAdmissaoFormat = data.TryGetValue(v => v.DataAdmissao).ToString("dd/MM/yyyy");
            retorno.DataCadastroFormat = data.TryGetValue(v => v.DataCadastro).ToString("dd/MM/yyyy");
            retorno.DataInternacaoFormat = data.TryGetValue(v => v.Internacao.DataInternacao).ToString("dd/MM/yyyy");
            retorno.DataCadastroInternacaoFormat = data.TryGetValue(v => v.Internacao.DataCadastro).ToString("dd/MM/yyyy");
            retorno.DataDesativacaoFormat = data.TryGetValue(v => v.DataDesativacao).ToString("dd/MM/yyyy");
            retorno.Desativado = data.Desativado;
            retorno.Editar = data.Editar;
          
            return retorno;

        }
        private Object formatarDadosPaciente(Paciente p)
        {
            var retorno = new
            {
                p.Codigo,
                p.DataCadastro,
                p.DataDesativacao,
                p.DataNascimento,
                p.Desativado,
                p.Documento,
                p.Genero,
                p.Nome,
                p.NumProntuario,
                p.TipoDocumento
            };
            return retorno;
        }
        private Object formatarConvenio(ParametrosBase p)
        {
            var retorno = new
            {
                p.Codigo,
                p.DataCadastro,
                p.DataDesativacao,
                p.Descricao,
                p.Desativado,
                p.Tipo
            };
            return retorno;
        }

        public int calculaIdade(DateTime nasc)
        {
            int idade;
            DateTime hoje = DateTime.Now;
            idade = hoje.Year - nasc.Year;
            if (nasc > hoje.AddYears(-idade)) idade--;
            return idade;
        }
    }
}