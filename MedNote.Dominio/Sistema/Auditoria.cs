using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra.Dominio.Sistema
{
    public class Auditoria
    {
        public virtual Int32 Codigo { get; set; }

        public virtual Seguranca.Usuario Usuario { get; set; }

        public virtual String Acao { get; set; }

        public virtual String CodigoRegistro { get; set; }

        public virtual DateTime DataDeCadastro { get; set; }

        public virtual String IP { get; set; }

        public virtual String Descricao { get; set; }

        public virtual String NomeCompletoEntidade { get; set; }

        public virtual String NomeEntidade { get; set; }
    }
}
