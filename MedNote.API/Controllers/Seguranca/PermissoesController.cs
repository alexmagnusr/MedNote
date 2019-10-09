using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Seguranca;
using MedNote.Infra;
using Globalsys;
using Globalsys.Extensoes;
using Globalsys.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MedNote.API.Controllers.Seguranca
{
    [ExtendController]
    [RoutePrefix("api/permissoes")]
    public class PermissoesController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }
        
        public PermissoesController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            PermissoesModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        public class PermissoesRequest
        {
            public int grupo { get; set; }
            public int[] acoes { get; set; }
        }

        [Authorize(Roles = "Master,GetPermissoes")]
        public Object Get(int idGrupo)
        {
            return Json(PermissoesModel.Instancia.Consultar(idGrupo)
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
        
        [Authorize(Roles = "Master,PostPermissoes")]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post(PermissoesRequest request)
        {
            return Json(PermissoesModel.Instancia.Cadastrar(request.acoes, request.grupo));
        }

        [Authorize(Roles = "Master,PostPermissoes")]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(PermissoesRequest request)
        {
            return Json(PermissoesModel.Instancia.Deletar(request.acoes, request.grupo).ToList()
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
