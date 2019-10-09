using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.MedNote
{
    public class DiagnosticoPrimarioMap : ClassMap<DiagnosticoPrimario>
    {
        public DiagnosticoPrimarioMap()
        {
            Table("SIS_DIAGNOSTICO_PRIMARIO");

            Id(u => u.Codigo, "[CD_DIAGNOSTICO_PRIMARIO");
            Map(u => u.Descricao, "TX_DESCRICAO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();

            References(u => u.Diagnostico, "CD_DIAGNOSTICO").Not.Nullable();
            References(u => u.Internacao, "CD_INTERNACAO").Not.Nullable();
            References(u => u.OrigemPaciente, "CD_ORIGEM").Not.Nullable();
        }
    }
}