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
using MedNote.Dominio.DTOs;

namespace MedNote.API.Controllers.Seguranca
{
    [RoutePrefix("api/Cliente")]
    public class ClienteController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public ClienteController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            ClienteModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize(Roles = "Master,PostCliente")]
        [HttpPost]
        [Route("Cadastro")]
        public Object Post([FromBody]ClienteDTO Cliente)
        {
            return Json(formatarDados(ClienteModel.Instancia.Cadastrar(Cliente)));
        }

        //[Authorize(Roles = "Master,GetCliente")]
        [HttpGet]
        [Route("Consulta")]
        public Object Get()
        {
            var listaCliente = ClienteModel.Instancia.Consultar().ToList();
            var listaClienteFormatada = listaCliente.Select(x => formatarDados(x, false)).ToList();
            return Json(listaClienteFormatada);
        }

        [Authorize(Roles = "Master,Get/idCliente")]
        [HttpGet]
        [Route("Edita")]
        public Object Get(int id)
        {
            return formatarDados(ClienteModel.Instancia.Editar(id), true);
        }

        [Authorize(Roles = "Master,PutCliente")]
        [HttpPost]
        [Route("Atualiza")]
        public Object Put(int id, [FromBody]ClienteDTO Cliente)
        {
            return Json(formatarDados(ClienteModel.Instancia.Atualizar(Cliente, id)));
        }

        [Authorize(Roles = "Master,DeleteCliente")]
        [HttpPost]
        [Route("Delete")]
        public Object Delete(int id)
        {
            return Json(formatarDados(ClienteModel.Instancia.Deletar(id)));
        }


        private Object formatarDados(ClienteDTO data, bool editar = true)
        {
            try
            {
                var contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
                var retorno = new
                {
                    data.Codigo,
                    data.NomeGestorContrato,
                    data.ValorContrato,
                    TelefoneFormt = data.Telefone.ToPhoneNumberFormat(),
                    Telefone = data.Telefone,
                    data.Email,
                    DiaVencimento = Int32.Parse(data.DiaVencimento),
                    DiaVencimentoFormt = data.DiaVencimento,//data.DiaVencimento != string.Empty ? ("Todo Dia " + data.TryGetValue(v => v.DiaVencimento)) : string.Empty,
                    NumDocumento = data.TryGetValue(v => v.NumDocumento),
                    CNPJFormatado = data.TryGetValue(v => v.NumDocumento.ToCNPJFormat()),
                    Nome = data.TryGetValue(v => v.Nome),
                    DataInicioContratoFormt = data.DataInicioContrato != null ? ((DateTime)data.DataInicioContrato).ToString("dd/MM/yyyy") : null,//data.TryGetValue(v => v.DataInicioContrato) != null ? data.TryGetValue(v => v.DataInicioContrato).ToCultureString(DateTime, false, true) : string.Empty,
                    DataTerminoContratoFormt = data.DataTerminoContrato != null ?  ((DateTime)data.DataTerminoContrato).ToString("dd/MM/yyyy") : null,
                    DataInicioContrato = data.TryGetValue(v => v.DataInicioContrato),
                    DataTerminoContrato = data.TryGetValue(v => v.DataTerminoContrato),
                    DataCadastro = data.TryGetValue(v => v.DataDeCadastro).ToString("dd/MM/yyyy"),
                    Estabelecimento = contexto.ObterTodos<EstabelecimentoSaude>()
                                            .Where(p => p.Cliente.Codigo == data.Codigo && p.Desativado == false).ToList()
                                            .Select(m => new
                                            {
                                                m.Codigo,
                                                Editar = true,
                                                m.Nome,
                                                m.Email,
                                                m.Telefone
                                            })
                };
                return retorno;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
