using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Dominio.MedNote
{
    public class ComorbidadeInternacao
    {
        public virtual int Codigo { get; set; }
        public virtual Internacao Internacao { get; set; }
        public virtual ParametrosBase Comorbidade { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
    }
}
