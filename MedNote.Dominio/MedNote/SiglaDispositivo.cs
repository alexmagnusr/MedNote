using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedNote.Dominio.MedNote.Enums;

namespace MedNote.Dominio.MedNote
{
    public class SiglaDispositivo
    {
        public virtual int Codigo { get; set; }
        public virtual string Sigla { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual TipoDispositivo TipoDispositivo { get; set; }
        public virtual SitioDispositivo SitioDispositivo { get; set; }
        public virtual CategoriaDispositivo CategoriaDispositivo { get; set; }
        public virtual string Lateralidade{ get; set; }
        public virtual List<DispositivoDaInternacao> DispositivosInstalados { get; set; }

        public SiglaDispositivo()
        {
            DispositivosInstalados = new List<DispositivoDaInternacao>();
        }
    }

    #region DTO

    public class SiglaDispositivoFormatado
    {
        public virtual int Codigo { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual string DataCadastroFormatada { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual int TipoDispositivo { get; set; }
        public virtual string TipoDispositivoDescricao { get; set; }
        public virtual int SitioDispositivo { get; set; }
        public virtual string SitioDispositivoDescricao { get; set; }
        public virtual int CategoriaDispositivo { get; set; }
        public virtual string CategoriaDispositivoDescricao { get; set; }
        public virtual string Lateralidade { get; set; }
        public virtual List<DispositivoDaInternacao> DispositivosInstalados { get; set; }
        public virtual string SiglaDispositivo { get; set; }
        public virtual DateTime DataCadastroDipositivo { get; set; }
        public virtual string DataCadastroDipositivoFormatada { get; set; }
        public virtual DateTime DataImplante { get; set; }
        public virtual string DataImplanteFormatada { get; set; }
        public virtual DateTime? DataRetirada { get; set; }
        public virtual string DataRetiradaFormatada { get; set; }
        public virtual Internacao Internacao { get; set; }
        public virtual int CodigoInternacao { get; set; }

        public SiglaDispositivoFormatado()
        {
            DispositivosInstalados = new List<DispositivoDaInternacao>();
        }
    }
    #endregion
}
