using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class DispositivoInternacaoMap : ClassMap<DispositivoDaInternacao>
    {
        public DispositivoInternacaoMap()
        {
            Table("SIS_DISPOSITIVO_INTERNACAO");

            Id(u => u.Codigo, "CD_DISPOSITIVO_INTERNACAO");
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.DataImplante, "DT_IMPLANTE").Nullable();
            Map(u => u.DataRetirada, "DT_RETIRADA").Nullable();
            References(u => u.SiglaDispositivo, "CD_SIGLA_DISPOSITIVO").Not.Nullable();
            References(u => u.Internacao, "CD_INTERNACAO").Not.Nullable();
        }
    }
}
