using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.MedNote
{
    class ComorbidadeInternacaoMap : ClassMap<ComorbidadeInternacao>
    {
        public ComorbidadeInternacaoMap()
        {
            Table("SIS_COMORBIDADE_INTERNACAO");

            Id(u => u.Codigo, "[CD_COMORBIDADE_INTERNACAO");
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();

            References(u => u.Comorbidade, "CD_COMORBIDADE").Not.Nullable();
            References(u => u.Internacao, "CD_INTERNACAO").Not.Nullable();
        }
    }
}
