using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class Recurso
    {
        public virtual Int32 Codigo { get; set; }

        public virtual Acao AcaoPrincipal { get; set; }

        public virtual Acao AcaoSecundaria { get; set; }
    }
}
