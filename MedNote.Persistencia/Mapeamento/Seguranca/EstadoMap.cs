using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace AtiveEngenharia.Persistencia.Mapeamento.Seguranca
{
    public class EstadoMap : ClassMap<Estado>
    {
        public EstadoMap()
        {
            Table("SIS_ESTADO");

            Id(u => u.Codigo, "CD_ESTADO");

            Map(u => u.Nome, "TX_NOME").Not.Nullable();
            Map(u => u.UF, "TX_UF").Not.Nullable();

        }
    }
}
