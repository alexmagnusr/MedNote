
using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Seguranca;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra;
using MedNote.Repositorios.Seguranca;
using Globalsys;
using Globalsys.Extensoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace MedNote.API.Controllers.Seguranca
{
    /// <summary>
    /// Controller usuários
    /// </summary>
    /// 

    [RoutePrefix("api/Acao")]
    public class AcaoController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AcaoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            AcaoModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize]
        // GET: api/Acao
        public Object Get()
        {
            return Json(AcaoModel.Instancia.Consultar().ToList().Select(p => new
            {
                Codigo = p.Codigo,
                Nome = p.Nome,
                DataDeCadastro = p.DataDeCadastro.ToString("dd/MM/yyyy"),
                DataDesativacao = p.DataDesativacao != null ? true : false,
                Funcao = p.Funcao.Nome
            }).OrderBy(x => x.Funcao));
        }

        // GET: api/Acao/5
        [Authorize]
        public Object Get(int id)
        {
            return Json(formataDados(AcaoModel.Instancia.Editar(id)));
        }       

        [Authorize]
        [HttpGet]
        [Route("ConsultarAcoesPorPagina/{str}")]
        public Object ConsultarAcoesPorPagina(int str)
        {
            IRepositorioAcao repAcao = Fabrica.Instancia.ObterRepositorio<IRepositorioAcao>(UnidadeTrabalho);

            return Json(repAcao.ObterAcoesPorPagina(str).Select(p => new
            {
                p.Codigo,
                Nome = p.TryGetValue(v => v.Nome) + " - " + p.TryGetValue(v => v.Funcao.Nome),               
                DataDeCadastro = p.DataDeCadastro.ToString(),
                DataDesativacao = p.DataDesativacao.ToString()
            }).OrderBy(x => x.Nome));
        }
        // POST: api/Acao
        [Authorize]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]Acao value)
        {
            return Json(AcaoModel.Instancia.Cadastrar(value));
        }

        // PUT: api/Acao/5
        [Authorize]
        [HttpPost]
        [Route("Atualiza")]
        public Object Put(int id, [FromBody]Acao Acao)
        {
            return Json(formataDados(AcaoModel.Instancia.Atualizar(Acao, id)));
        }

        // DELETE: api/Acao/5
        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formataDados(AcaoModel.Instancia.Deletar(id)));
        }

        private Object formataDados(Acao data) {
            return new
            {
                Codigo = data.Codigo,
                Nome = data.Nome,
                Ref = data.Ref,
                CodFuncao = data.Funcao.Codigo
            };
        }
    }
}
