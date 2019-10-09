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
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Repositorios.Seguranca;

namespace MedNote.API.Controllers.MedNote
{
    [RoutePrefix("api/painel")]
    public class PainelController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public PainelController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
        }

        [Authorize]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar()
        {
            Usuario usuario = Fabrica.Instancia.ObterRepositorio<IRepositorioUsuario>(UnidadeTrabalho).ObterUsuarioLogado();
            
            var setores = AdmissaoModel.Instancia.ConsultarSetores(usuario);
            var listaSetorFormatada = setores.Select(x => formatarDadosSetor(x)).ToList();
            return Json(listaSetorFormatada);
        }

        [Authorize]
        [Route("AbrirPainel")]
        [HttpGet]
        public Object AbrirPainel(int codigo)
        {
            var leitos = AdmissaoModel.Instancia.ConsultarLeitosDoSetor(codigo);
            return leitos.OrderBy(x => x.Identificador);
        }

        private Object formatarDadosSetor(Setor data)
        {

            var retorno = new
            {
                data.Codigo,
                TipoSetor = data.TipoSetor.Codigo,
                nomeTipoSetor = data.TipoSetor.Descricao,
                Estabelecimento = data.Estabelecimento.Codigo,
                EstabelecimentosNome = data.Estabelecimento.Nome,
                Cliente = data.Estabelecimento.Cliente.Codigo,
                ClienteNome = data.Estabelecimento.Cliente.Nome,
                data.Nome,
                QtdLeitos = data.Leitos.Count(),
                data.Desativado,
                DataCadastro = data.TryGetValue(v => v.DataCadastro).ToString("dd/MM/yyyy"),
             
            };
            return retorno;

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
    }
}