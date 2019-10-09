using MedNote.Infra.Dominio.Seguranca;
using System;

namespace MedNote.Dominio.MedNote
{
    public class ParametrosBase
    {
        public virtual int Codigo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual int Tipo { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}
