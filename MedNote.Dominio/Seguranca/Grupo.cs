using System;
using System.Collections.Generic;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class Grupo
    {
        public virtual Int32 Codigo { get; set; }
        public virtual String Descricao { get; set; }
        //public virtual IList<Usuario> Usuarios { get; set; }
        //public virtual Cliente Cliente { get; set; }
        public virtual IList<Acao> Acoes { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataDeCadastro { get; set; }
    }
}
