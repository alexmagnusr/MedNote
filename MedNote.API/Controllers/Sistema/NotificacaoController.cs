using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VIX.API.Helpers.Globalsys;
using Globalsys.Extensoes;
using MedNote.Infra;
using MedNote.API.Models.Sistema;
using MedNote.API.Helpers.Globalsys;

namespace MedNote.API.Controllers.Sistema
{
    [RoutePrefix("api/Notificacao")]
    public class NotificacaoController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public NotificacaoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            NotificacaoModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize]
        [HttpGet]
        public Object Get(bool? visualizado = null)
        {
            return Json(NotificacaoModel.Instancia.Consultar(visualizado).ToList().Select(p => new
            {
                p.Codigo,
                p.Descricao,
                Data = p.DataDeCadastro.ToCultureString(),
                Nome = p.Nome,
                p.DataDeCadastro,
                p.Pagina,
                NomeUsuario = p.Usuario.TryGetValue(v => v.Nome),
                p.Visualizado,
                DataDeVisualizacao = p.TryGetValue(v => v.DataDeVisualizacao.ToCultureString())

            }).OrderByDescending(x => x.DataDeCadastro));
        }

        [Authorize]
        [HttpGet]
        [Route("QntNotificacoes")]
        public Object QntNotificacoes(bool visualizado)
        {
            return Json(NotificacaoModel.Instancia.QntNotificacoes(visualizado));
        }

        [Authorize]
        [HttpPost]
        [Route("MarcarComoVisualizado")]
        public Object MarcarComoVisualizado(int codigo)
        {
            var data = NotificacaoModel.Instancia.MarcarComoVisualizado(codigo);
            return Json(new
            {
                Codigo = data.Codigo,
                data.Descricao,
                Data = data.DataDeCadastro.ToCultureString(),
                Nome = data.Nome,
                data.DataDeCadastro,
                data.Pagina,
                NomeUsuario = data.Usuario.TryGetValue(v => v.Nome),
                data.Visualizado,
                DataDeVisualizacao = data.TryGetValue(v => v.DataDeVisualizacao.ToCultureString())
            });
        }

        [Authorize]
        [HttpGet]
        [Route("SetPlayerId")]
        public Object SetPlayerId(string playerid)
        {
            return Json(NotificacaoModel.Instancia.SetPlayerId(playerid));
        }
    }
}
