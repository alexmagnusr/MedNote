
using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Seguranca;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra;
using Globalsys;
using System;
using System.Linq;
using System.Web.Http;

namespace MedNote.API.Controllers.Seguranca
{
    /// <summary>
    /// Controller usuários
    /// </summary>
    /// 
    [RoutePrefix("api/Grupo")]
    public class GrupoController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GrupoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            GrupoModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        //[Authorize(Roles = "Master,GetPerfil")]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(int? codigo)
        {
            return Json(GrupoModel.Instancia.Consultar(codigo).ToList().Select(p => new
            {
                Codigo = p.Codigo,
                Descricao = p.Descricao,
                //ClienteDesc = p.Cliente.Nome,
                //Cliente = p.Cliente.Codigo,
                DataDeCadastro = p.DataDeCadastro.ToString("dd/MM/yyyy"),
                DataDesativacao = p.DataDesativacao != null ? true : false
            }).OrderBy(x => x.Descricao));
        }

        // GET: api/Grupo/5
        [Authorize(Roles = "Master,Get/idPerfil")]
        public Object Get(int id)
        {
            var grupo = GrupoModel.Instancia.Editar(id);
            var retorno = new
            {
                Codigo = grupo.Codigo,
                Descricao = grupo.Descricao,
                //ClienteDesc = grupo.Cliente.Nome,
                //Cliente = grupo.Cliente.Codigo,
                DataDeCadastro = grupo.DataDeCadastro.ToString("dd/MM/yyyy"),
                DataDesativacao = grupo.DataDesativacao != null ? true : false
            };
            return retorno;
        }

        private Object formatarDados(Grupo p)
        {
            try
            {
                var retorno = new
                {
                    Codigo = p.Codigo,
                    Descricao = p.Descricao,
                    //ClienteDesc = p.Cliente.Nome,
                    //Cliente = p.Cliente.Codigo,
                    DataDeCadastro = p.DataDeCadastro.ToString("dd/MM/yyyy"),
                    DataDesativacao = p.DataDesativacao != null ? true : false
                };
                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // POST: api/Grupo
        [Authorize(Roles = "Master,PostPerfil")]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]Grupo grupo)
        {
            var retorno = formatarDados(GrupoModel.Instancia.Cadastrar(grupo));
            return retorno;
        }

        // PUT: api/Grupo/5
        [Authorize(Roles = "Master,PutPerfil")]
        [HttpPost]
        [Route("Atualiza")]
        public Object Put(int id, [FromBody]Grupo Grupo)
        {
            var retorno = formatarDados(GrupoModel.Instancia.Atualizar(Grupo, id));
            return retorno;
        }

        // DELETE: api/Grupo/5
        [Authorize(Roles = "Master,DeletePerfil")]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formatarDados(GrupoModel.Instancia.Deletar(id)));
        }
    }
}
