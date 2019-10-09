using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.MedNote
{ 
    public class DiagnosticoMap : ClassMap<Diagnostico>
    {
        public DiagnosticoMap()
        {
            Table("SIS_DIAGNOSTICO");

            Id(u => u.Codigo, "CD_DIAGNOSTICO");
            Map(u => u.Descricao, "TX_DESCRICAO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();

            References(u => u.CategoriaDiagnostico, "CD_CATEGORIA_DIAGNOSTICO").Not.Nullable();
        }
    }
}
