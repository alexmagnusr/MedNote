using System;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class Usuario
    {
        public virtual int Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string NumDocumento { get; set; }
        public virtual string Email { get; set; }
        public virtual string Login { get; set; }
        public virtual string Senha { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual bool LoginAD { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual DateTime DataDeCadastro { get; set; }
        public virtual Especialidade Especialidade { get; set; }
        public virtual EstabelecimentoSaude EstabelecimentoSaude { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Grupo Grupo { get; set; }
        public virtual bool Admin { get; set; }
        public virtual bool AdminCliente { get; set; }
        //public virtual Setor Setor { get; set; }

    }
}
