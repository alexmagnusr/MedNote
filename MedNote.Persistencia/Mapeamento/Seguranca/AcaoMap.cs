using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class AcaoMap : ClassMap<Acao>
    {
        public AcaoMap()
        {
            Table("SEG_ACAO");

            Id(f => f.Codigo, "CD_ACAO");            
                  
            Map(f => f.Nome, "TX_NOME").Not.Nullable();
            Map(f => f.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(f => f.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(f => f.Ref, "TX_REF").Nullable();

            References(f => f.Funcao, "CD_FUNCAO").Nullable();
        }
    }
}
