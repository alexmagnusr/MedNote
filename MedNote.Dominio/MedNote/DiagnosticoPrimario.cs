using System;

namespace MedNote.Dominio.MedNote
{
    public class DiagnosticoPrimario
    {
        public virtual int Codigo { get; set; }
        public virtual Diagnostico Diagnostico { get; set; }
        public virtual ParametrosBase OrigemPaciente { get; set; }
        public virtual Internacao Internacao { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
    }
}
