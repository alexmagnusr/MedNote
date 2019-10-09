using System;

namespace MedNote.Dominio.MedNote
{
    public class Leito
    {
        public virtual int Codigo { get; set; }
        public virtual string Identificador { get; set; }
        public virtual bool Bl_Liberado { get; set; }
        public virtual Setor Setor { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual bool Editar { get; set; }
    }
}
