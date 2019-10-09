using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class Membro
    {
        public virtual Int32 Codigo { get; set; }

        public virtual Grupo Grupo { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual DateTime DataDeCadastro { get; set; }

        public virtual DateTime? DataDesativacao { get; set; }
    }
}
