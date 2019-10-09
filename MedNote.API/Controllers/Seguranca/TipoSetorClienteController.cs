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
using MedNote.API.Models.Seguranca;

namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/tipoSetorCliente")]
    public class TipoSetorClienteController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public TipoSetorClienteController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            TipoSetorClienteModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]TipoSetorCliente tipoSetorCliente)
        {
            var retorno = formatarDados(TipoSetorClienteModel.Instancia.Cadastrar(tipoSetorCliente));
            return retorno;
        }

        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formatarDados(TipoSetorClienteModel.Instancia.Deletar(id)));
        }

        private Object formatarDados(TipoSetorCliente data, bool editar = true)
        {
            var retorno = new
            {
                data.Codigo,
                Cliente = data.Cliente.Codigo,
                ClienteDesc = data.Cliente.Nome,
                TipoSetor = data.TipoSetor.Codigo,
                TipoSetorDesc = data.TipoSetor.Descricao
            };
            return retorno;

        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarTipoSetorPorCliente")]
        public Object ConsultarTipoSetorPorCliente(int codigo)
        {
            try
            {
                var retorno = TipoSetorClienteModel.Instancia.ObterTipoSetorPorCliente(codigo)
                                   .Select(
                                               p => new
                                               {
                                                   p.Codigo,
                                                   Cliente = p.Cliente.Codigo,
                                                   ClienteDesc = p.Cliente.Nome,
                                                   TipoSetor = p.TipoSetor.Codigo,
                                                   TipoSetorDesc = p.TipoSetor.Descricao
                                               }
                                               ).OrderBy(x => x.TipoSetorDesc);

                return Json(retorno);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
