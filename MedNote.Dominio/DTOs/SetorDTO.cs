using MedNote.Dominio.MedNote;
using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;

namespace MedNote.Dominio.DTOs
{
    public class SetorDTO
    {
        public virtual int Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual int QtdLeitos { get; set; }
        public virtual int QtdLeitoInicio { get; set; }
        public virtual int QtdLeitoFim { get; set; }
        public virtual TipoSetor TipoSetor { get; set; }
        public virtual EstabelecimentoSaude Estabelecimento { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual IList<Leito> Leitos { get; set; }
        public virtual bool Editar { get; set; }
        public SetorDTO()
        {
            Leitos = new List<Leito>();
            Editar = false;
        }
    }
}
