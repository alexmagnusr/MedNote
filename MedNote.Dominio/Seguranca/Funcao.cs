using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class Funcao
    {
        public virtual Int32 Codigo { get; set; }

        public virtual String Nome { get; set; }

        public virtual String Descricao { get; set; }

        public virtual DateTime? DataDesativacao { get; set; }

        public virtual DateTime DataDeCadastro { get; set; }

        public virtual String Ref { get; set; }

        public virtual TipoFuncao Tipo { get; set; }

        public virtual String Cor { get; set; }

        public virtual String Icone { get; set; }

        public virtual Canal Canal { get; set; }

        public virtual Funcao Pai { get; set; }
        public virtual IList<Acao> Acoes { get; set; }

        public virtual IList<Funcao> Filhas { get; set; }

        public Funcao()
        {
            Acoes = new List<Acao>();
        }

    }
}
