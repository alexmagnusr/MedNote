using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class Acao
    {
        public virtual Int32 Codigo { get; set; }

        public virtual String Nome { get; set; }

        public virtual DateTime? DataDesativacao { get; set; }

        public virtual DateTime DataDeCadastro { get; set; }

        public virtual String Ref { get; set; }

        public virtual Funcao Funcao { get; set; }

        public virtual IList<Recurso> Recursos { get; set; }

        public Acao()
        {
            Recursos = new List<Recurso>();
        }

    }
}
