using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.MedNote
{
    public class CategoriaDiagnosticoMap : ClassMap<CategoriaDiagnostico>
    {
        public CategoriaDiagnosticoMap()
        {
            Table("SIS_CATEGORIA_DIAGNOSTICO");

            Id(u => u.Codigo, "CD_CATEGORIA_DIAGNOSTICO");
            Map(u => u.Descricao, "TX_DESCRICAO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();

            References(u => u.TipoInternacao, "CD_TIPO_INTERNACAO").Not.Nullable();
            References(u => u.TipoDiagnostico, "CD_TIPO_DIAGNOSTICO").Not.Nullable();
        }
    }
}
