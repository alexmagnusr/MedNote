using MedNote.API.Models.Seguranca;
using MedNote.Infra;
using Globalsys;
using Globalsys.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace MedNote.API.Helpers.Globalsys
{
    public class BaseApiController : ApiController, IControllerBase
    {
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public BaseApiController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            UsuarioModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }
    }
}