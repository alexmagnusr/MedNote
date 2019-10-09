using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class CategoriaDispositivoMap : ClassMap<CategoriaDispositivo>
    {
        public CategoriaDispositivoMap()
        {
            Table("SIS_CATEGORIA_DISPOSITIVO");

            Id(u => u.Codigo, "CD_CATEGORIA_DISPOSITIVO");

            Map(u => u.Descricao, "TX_DESCRICAO").Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
        }
    }
}
