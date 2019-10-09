using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class TipoSetorMap : ClassMap<TipoSetor>
    {
        public TipoSetorMap()
        {
            Table("SEG_TIPO_SETOR");

            Id(u => u.Codigo, "CD_TIPO_SETOR");
            Map(u => u.Descricao, "TX_NOME").Not.Nullable(); 
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVO").Not.Nullable();
            //Map(u => u.Estabelecimento, "CD_ESTABELECIMENTO").Nullable();

            References(u => u.Estabelecimento, "CD_ESTABELECIMENTO").Nullable();
        }
    }
}
