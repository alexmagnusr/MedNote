using System;

namespace MedNote.Dominio.MedNote
{
    public class Internacao
    {
        public virtual int Codigo { get; set; }
        public virtual int NumeroAtendimento { get; set; }
        //public virtual int CodigoConvenio { get; set; }
       // public virtual int CodigoPaciente { get; set; }
        public virtual ParametrosBase Convenio { get; set; }
        public virtual Paciente Paciente { get; set; }
        public virtual DateTime DataInternacao { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual bool Desativado { get; set; }

    }

}
