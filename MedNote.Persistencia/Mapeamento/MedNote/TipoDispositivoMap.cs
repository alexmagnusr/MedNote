using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class TipoDispositivoMap : ClassMap<TipoDispositivo>
    {
        public TipoDispositivoMap()
        {
            Table("SIS_TIPO_DISPOSITIVO");

            Id(u => u.Codigo, "CD_TIPO_DISPOSITIVO");

            Map(u => u.Descricao, "TX_DESCRICAO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();

            References(u => u.CategoriaDispositivo, "CD_CATEGORIA_DISPOSITIVO").Not.Nullable();
        }
    }
}
