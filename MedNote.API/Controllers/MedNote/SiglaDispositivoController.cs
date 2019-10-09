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
using MedNote.Dominio.MedNote.Enums;
using MedNote.API.Models.MedNote;
using MedNote.Dominio.DTOs;


namespace MedNote.API.Controllers.MedNote
{
    [RoutePrefix("api/siglaDispositivo")]
    public class SiglaDispositivoController : BaseApiController
    {

        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public SiglaDispositivoController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            SiglaDispositivoModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        private static Object GetItem(DispositivoDaInternacao dispositivo)
        {
            try
            {
                var contexto = SiglaDispositivoModel.Instancia.UnidadeDeTrabalho;
                return new
                {
                    dispositivo.Codigo,
                    SiglaNome = dispositivo.TryGetValue(v => v.SiglaDispositivo.Sigla),
                    SiglaCodigo = new { Codigo = dispositivo.TryGetValue(v => v.SiglaDispositivo.Codigo) },
                    DataCadastroFormatado = dispositivo.DataCadastro.ToCultureString(),
                    dispositivo.DataCadastro,
                    DataImplanteFormatado = dispositivo.DataImplante.ToCultureString(),
                    dispositivo.DataImplante,
                    DataRetiradaFormatado = dispositivo.DataRetirada.ToCultureString(),
                    dispositivo.DataRetirada,
                    Editar = true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Object formatarDados(DispositivoInternacaoFormatado data)
        {
            try
            {
                //cria um objeto anonimo com os dados formatado
                var retorno = new
                {
                    data.Codigo,
                    data.DataCadastroDispositivo,
                    DataCadastroDispositivoFormatada = data.Codigo != 0 ? data.DataCadastroDispositivo.ToString("dd/MM/yyyy") : null,
                    data.DataImplante,
                    DataImplanteFormatada = data.Codigo != 0 ? data.DataImplante.ToString("dd/MM/yyyy HH:mm") : null,
                    data.DataRetirada,
                    DataRetiradaFormatada = data.DataRetirada != null ? Convert.ToDateTime(data.DataRetirada).ToString("dd/MM/yyyy HH:mm") : null,
                    data.Tempo,
                    DataCadastroSigla = data.DataCadastroDispositivo,
                    DataCadastroSiglaFormatada = data.SiglaDispositivo != "" ? data.DataCadastroSigla.ToString("dd/MM/yyyy") : null,
                    data.TipoDispositivo,
                    data.TipoDispositivoDescricao,
                    data.CategoriaDispositivo,
                    data.CategoriaDispositivoDescricao,
                    data.SitioDispositivo,
                    data.SitioDispositivoDescricao,
                    data.SiglaDispositivo,
                    data.Lateralidade,
                    data.CodigoInternacao
            };

                return retorno;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public class DispositivoRequest
        {
            public int codigo { get; set; }
            public DispositivoDaInternacao dispositivo { get; set; }
        }


        [HttpPost]
        [Route("Implantar")]
        public Object Post([FromBody] SiglaDispositivoFormatado siglaDispositivo)
        {
            try
            {
                return Json(formatarDados(SiglaDispositivoModel.Instancia.Implantar(siglaDispositivo)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(int codigo)
        {
            try
            {
                var resultados = SiglaDispositivoModel.Instancia.ConsultarFormatada(codigo);

                return resultados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("Editar")]
        public Object Get(int id)
        {
            try
            {
                var data = SiglaDispositivoModel.Instancia.Editar(id);

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("ConsultarDispositivoInternacao")]
        public Object ConsultarDispositivoInternacao(int id)
        {
            try
            {
                var resultados = SiglaDispositivoModel.Instancia.ConsultarDispositivoInternacao(id, 0);
                return resultados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("AtualizarDispositivoInternacao")]
        public Object AtualizarDispositivoInternacao(DispositivoRequest request)
        {

            return Json(formatarDados(SiglaDispositivoModel.Instancia.AtualizarDispositivoInternacao(request.codigo, request.dispositivo)));
        }

        //[HttpPost]
        //[Route("IncluirDispositivoInternacao")]
        //public Object IncluirDispositivoInternacao([FromBody]DispositivoDaInternacao data)
        //{
        //    try
        //    {
        //        var dispositivo = SiglaDispositivoModel.Instancia.IncluirDispositivoInternacao(data);
        //        return GetItem(dispositivo);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpPost]
        [Route("DeletarDispositivoInternacao")]
        public Object DeletarDispositivoInternacao(int id)
        {
            var data = SiglaDispositivoModel.Instancia.DeletarDispositivoInternacao(id);
            return Json(new { data.Codigo });
        }

        [HttpGet]
        [Route("ConsultarLateralidade")]
        public IEnumerable<Object> ConsultarSituacaoTitulo()
        {
            Dictionary<int, EnumLateralidade> lista = new Dictionary<int, EnumLateralidade>();
            foreach (EnumLateralidade item in Enum.GetValues(typeof(EnumLateralidade)))
                lista.Add((int)item, item);

            return lista
                .Select(p => new { Id = p.Key, Nome = p.Value.ObterDescricaoEnum() })
                    .OrderBy(p => p.Nome)
                        .ToArray();
        }

        [Authorize]
        [Route("ConsultarCategoria")]
        [HttpGet]
        public Object ConsultarCategoria()
        {
            try
            {
                var resultados = SiglaDispositivoModel.Instancia.ConsultarCategoria();

                return resultados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [Route("ConsultarTipo")]
        [HttpGet]
        public Object ConsultarTipo(int categoria)
        {
            try
            {
                var resultados = SiglaDispositivoModel.Instancia.ConsultarTipo(categoria);

                return resultados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Authorize]
        [Route("ConsultarSitio")]
        [HttpGet]
        public Object ConsultarSitio(int tipo)
        {
            try
            {
                var resultados = SiglaDispositivoModel.Instancia.ConsultarSitio(tipo);

                return resultados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}