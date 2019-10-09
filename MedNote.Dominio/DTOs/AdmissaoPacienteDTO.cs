using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;

namespace MedNote.Dominio.DTOs
{
    public class AdmissaoPacienteDTO
    {

        public virtual int Codigo { get; set; }
        public virtual int CodigoPaciente { get; set; }
        public virtual int CodigoInternacao { get; set; }
        public virtual int CodigoSetor { get; set; }
        public virtual int? CodigoLeito { get; set; }
        public virtual int CodigoEstabelecimeto { get; set; }
        public virtual int NumeroAtendimento { get; set; }
        public virtual string NomePaciente { get; set; }
        public virtual string NomeEstabelecimeto { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual string DataNascimentoFormat { get; set; }
        public virtual int Idade { get; set; }
        public virtual string Documento { get; set; }
        public virtual int TipoDocumento { get; set; }
        public virtual string Genero { get; set; }
        public virtual string NumProntuario { get; set; }
        public virtual int CodigoConvenio { get; set; }
        public virtual string NomeConvenio { get; set; }
        public virtual string NomeSetor { get; set; }
        public virtual string NomeLeito { get; set; }
        public virtual DateTime DataAdmissao { get; set; }
        public virtual DateTime DataCadastro { get; set; }
        public virtual DateTime DataInternacao { get; set; }
        public virtual DateTime DataCadastroInternacao { get; set; }
        public virtual DateTime? DataDesativacao { get; set; }
        public virtual string DataAdmissaoFormat { get; set; }
        public virtual string DataCadastroFormat { get; set; }
        public virtual string DataInternacaoFormat { get; set; }
        public virtual string DataCadastroInternacaoFormat { get; set; }
        public virtual string DataDesativacaoFormat { get; set; }
        public virtual bool Desativado { get; set; }
        public virtual bool Editar { get; set; }
        public AdmissaoPacienteDTO()
        {
            Editar = false;
        }
    }
}
