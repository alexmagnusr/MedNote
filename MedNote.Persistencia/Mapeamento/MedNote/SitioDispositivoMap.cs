using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class SitioDispositivoMap : ClassMap<SitioDispositivo>
    {
        public SitioDispositivoMap()
        {
            Table("SIS_SITIO_DISPOSITIVO");

            Id(u => u.Codigo, "CD_SITIO_DISPOSITIVO");
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Descricao, "TX_DESCRICAO").Nullable();
            References(u => u.TipoDispositivo, "CD_TIPO_DISPOSITIVO").Not.Nullable();
        }
    }
}
