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


namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/leito")]
    public class LeitoController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public LeitoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            LeitoModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize]
        public Object Post([FromBody] Leito leito)
        {
            var retorno = formatarDados(LeitoModel.Instancia.Cadastrar(leito));
            return retorno;
        }

        [Authorize]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(int codigo)
        {
            var leitos = LeitoModel.Instancia.Consultar(codigo).ToList();
            var leitosFormatada = leitos.Select(x => formatarDados(x, false)).ToList();
            return Json(leitosFormatada);
        }

        [Authorize]
        public Object Get(int id)
        {
            return formatarDados(LeitoModel.Instancia.Editar(id), true);
        }

        [Authorize]
        public Object Put(int id, [FromBody]Leito leito)
        {
            return Json(formatarDados(LeitoModel.Instancia.Atualizar(leito, id)));
        }

        [Authorize]
        public Object Delete(int id)
        {
            return Json(formatarDados(LeitoModel.Instancia.Deletar(id)));
        }
        
        private Object formatarDados(Leito data, bool editar = true)
        {
            var retorno = new
            {
                data.Codigo,
                Setor = data.Setor.Codigo,
                nomeSetor = data.Setor.Nome,
                Bl_Liberado = data.Bl_Liberado ? "Sim" : "Não",
                data.Identificador,
                data.Desativado,
                DataCadastro = data.TryGetValue(v => v.DataCadastro).ToString("dd/MM/yyyy")
            };
            return retorno;

        }

    }
}
