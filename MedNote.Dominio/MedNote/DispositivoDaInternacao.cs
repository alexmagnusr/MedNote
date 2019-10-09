using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedNote.Dominio.MedNote.Enums;

namespace MedNote.Dominio.MedNote
{
    public class DispositivoDaInternacao
    {
        public virtual int Codigo { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual DateTime DataImplante { get; set; }
        public virtual DateTime? DataRetirada { get; set; }
        public virtual SiglaDispositivo SiglaDispositivo { get; set; }
        public virtual Internacao Internacao { get; set; }
    }

    #region DTO

    public class DispositivoInternacaoFormatado
    {
        public virtual int Codigo { get; set; }
        public virtual DateTime DataCadastroSigla { get; set; }
        public virtual string Tempo { get; set; }
        public virtual string DataCadastroSiglaFormatada { get; set; }
        public virtual int TipoDispositivo { get; set; }
        public virtual string TipoDispositivoDescricao { get; set; }
        public virtual int SitioDispositivo { get; set; }
        public virtual string SitioDispositivoDescricao { get; set; }
        public virtual int CategoriaDispositivo { get; set; }
        public virtual string CategoriaDispositivoDescricao { get; set; }
        public virtual string Lateralidade { get; set; }
        public virtual string LateralidadeDescricao { get; set; }
        public virtual List<SiglaDispositivo> SiglaDispositivos { get; set; }
        public virtual string SiglaDispositivo { get; set; }
        public virtual DateTime DataCadastroDispositivo { get; set; }
        public virtual string DataCadastroDispositivoFormatada { get; set; }
        public virtual DateTime DataImplante { get; set; }
        public virtual string DataImplanteFormatada { get; set; }
        public virtual DateTime? DataRetirada { get; set; }
        public virtual string DataRetiradaFormatada { get; set; }
        public virtual Internacao Internacao { get; set; }
        public virtual int CodigoInternacao { get; set; }
        public virtual Boolean IsOpen { get; set; }
        public virtual string NomePaciente { get; set; }
        

        public DispositivoInternacaoFormatado()
        {
            SiglaDispositivos = new List<SiglaDispositivo>();
        }
    }
    #endregion
}