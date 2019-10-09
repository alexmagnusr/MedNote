using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class PermisaoUsuarioAcao
    {
        public virtual Int32 Codigo { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual Acao Acao { get; set; }

        public virtual DateTime DataDeCadastro { get; set; }

        public virtual DateTime? DataDesativacao { get; set; }
        public virtual bool Desativado { get; set; }
    }
}
