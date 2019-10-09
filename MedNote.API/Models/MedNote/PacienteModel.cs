using Globalsys;
using Globalsys.Model;
using Globalsys.Validacao;
using LinqKit;
using MedNote.Dominio.MedNote;
using MedNote.Infra;
using System;
using System.Linq;

namespace MedNote.API.Models.MedNote
{
    public class PacienteModel : IModel
    {
        private static PacienteModel model { get; set; }

        public IUnidadeTrabalho UnidadeDeTrabalho { get; set; }

        public static PacienteModel Instancia
        {
            get
            {
                if (model == null)
                    model = new PacienteModel();

                return model;
            }
        }

        public Paciente Cadastrar(Paciente paciente)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            
            Paciente pacienteNew = new Paciente();
            try
            {
                return pacienteNew;
            }
                       
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public IQueryable<Paciente> Consultar(string nome = "", int codigoSetor = 0)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            
            int codigo = contexto.ObterPorId<Setor>(codigoSetor).Estabelecimento.Codigo;

            var predicate = PredicateBuilder.New<Paciente>();
            predicate = predicate.And(p => p.Desativado == false && p.EstabelecimentoSaude.Codigo == codigo);

            if (!string.IsNullOrWhiteSpace(nome))
            {
                nome = nome.Trim().ToLower();
                predicate = predicate.And(p => p.Nome.Trim().ToLower().Contains(nome));
            }

            IQueryable<Paciente> query = contexto.ObterTodos<Paciente>().Where(predicate);
            return query;
        }

        public Paciente Atualizar(Paciente paciente, int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                contexto.BeginTransaction();

                var pacienteOld = contexto.ObterPorId<Paciente>(codigo);
                pacienteOld.Nome = paciente.Nome;
                pacienteOld.Documento = paciente.Documento;
                pacienteOld.Genero = paciente.Genero;
                pacienteOld.DataNascimento = paciente.DataNascimento;
                pacienteOld.NumProntuario = paciente.NumProntuario;
                pacienteOld.TipoDocumento = paciente.TipoDocumento;

                contexto.Atualizar(pacienteOld);
                contexto.Commit();

                return pacienteOld;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public Paciente Editar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            return contexto.ObterPorId<Paciente>(codigo);
        }

        public Paciente Deletar(int codigo)
        {
            IUnidadeTrabalho contexto = Fabrica.Instancia.Obter<IUnidadeTrabalho>();
            try
            {
                Paciente paciente = contexto.ObterPorId<Paciente>(codigo);
                paciente.DataDesativacao = DateTime.Now;
                paciente.Desativado = true;
                contexto.BeginTransaction();
                contexto.Atualizar(paciente);
                contexto.Commit();

                return paciente;
            }
            catch (Exception ex)
            {
                contexto.Rollback();
                throw ex;
            }
        }

        public void ValidarCampos(object objeto, EstadoObjeto? estadoObjeto = null)
        {
            Paciente item = (Paciente)objeto;
            switch ((EstadoObjeto)estadoObjeto)
            {
                case EstadoObjeto.Novo:
                case EstadoObjeto.Alterado:
                    break;
            }
        }

        public void ValidarRegras(object objeto, EstadoObjeto? estadoObjeto = null)
        {

        }
    }
}