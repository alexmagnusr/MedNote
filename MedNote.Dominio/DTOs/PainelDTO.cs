using System;
using System.Collections.Generic;
using MedNote.Dominio.MedNote;

namespace MedNote.Dominio.DTOs
{
    public class PainelDTO
    {
        public virtual int CodigoLeito { get; set; }
        public virtual int CodigoSetor { get; set; }
        public virtual int? CodigoAdmissao { get; set; }
        public virtual string Identificador { get; set; }
        public virtual string NomeEstabelecimento { get; set; }
        public virtual string NomeSetor { get; set; }
        public virtual string NomePaciente { get; set; }
        public virtual string InfoSetor { get; set; }
        public virtual List<string> Dispositivos { get; set; }
        public virtual bool Liberado { get; set; }
        public virtual Admissao Admissao { get; set; }
        public virtual DateTime? DataAdmissao { get; set; }
        public virtual string DataAdmissaoFormat { get; set; }
        public virtual decimal MediaPermanencia { get; set; }
        public virtual decimal TaxaOcupacao { get; set; }
    }
}
