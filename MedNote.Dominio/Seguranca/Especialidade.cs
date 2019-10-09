using System;

namespace MedNote.Infra.Dominio.Seguranca
{
    public class Especialidade
    {
        public virtual int Codigo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool Medica { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
      
    }
}
