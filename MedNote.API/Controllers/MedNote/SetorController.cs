using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Ative;
using MedNote.Infra;
using Globalsys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Globalsys.Extensoes;
using MedNote.Dominio.MedNote;
using MedNote.Dominio.DTOs;

namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/setor")]
    public class SetorController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public SetorController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            SetorModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize(Roles = "Master,PostSetor")]
        public Object Post([FromBody] SetorDTO setor)
        {
            var retorno = formatarDados(SetorModel.Instancia.Cadastrar(setor));
            return retorno;
        }
        
        [Route("ConsultarPorEstabelecimento")]
        [HttpGet]
        public Object ConsultarPorEstabelecimento(int codigo)
        {
            var listaSetor = SetorModel.Instancia.ConsultarPorEstabelecimento(codigo).ToList();
            var listaSetorFormatada = listaSetor.Select(x => formatarDados(x, false)).ToList();
            return Json(listaSetorFormatada);
        }

        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(int codigo)
        {
            var listaSetor = SetorModel.Instancia.Consultar(codigo).ToList();
            var listaSetorFormatada = listaSetor.Select(x => formatarDados(x, false)).ToList();
            return Json(listaSetorFormatada);
        }

        [Authorize(Roles = "Master,Get/idSetor")]
        public Object Get(int id)
        {
            return formatarDados(SetorModel.Instancia.Editar(id), true);
        }

        [Authorize(Roles = "Master,PutSetor")]
        public Object Put(int id, [FromBody]SetorDTO setor)
        {
            return Json(formatarDados(SetorModel.Instancia.Atualizar(setor, id)));
        }

        [Authorize(Roles = "Master,DeleteSetor")]
        public Object Delete(int id)
        {
            return Json(formatarDados(SetorModel.Instancia.Deletar(id)));
        }

        private Object formatarDados(SetorDTO data, bool editar = true)
        {
            var retorno = new
            {
                data.Codigo,
                TipoSetor = data.TipoSetor.Codigo,
                nomeTipoSetor = data.TipoSetor.Descricao,
                Estabelecimento = data.Estabelecimento.Codigo,
                EstabelecimentosNome = data.Estabelecimento.Nome,
                Cliente = data.Estabelecimento.Cliente.Codigo,
                ClienteNome = data.Estabelecimento.Cliente.Nome,
                data.Nome,
                QtdLeitos = data.Leitos.Count(),
                data.Desativado,
                DataCadastro = data.TryGetValue(v => v.DataCadastro).ToString("dd/MM/yyyy"),
                Leitos = data.Leitos.Select(m => new
                {
                    m.Codigo,
                    Editar = true,
                    Bl_Liberado = m.Bl_Liberado ? "Sim" : "Não",
                    m.Identificador
                })
            };
            return retorno;

        }

    }
}
