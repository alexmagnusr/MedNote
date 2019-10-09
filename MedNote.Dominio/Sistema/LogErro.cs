using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra.Dominio.Sistema
{
    public class LogErro
    {
        public virtual Int32 Codigo { get; set; }

        public virtual String Acao { get; set; }

        public virtual DateTime DataErro { get; set; }

        public virtual String Mensagem { get; set; }

        public virtual String StackTrace { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
