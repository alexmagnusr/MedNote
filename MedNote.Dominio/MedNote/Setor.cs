using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;

namespace MedNote.Dominio.MedNote
{
    public class Setor
    {
        public virtual int Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual int QtdLeito { get; set; }
        public virtual TipoSetor TipoSetor { get; set; }
        public virtual EstabelecimentoSaude Estabelecimento { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }

        public virtual List<Leito> Leitos { get; set; }

        public Setor()
        {
            Leitos = new List<Leito>();
        }

    }
}
