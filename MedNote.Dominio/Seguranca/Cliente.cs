using System;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class Cliente
    {
        public virtual int Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string NumDocumento { get; set; }
        public virtual string Email { get; set; }
        public virtual string NomeGestorContrato { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string DiaVencimento { get; set; }
        public virtual float? ValorContrato { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual DateTime? DataInicioContrato { get; set; }
        public virtual DateTime? DataTerminoContrato { get; set; }
        public virtual DateTime DataDeCadastro { get; set; }
        

    }
}
