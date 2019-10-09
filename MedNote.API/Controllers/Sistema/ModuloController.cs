using MedNote.API.Models.Seguranca;
using MedNote.API.Models.Sistema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Globalsys.Exceptions;
using Globalsys.Extensoes;
using MedNote.Infra.Dominio.Seguranca;

namespace MedNote.API.Controllers.Sistema
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/Modulo")]
    public class ModuloController : ApiController
    {
        // GET: api/Modulo
        [Authorize]
        public Object Get()
        {
            try
            {
                //return Json(ModuloModel.Instancia.Consultar());
                var usuario = UsuarioModel.Instancia.LoadMenuClientes().Where(u => !u.Cliente.IsNull());
                
                if(usuario != null)
                {
                    var retorno = Json(usuario.Select(x => new
                    {
                        x.Cliente.Codigo,
                        Nome = x.Cliente.Nome,
                        Descricao = x.TryGetValue(v => v.Cliente.Nome)
                    }));

                    return retorno;
                }

                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
