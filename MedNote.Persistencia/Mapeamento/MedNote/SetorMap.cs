using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class SetorMap : ClassMap<Setor>
    {
        public SetorMap()
        {
            Table("SEG_SETOR");

            Id(u => u.Codigo, "CD_SETOR");
            Map(u => u.Nome, "TX_NOME").Not.Nullable(); 
            Map(u => u.QtdLeito, "IN_QTD_LEITO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVO").Not.Nullable();

            References(u => u.TipoSetor, "CD_TIPO_SETOR").Not.Nullable();
            References(u => u.Estabelecimento, "CD_ESTABELECIMENTO").Not.Nullable();
        }
    }
}
