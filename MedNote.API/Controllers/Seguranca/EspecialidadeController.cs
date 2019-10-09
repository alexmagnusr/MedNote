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

namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/especialidade")]
    public class EspecialidadeController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public EspecialidadeController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            EspecialidadeModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize(Roles = "Master,PostEspecialidade")]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]Especialidade especialidade)
        {
            var retorno = formatarDados(EspecialidadeModel.Instancia.Cadastrar(especialidade));
            return retorno;
        }

        //[Authorize(Roles = "Master,GetEspecialidade")]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(int codigo)
        {
            var listaEspecialidade = EspecialidadeModel.Instancia.Consultar(codigo).ToList();
            var listaEspecialidadeFormatada = listaEspecialidade.Select(x => formatarDados(x, false)).ToList();
            return Json(listaEspecialidadeFormatada);
        }

        [Authorize(Roles = "Master,Get/idEspecialidade")]
        public Object Get(int id)
        {
            return formatarDados(EspecialidadeModel.Instancia.Editar(id), true);
        }

        [Authorize(Roles = "Master,PutEspecialidade")]
        [HttpPost]
        [Route("Atualiza")]
        public Object Put(int id, [FromBody]Especialidade especialidade)
        {
            return Json(formatarDados(EspecialidadeModel.Instancia.Atualizar(especialidade, id)));
        }

        [Authorize(Roles = "Master,DeleteEspecialidade")]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formatarDados(EspecialidadeModel.Instancia.Deletar(id)));
        }
        
        private Object formatarDados(Especialidade data, bool editar = true)
        {
            var retorno = new
            {
                data.Codigo,
                codigoCliente = data.Cliente.Codigo,
                nomeCliente = data.Cliente.Nome,
                data.Medica,
                data.Descricao,
                DataCadastro = data.TryGetValue(v => v.DataCadastro).ToString("dd/MM/yyyy")
            };
            return retorno;

        }

    }
}
