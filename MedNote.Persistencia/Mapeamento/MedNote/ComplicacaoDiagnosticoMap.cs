using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.MedNote
{
    public class ComplicacaoDiagnosticoMap : ClassMap<ComplicacaoDiagnostico>
    {
        public ComplicacaoDiagnosticoMap()
        {
            Table("SIS_COMPLICACAO_DIAGNOSTICO");

            Id(u => u.Codigo, "[CD_COMPLICACAO_DIAGNOSTICO");
            Map(u => u.DataDiagnostico, "DT_DIAGNOSTICO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();

            References(u => u.Diagnostico, "CD_DIAGNOSTICO").Not.Nullable();
            References(u => u.Internacao, "CD_INTERNACAO").Not.Nullable();
        }
    }
}