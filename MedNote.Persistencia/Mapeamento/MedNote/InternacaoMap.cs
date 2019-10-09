using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    class InternacaoMap : ClassMap<Internacao>
    {
        public InternacaoMap()
        {
            Table("SIS_INTERNACAO");

            Id(u => u.Codigo, "CD_INTERNACAO");

            Map(u => u.NumeroAtendimento, "NM_ATENDIMENTO").Not.Nullable();
            //Map(u => u.CodigoConvenio, "CD_PARAMETROS_BASE").Not.Nullable();
            //Map(u => u.CodigoPaciente, "CD_PACIENTE").Not.Nullable();
            Map(u => u.DataCadastro, "DT_INTERNACAO").Not.Nullable();
            Map(u => u.DataInternacao, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVACAO").Not.Nullable();

            References(u => u.Paciente, "CD_PACIENTE").Not.Nullable();

            References(u => u.Convenio, "CD_PARAMETROS_BASE").Not.Nullable();
        }

    }
}
