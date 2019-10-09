using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra.Dominio.Sistema
{
    public class Notificacao
    {
        public virtual Int32 Codigo { get; set; }

        public virtual string Nome { get; set; }

        public virtual string Descricao { get; set; }

        public virtual DateTime DataDeCadastro { get; set; }

        public virtual string Pagina { get; set; }

        public virtual bool Visualizado { get; set; }

        public virtual DateTime? DataDeVisualizacao { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
