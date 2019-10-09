using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Sistema;
using MedNote.Infra;
using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MedNote.API.Controllers.Sistema
{
    [RoutePrefix("api/Util")]
    public class UtilController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public UtilController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            UtilModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarCep/{cep}")]
        public Object ConsultarCep(string cep)
        {
            return Json(UtilModel.Instancia.ConsultarCEP(cep));
        }
        
    }
}
