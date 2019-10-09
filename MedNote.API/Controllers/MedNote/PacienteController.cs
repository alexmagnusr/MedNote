using Globalsys;
using Globalsys.Extensoes;
using MedNote.API.Helpers.Globalsys;
using MedNote.API.Models.MedNote;
using MedNote.Dominio.MedNote;
using MedNote.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MedNote.API.Controllers.MedNote
{
    [RoutePrefix("api/paciente")]
    public class PacienteController : BaseApiController
    {
        public new IUnidadeTrabalho UnidadeTrabalho { get; set; }

        public PacienteController()
        {
            UnidadeTrabalho = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            PacienteModel.Instancia.UnidadeDeTrabalho = UnidadeTrabalho;
        }

        [Authorize]
        public Object Post([FromBody]Paciente paciente)
            => formatarDados(PacienteModel.Instancia.Cadastrar(paciente));

        [Authorize]
        [Route("Consultar")]
        [HttpGet]
        public Object Consultar(string nome)
        {
            var listaPaciente = PacienteModel.Instancia.Consultar(nome).ToList();
            var listaPacienteFormatada = listaPaciente.Select(x => formatarDados(x, false)).ToList();
            return Json(listaPacienteFormatada);
        }

        [Authorize]
        public Object Get(int id)
            => formatarDados(PacienteModel.Instancia.Editar(id), true);

        [Authorize]
        public Object Put(int id, [FromBody]Paciente paciente)
            => Json(formatarDados(PacienteModel.Instancia.Atualizar(paciente, id)));

        [Authorize]
        public Object Delete(int id)
            => Json(formatarDados(PacienteModel.Instancia.Deletar(id)));

        private Object formatarDados(Paciente paciente, bool editar = true)
        {
            var retorno = new
            {
                paciente.Codigo,
                DataCadastro = paciente.TryGetValue(v => v.DataCadastro).ToString("dd/MM/yyyy"),
                paciente.Genero,
                Nascimento = paciente.TryGetValue(v => v.DataNascimento).ToString("dd/MM/yyyy"),
                paciente.NumProntuario,
                paciente.Documento,
                paciente.Nome,
                paciente.Desativado
            };
            return retorno;

        }
    }
}
