using System;
using MedNote.Infra.Dominio.Seguranca;

namespace MedNote.Dominio.MedNote
{
    public class Paciente
    {
        public virtual int Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual string Documento { get; set; }
        public virtual int TipoDocumento { get; set; }
        public virtual string Genero { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual string NumProntuario { get; set; }
        public virtual EstabelecimentoSaude EstabelecimentoSaude { get; set; }
    }
}
