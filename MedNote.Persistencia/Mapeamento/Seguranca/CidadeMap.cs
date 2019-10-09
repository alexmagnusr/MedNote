using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace AtiveEngenharia.Persistencia.Mapeamento.Seguranca
{
    public class CidadeMap : ClassMap<Cidade>
    {
        public CidadeMap()
        {
            Table("SIS_CIDADE");

            Id(u => u.Codigo, "CD_CIDADE");
            Map(u => u.Nome, "TX_NOME").Not.Nullable();
            References(u => u.Estado, "CD_ESTADO").Not.Nullable();
        }
    }
}
