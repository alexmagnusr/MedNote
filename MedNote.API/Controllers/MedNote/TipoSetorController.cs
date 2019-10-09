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

namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/tipoSetor")]
    public class TipoSetorController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public TipoSetorController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            TipoSetorModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize(Roles = "Master,PostTipoSetor")]
        public Object Post([FromBody] TipoSetor tipoSetor)
        {
            var retorno = formatarDados(TipoSetorModel.Instancia.Cadastrar(tipoSetor));
            return retorno;
        }

        [Authorize(Roles = "Master,GetTipoSetor")]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(bool inativos)
        {
            var listaTipoSetor = TipoSetorModel.Instancia.Consultar(inativos).ToList();
            var listaTipoSetorFormatada = listaTipoSetor.Select(x => formatarDados(x, false)).ToList();
            return Json(listaTipoSetorFormatada);
        }

        [Authorize(Roles = "Master,Get/idTipoSetor")]
        public Object Get(int id)
        {
            return formatarDados(TipoSetorModel.Instancia.Editar(id), true);
        }

        [Authorize(Roles = "Master,PutTipoSetor")]
        public Object Put(int id, [FromBody]TipoSetor tipoSetor)
        {
            return Json(formatarDados(TipoSetorModel.Instancia.Atualizar(tipoSetor, id)));
        }

        [Authorize(Roles = "Master,DeleteTipoSetor")]
        public Object Delete(int id)
        {
            return Json(formatarDados(TipoSetorModel.Instancia.Deletar(id)));
        }
        
        private Object formatarDados(TipoSetor data, bool editar = true)
        {
            var retorno = new
            {
                data.Codigo,
                Estabelecimento = data.Estabelecimento != null ? data.Estabelecimento.Codigo : 0,
                nomeEstabelecimento = data.Estabelecimento != null ? data.Estabelecimento.Nome : "",
                codigoCliente = data.Estabelecimento != null ? data.Estabelecimento.Cliente.Codigo : 0,
                nomeCliente = data.Estabelecimento != null ? data.Estabelecimento.Cliente.Nome : "",
                data.Descricao,
                data.Desativado,
                DataCadastro = data.TryGetValue(v => v.DataCadastro).ToString("dd/MM/yyyy")
            };
            return retorno;

        }

    }
}
