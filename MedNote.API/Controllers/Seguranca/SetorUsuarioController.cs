using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Ative;
using MedNote.Infra;
using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Globalsys.Extensoes;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.API.Models.Seguranca;

namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/setorUsuario")]
    public class SetorUsuarioController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public SetorUsuarioController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            SetorUsuarioModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }
        [Authorize]
        [HttpGet]
        [Route("Consultar")]
        public Object Consultar(int codigo)
        {
            try
            {
                var retorno = SetorUsuarioModel.Instancia.Consultar(codigo)
                                   .Select(
                                               p => new
                                               {
                                                   p.Codigo,
                                                   Setor = p.Setor.Codigo,
                                                   SetorDesc = p.Setor.Nome,
                                                   Usuario = p.Usuario.Codigo,
                                                   UsuarioDesc = p.Usuario.Nome
                                               }
                                               ).OrderBy(x => x.SetorDesc);

                return Json(retorno);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [Authorize]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]SetorUsuario setorUsuario)
        {
            var retorno = formatarDados(SetorUsuarioModel.Instancia.Cadastrar(setorUsuario));
            return retorno;
        }

        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formatarDados(SetorUsuarioModel.Instancia.Deletar(id)));
        }

        private Object formatarDados(SetorUsuario data, bool editar = true)
        {
            var retorno = new
            {
                data.Codigo,
                Setor = data.Setor.Codigo,
                SetorDesc = data.Setor.Nome,
                Usuario = data.Usuario.Codigo,
                UsuarioDesc = data.Usuario.Nome
            };
            return retorno;

        }

    }
}
