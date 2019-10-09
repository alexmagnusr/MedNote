using Globalsys;
using Globalsys.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Globalsys.Extensoes;
using MedNote.Infra;
using MedNote.API.Models.Sistema;

namespace MedNote.API.Controllers.Sistema
{
    /// <summary>
    /// Controller de aditoria 
    /// </summary>
    public class AuditoriaController : ApiController, IControllerBase
    {
        /// <summary>
        /// Unidade de trabalho
        /// </summary>
        public IUnidadeTrabalho UnidadeTrabalho { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AuditoriaController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            AuditoriaModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        // GET: api/Auditoria
        [Authorize]
        // GET: api/Grupo
        public Object Get(string tipo)
        {
            return Json(AuditoriaModel.Instancia.Consultar(tipo).ToList().Select(p => new
            {
                p.Codigo,
                p.Descricao,
                Data = p.DataDeCadastro.ToString(),
                Nome = p.TryGetValue(v => v.Usuario.Nome),
                p.NomeEntidade,
                p.NomeCompletoEntidade,
                p.IP,
                p.CodigoRegistro,
                p.DataDeCadastro,
                p.Acao,

            }).OrderByDescending(x => x.DataDeCadastro));
        }

    }
}
