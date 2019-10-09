using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class AdmissaoMap : ClassMap<Admissao>
    {
        public AdmissaoMap()
        {
            Table("SIS_ADMISSAO");

            Id(u => u.Codigo, "CD_ADMISSAO");

            //Map(u => u.CodigoSetor, "CD_SETOR");
            //Map(u => u.CodigoLeito, "CD_LEITO");
            //Map(u => u.CodigoInternacao, "CD_INTERNACAO");
            Map(u => u.DataAdmissao, "DT_ADMISSAO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVACAO").Not.Nullable();

            References(u => u.Internacao, "CD_INTERNACAO").Not.Nullable();
            References(u => u.Setor, "CD_SETOR").Not.Nullable();
            References(u => u.Leito, "CD_LEITO").Nullable();
        }
    }
}
