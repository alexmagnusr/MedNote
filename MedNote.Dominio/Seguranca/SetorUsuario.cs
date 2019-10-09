using System;
using MedNote.Dominio.MedNote;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class SetorUsuario
    {
        public virtual int Codigo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Setor Setor { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual DateTime DataDeCadastro { get; set; }
    }
}