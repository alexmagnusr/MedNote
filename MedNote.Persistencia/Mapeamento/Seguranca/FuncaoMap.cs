using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class FuncaoMap : ClassMap<Funcao>
    {
        public FuncaoMap()
        {
            Table("SEG_FUNCOES");

            Id(f => f.Codigo, "CD_FUNCAO");            
                  
            Map(f => f.Nome, "TX_NOME").Not.Nullable();
            Map(f => f.Descricao, "TX_DESCRICAO").Not.Nullable();
            Map(f => f.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(f => f.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(f => f.Ref, "TX_REF").Nullable();
            Map(f => f.Tipo, "TX_TIPO").Not.Nullable();
            Map(f => f.Cor, "TX_COR").Nullable();
            Map(f => f.Icone, "TX_ICON").Nullable();
            Map(f => f.Canal, "TX_CANAL").Not.Nullable();

            References(f => f.Pai, "CD_PAI").Nullable(); 
        }
    }
}
