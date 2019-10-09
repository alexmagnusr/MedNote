using System;

namespace MedNote.Dominio.MedNote
{
    public class CategoriaDiagnostico
    {
        public virtual int Codigo { get; set; }
        public virtual ParametrosBase TipoInternacao { get; set; }
        public virtual ParametrosBase TipoDiagnostico { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
    }
}
