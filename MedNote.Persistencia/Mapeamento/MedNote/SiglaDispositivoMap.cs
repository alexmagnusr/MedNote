using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class SiglaDispositivoMap : ClassMap<SiglaDispositivo>
    {
        public SiglaDispositivoMap()
        {
            Table("SIS_SIGLA_DISPOSITIVO");

            Id(u => u.Codigo, "CD_SIGLA_DISPOSITIVO");

            Map(u => u.Lateralidade, "TX_LATERALIDADE").Not.Nullable();
            Map(u => u.Sigla, "TX_SIGLA").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();

            References(u => u.TipoDispositivo, "CD_TIPO_DISPOSITIVO").Not.Nullable();
            References(u => u.SitioDispositivo, "CD_SITIO_DISPOSITIVO").Not.Nullable();
            References(u => u.CategoriaDispositivo, "CD_CATEGORIA_DISPOSITIVO").Not.Nullable();
        }
    }
}
