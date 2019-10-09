using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;


namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class GrupoMap : ClassMap<Grupo>
    {
        public GrupoMap()
        {
            Table("SEG_PERFIL");
            Id(u => u.Codigo, "CD_PERFIL");            
            Map(u => u.Descricao, "TX_DESCRICAO").Not.Nullable();
            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();
            //References(u => u.Cliente, "CD_CLIENTE").Not.Nullable();
        }
    }
}
