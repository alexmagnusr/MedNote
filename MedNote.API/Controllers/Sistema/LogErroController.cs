using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Globalsys.Extensoes;
using MedNote.API.Models.Sistema;

namespace MedNote.API.Controllers.Sistema
{
    public class LogErroController : ApiController
    {
        // GET: api/LogErro
        [Authorize]
        public Object Get()
        {

            return Json(LogErroModel.Instancia.Consultar().ToList().Select(p => new
            {
                p.Codigo,
                p.Acao,
                p.Mensagem,
                DataErro = p.DataErro.ToString(),
                HoraErro = p.DataErro.ToLongDateString(),
                Data = p.DataErro,
                p.StackTrace,
                Usuario = p.TryGetValue(v => v.Usuario.Nome),
            }).OrderByDescending(x => x.Data));
        }

    }
}
