using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Ative;
using MedNote.Infra;
using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Globalsys.Extensoes;
using MedNote.Infra.Dominio.Seguranca;

namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/estabelecimentoSaude")]
    public class EstabelecimentoSaudeController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public EstabelecimentoSaudeController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            EstabelecimentoSaudeModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]EstabelecimentoSaude estabelecimentoSaude)
        {
            var retorno = formatarDados(EstabelecimentoSaudeModel.Instancia.Cadastrar(estabelecimentoSaude));
            return retorno;
        }

        //[Authorize]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar( int codigo)
        {
            try
            {
                var listaEstabelecimento = EstabelecimentoSaudeModel.Instancia.Consultar(codigo).ToList();
                var listaEstabelecimentoFormatada = listaEstabelecimento.Select(x => formatarDados(x, false)).ToList();
                return Json(listaEstabelecimentoFormatada);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        [Authorize]
        public Object Get(int id)
        {
            return formatarDados(EstabelecimentoSaudeModel.Instancia.Editar(id), true);
        }

        [Authorize]
        [HttpPost]
        [Route("Atualiza")]
        public Object Put(int id, [FromBody]EstabelecimentoSaude estabelecimentoSaude)
        {
            return Json(formatarDados(EstabelecimentoSaudeModel.Instancia.Atualizar(estabelecimentoSaude, id)));
        }

        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formatarDados(EstabelecimentoSaudeModel.Instancia.Deletar(id)));
        }

        private Object formatarDados(EstabelecimentoSaude data, bool editar = true)
        {
            var retorno = new
            {
                data.Codigo,
                codigoCliente = data.Cliente.Codigo,
                nomeCliente = data.Cliente.Nome,
                data.Nome,
                data.Telefone,
                TelefoneFomart = data.Telefone.ToPhoneNumberFormat(),
                data.Email,
                DataCadastro = data.TryGetValue(v => v.DataDeCadastro).ToString("dd/MM/yyyy")
            };
            return retorno;

        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarEstabelecimentoPorCliente/{codigo}")]
        public Object ConsultarUsuarioPorTipoColaborador(int codigo)
        {
            try
            {
                var estabelecimento = EstabelecimentoSaudeModel.Instancia.ObterEstabelecimentoPorcliente(codigo)
                                   .Select(
                                               p => new
                                               {
                                                   p.Codigo,
                                                   p.Nome,
                                                   Cliente = p.Cliente.Codigo,
                                                   ClienteDesc = p.Cliente.Nome,
                                                   p.Telefone,
                                                   p.Email
                                               }
                                               ).OrderBy(x => x.Nome);

                return Json(estabelecimento);
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }


    }
}
