using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class ParametrosBaseMap : ClassMap<ParametrosBase>
    {
        public ParametrosBaseMap()
        {
            Table("SIS_PARAMETROS_BASE");

            Id(u => u.Codigo, "CD_PARAMETROS_BASE");
            Map(u => u.Descricao, "TX_NOME").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();
            Map(u => u.Tipo, "IN_TIPO_PARAMETRO").Not.Nullable();

            References(u => u.Cliente, "CD_CLIENTE").Nullable();
        }
    }
}
