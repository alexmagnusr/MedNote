using Globalsys;
using Globalsys.Extensoes;
using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.MedNote;
using MedNote.Dominio.MedNote.Enums;
using MedNote.Dominio.MedNote;
using MedNote.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MedNote.API.Controllers.MedNote
{
    [RoutePrefix("api/parametrosBase")]
    public class ParametrosBaseController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public ParametrosBaseController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            ParametrosBaseModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize(Roles = "Master,PostParametrosBase")]
        public Object Post([FromBody]ParametrosBase parametrosBase)
            => formatarDados(ParametrosBaseModel.Instancia.Cadastrar(parametrosBase));

        [Authorize(Roles = "Master,GetParametrosBase")]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(int codigo, int tipo, string descricao)
        {
            var listaParametrosBase = ParametrosBaseModel.Instancia.Consultar(codigo, tipo, descricao).ToList();
            var listaParametrosBaseFormatada = listaParametrosBase.Select(x => formatarDados(x, false)).ToList();
            return Json(listaParametrosBaseFormatada);
        }

        [Authorize(Roles = "Master,Get/idParametrosBase")]
        public Object Get(int id)
            => formatarDados(ParametrosBaseModel.Instancia.Editar(id), true);

        [Authorize(Roles = "Master,PutParametrosBase")]
        public Object Put(int id, [FromBody]ParametrosBase parametrosBase)
            => Json(formatarDados(ParametrosBaseModel.Instancia.Atualizar(parametrosBase, id)));

        [Authorize(Roles = "Master,DeleteParametrosBase")]
        public Object Delete(int id)
            => Json(formatarDados(ParametrosBaseModel.Instancia.Deletar(id)));

        [Authorize]
        [HttpGet]
        [Route("tipos")]
        public IEnumerable<Object> ObterTipos()
        {
            Dictionary<int, EnumParametrosBase> lista = new Dictionary<int, EnumParametrosBase>();
            foreach (EnumParametrosBase item in Enum.GetValues(typeof(EnumParametrosBase)))
                lista.Add((int)item, item);

            return lista
                .Select(p => new { Id = p.Key, Descricao = p.Value.ObterDescricaoEnum() })
                .OrderBy(p => p.Descricao)
                .ToArray();
        }

        private Object formatarDados(ParametrosBase parametrosBase, bool editar = true)
        {
            var retorno = new
            {
                parametrosBase.Codigo,
                Cliente = parametrosBase.Cliente.IsNull() ? 0 : parametrosBase.Cliente.Codigo,
                nomeCliente = parametrosBase.Cliente.IsNull() ? "" : parametrosBase.Cliente.Nome,
                parametrosBase.Descricao,
                parametrosBase.Desativado,
                DataCadastro = parametrosBase.TryGetValue(v => v.DataCadastro).ToString("dd/MM/yyyy"),
                parametrosBase.Tipo,
                tipoNome = EnumExtensoes.ObterDescricaoEnum((EnumParametrosBase)parametrosBase.Tipo)
            };
            return retorno;

        }
    }
}
