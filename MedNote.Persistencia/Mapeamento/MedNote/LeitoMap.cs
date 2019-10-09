using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class LeitoMap : ClassMap<Leito>
    {
        public LeitoMap()
        {
            Table("SEG_LEITO");

            Id(u => u.Codigo, "CD_LEITO");
            Map(u => u.Bl_Liberado, "BL_LIBERADO").Not.Nullable(); 
            Map(u => u.Identificador, "TX_IDENTIFICACAO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVO").Not.Nullable();

            References(u => u.Setor, "CD_SETOR").Not.Nullable();
        }
    }
}
