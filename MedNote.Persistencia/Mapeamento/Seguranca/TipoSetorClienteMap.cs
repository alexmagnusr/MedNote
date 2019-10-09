using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class TipoSetorClienteMap : ClassMap<TipoSetorCliente>
    {
        public TipoSetorClienteMap()
        {
            Table("SEG_TIPO_SETOR_CLIENTE");

            Id(u => u.Codigo, "CD_TIPO_SETOR_CLIENTE"); 
            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVACAO").Not.Nullable();
            References(u => u.Cliente, "CD_CLIENTE").Not.Nullable();
            References(u => u.TipoSetor, "CD_TIPO_SETOR").Not.Nullable();

        }
    }
}
