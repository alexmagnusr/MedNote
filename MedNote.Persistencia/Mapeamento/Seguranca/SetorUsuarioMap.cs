using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class SetorUsuarioMap : ClassMap<SetorUsuario>
    {
        public SetorUsuarioMap()
        {
            Table("SEG_SETOR_USUARIO");

            Id(u => u.Codigo, "CD_SETOR_USUARIO");
            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVACAO").Not.Nullable();
            References(u => u.Usuario, "CD_USUARIO").Not.Nullable();
            References(u => u.Setor, "CD_SETOR").Not.Nullable();

        }
    }
}
