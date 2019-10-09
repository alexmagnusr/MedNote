
using MedNote.Infra;
using Globalsys;
using Globalsys.Extensoes;
using Globalsys.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VIX.API.Models.Seguranca;

namespace MedNote.API.Controllers.Seguranca
{
    /// <summary>
    /// Controller usuários
    /// </summary>
    /// 
    [ExtendController]
    [RoutePrefix("api/membro")]
    public class MembroController : ApiController, IControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MembroController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            MembroModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        public class MembrosRequest
        {
            public int grupo { get; set; }
            public int[] membros { get; set; }
        }

        // GET: api/Membro/5
        [Authorize]
        public Object Get(int idGrupo)
        {
            return Json(MembroModel.Instancia.Consultar(idGrupo)
                .ToList()
                .Select(p => new
                {
                    //CodigoMembro = p.Codigo,
                    Codigo = p.Usuario.Codigo,
                    Nome = p.TryGetValue(v => v.Usuario.Nome),
                    Login = p.TryGetValue(v => v.Usuario.Login),
                    Email = p.TryGetValue(v => v.Usuario.Email),
                    CpfFormatado = p.TryGetValue(v => v.Usuario.NumDocumento.ToCPFFormat()),
                    Cpf = p.TryGetValue(v => v.Usuario.NumDocumento.ToCPFFormat()),
                    DataDeCadastro = p.Usuario.DataDeCadastro.ToString(),
                    DataDesativacao = p.Usuario.DataDesativacao.ToString(),
                  //  p.Usuario.LoginAd
                }).OrderBy(x => x.Codigo));
        }

        [HttpPost]
        [Route("Cadastro")]
        public Object Post(MembrosRequest request)
        {
            return Json(MembroModel.Instancia.Cadastrar(request.membros, request.grupo));
        }

        // DELETE: api/Membro/5
        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(MembrosRequest request)
        {
            return Json(MembroModel.Instancia.Deletar(request.membros, request.grupo).ToList()
                .Select(p => new
                {
                    //CodigoMembro = p.Codigo,
                    Codigo = p.Usuario.Codigo,
                    Nome = p.TryGetValue(v => v.Usuario.Nome),
                    Login = p.TryGetValue(v => v.Usuario.Login),
                    Email = p.TryGetValue(v => v.Usuario.Email),
                    CpfFormatado = p.TryGetValue(v => v.Usuario.NumDocumento.ToCPFFormat()),
                    Cpf = p.TryGetValue(v => v.Usuario.NumDocumento.ToCPFFormat()),
                    DataDeCadastro = p.Usuario.DataDeCadastro.ToString(),
                    DataDesativacao = p.Usuario.DataDesativacao.ToString(),
                  //  p.Usuario.LoginAd
                }).OrderBy(x => x.Codigo));
        }
    }
}
