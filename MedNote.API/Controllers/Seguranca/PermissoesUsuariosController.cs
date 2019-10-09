
using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Seguranca;
using MedNote.Infra;
using Globalsys;
using Globalsys.Extensoes;
using Globalsys.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace MedNote.API.Controllers.Seguranca
{
    /// <summary>
    /// Controller usuários
    /// </summary>
    /// 
    [ExtendController]
    [RoutePrefix("api/PermissoesUsuarios")]
    public class PermissoesUsuariosController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PermissoesUsuariosController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            PermissoesUsuariosModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        public class PermissoesRequest
        {
            public int usuario { get; set; }
            public int[] acoes { get; set; }
        }

        [Authorize(Roles = "Master,GetPermissoesUsuarios,Get/idPermissoesUsuarios")]
        public Object Get(int idUsuario)
        {
            return Json(PermissoesUsuariosModel.Instancia.Consultar(idUsuario)
                .ToList()
                .Select(p => new
                {
                    //CodigoMembro = p.Codigo,
                    Codigo = p.Acao.Codigo,
                    Nome = p.TryGetValue(v => v.Acao.Nome) + " - " + p.TryGetValue(v => v.Acao.Funcao.Nome),
                    DataDeCadastro = p.Acao.DataDeCadastro.ToString(),
                    DataDesativacao = p.Acao.DataDesativacao.ToString()
                }).OrderBy(x => x.Codigo));
        }

        [Authorize(Roles = "Master,PostPermissoesUsuarios")]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post(PermissoesRequest request)
        {
            return Json(PermissoesUsuariosModel.Instancia.Cadastrar(request.acoes, request.usuario));
        }

        [Authorize(Roles = "Master,DeletePermissoesUsuarios")]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(PermissoesRequest request)
        {
            return Json(PermissoesUsuariosModel.Instancia.Deletar(request.acoes, request.usuario).ToList()
                .Select(p => new
                {
                    Codigo = p.Acao.Codigo,
                    Nome = p.TryGetValue(v => v.Acao.Nome),
                    DataDeCadastro = p.Acao.DataDeCadastro.ToString(),
                    DataDesativacao = p.Acao.DataDesativacao.ToString()
                }).OrderBy(x => x.Codigo));
        }

    }
}
