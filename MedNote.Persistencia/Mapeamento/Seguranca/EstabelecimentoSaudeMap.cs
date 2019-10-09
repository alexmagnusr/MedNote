using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class EstabelecimentoSaudeMap : ClassMap<EstabelecimentoSaude>
    {
        public EstabelecimentoSaudeMap()
        {
            Table("SEG_ESTABELECIMENTO_SAUDE");

            Id(u => u.Codigo, "CD_ESTABELECIMENTO");
            Map(u => u.Nome, "TX_NOME").Not.Nullable(); 
            Map(u => u.Telefone, "TX_TELEFONE").Nullable(); 
            Map(u => u.Email, "TX_EMAIL").Nullable();
            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();
            References(u => u.Cliente, "CD_CLIENTE").Not.Nullable();

        }
    }
}
