using System;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class EstabelecimentoSaude
    {
        public virtual int Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Email { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual DateTime DataDeCadastro { get; set; }
        public virtual bool Editar { get; set; }
      
        public EstabelecimentoSaude()
        {
            Editar = false;
        }
    }
}
