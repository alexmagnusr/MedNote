
using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.Seguranca;
using MedNote.Infra.Dominio.Seguranca;
using MedNote.Infra;
using MedNote.Repositorios.Seguranca;
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

    [RoutePrefix("api/Funcao")]
    public class FuncaoController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FuncaoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            FuncaoModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }
        private Object formatarDados(Funcao p)
        {
            return new
            {
                Codigo = p.Codigo,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Tipo = p.Tipo,
                DescTipo = p.Tipo.ObterDescricaoEnum(),
                Ref = p.Ref,
                Canal = p.Canal,
                DescCanal = p.Canal.ObterDescricaoEnum(),
                Cor = p.Cor,
                NomePai = p.TryGetValue(f => f.Pai.Nome),
                CodPai = p.TryGetValue(f => f.Pai.Codigo),
                DataDeCadastro = p.DataDeCadastro.ToString("dd/MM/yyyy"),
            };
        }

        [Authorize]
        // GET: api/Funcao
        public Object Get()
        {

            return Json(FuncaoModel.Instancia.Consultar().Select(x => formatarDados(x)));
            //return Json(FuncaoModel.Instancia.Consultar().ToList().Select(p => new
            //{
            //    Codigo = p.Codigo,
            //    Nome = p.Nome,
            //    Descricao = p.Descricao,
            //    Tipo = p.Tipo.ObterDescricaoEnum(),
            //    Ref = p.Ref,
            //    Canal = p.Canal.ObterDescricaoEnum(),
            //    Cor = p.Cor,
            //    NomePai = p.TryGetValue(f => f.Pai.Nome),
            //    DataDeCadastro = p.DataDeCadastro.ToString("dd/MM/yyyy"),
            //    DataDesativacao = p.DataDesativacao != null ? true : false
            //}).OrderBy(x => x.Nome));
        }

        // GET: api/Funcao/5
        [Authorize]
        public Object Get(int id)
        {
            return Json(formatarDados(FuncaoModel.Instancia.Editar(id)));
        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarPorTipo/{str}")]
        public Object ConsultarPorTipo(int str)
        {
            try
            {
                IRepositorioFuncao repFuncao = Fabrica.Instancia.ObterRepositorio<IRepositorioFuncao>(UnidadeTrabalho);
                var retorno = repFuncao.ObterPorTipo((TipoFuncao)str).Select(p => new
                {
                    p.Codigo,
                    Nome = p.TryGetValue(v => v.Nome),
                    DataDeCadastro = p.DataDeCadastro.ToString(),
                    DataDesativFuncao = p.DataDesativacao.ToString()

                }).OrderBy(x => x.Nome);
                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarPaginasPorModulo/{idModulo}")]
        public Object ConsultarPaginasPorModulo(int idModulo)
        {
            IRepositorioFuncao repFuncao = Fabrica.Instancia.ObterRepositorio<IRepositorioFuncao>(UnidadeTrabalho);

            return Json(repFuncao.ObterPaginasPorModulo(idModulo).Select(p => new
            {
                p.Codigo,
                Nome = p.TryGetValue(v => v.Nome),
                DataDeCadastro = p.DataDeCadastro.ToString(),
                DataDesativFuncao = p.DataDesativacao.ToString()

            }).OrderBy(x => x.Nome));
        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarTipo")]
        public IEnumerable<Object> ConsultarTipo()
        {
            Dictionary<int, TipoFuncao> lista = new Dictionary<int, TipoFuncao>();
            foreach (TipoFuncao item in Enum.GetValues(typeof(TipoFuncao)))
                lista.Add((int)item, item);

            return lista
                .Select(p => new { Id = p.Key, Descricao = p.Value.ObterDescricaoEnum() })
                    .OrderBy(p => p.Descricao)
                        .ToArray();
        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarCanal")]
        public IEnumerable<Object> ConsultarCanal()
        {
            Dictionary<int, Canal> lista = new Dictionary<int, Canal>();
            foreach (Canal item in Enum.GetValues(typeof(Canal)))
                lista.Add((int)item, item);

            return lista
                .Select(p => new { Id = p.Key, Descricao = p.Value.ObterDescricaoEnum() })
                    .OrderBy(p => p.Descricao)
                        .ToArray();
        }

        // POST: api/Funcao
        [Authorize]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]Funcao funcao)
        {
            return Json(formatarDados(FuncaoModel.Instancia.Cadastrar(funcao)));
        }

        // PUT: api/Funcao/5
        [Authorize]
        [HttpPost]
        [Route("Atualiza")]
        public Object Put(int id, [FromBody]Funcao funcao)
        {
            return formatarDados(FuncaoModel.Instancia.Atualizar(funcao, id));
        }

        // DELETE: api/Funcao/5
        [Authorize]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formatarDados(FuncaoModel.Instancia.Deletar(id)));
        }
    }
}
