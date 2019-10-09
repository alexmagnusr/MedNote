using System;
using MedNote.Dominio.MedNote;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class TipoSetorCliente
    {
        public virtual int Codigo { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual TipoSetor TipoSetor { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual DateTime DataDeCadastro { get; set; }
    }
}
