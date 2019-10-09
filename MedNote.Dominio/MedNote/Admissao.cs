using System;

namespace MedNote.Dominio.MedNote
{
    public class Admissao
    {
        public virtual int Codigo { get; set; }
        public virtual Internacao Internacao { get; set; }
        public virtual Setor Setor { get; set; }
        public virtual Leito Leito { get; set; }
        public virtual DateTime DataAdmissao { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual bool Editar { get; set; }

    }
}
