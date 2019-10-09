using MedNote.Infra.Dominio.Seguranca;

namespace MedNote.Dominio.DTOs
{
    public class UsuarioEmpresa
    {
        public virtual int Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { get; set; }
        public virtual string NumDocumento { get; set; }
        public virtual string CodigoMegaERP { get; set; }
        public virtual bool LoginAd { get; set; }
        public virtual int Cliente { get; set; }
        public virtual int Especialidade { get; set; }
        public virtual int EstabelecimentoSaude { get; set; }
        public virtual int Grupo { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Celular { get; set; }
        public virtual bool Admin { get; set; }
        public virtual bool AdminCliente { get; set; }
    }
}
