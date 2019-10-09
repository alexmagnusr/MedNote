using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class RecursoMap : ClassMap<Recurso>
    {
        public RecursoMap()
        {
            Table("SISTEMA.RECURSOS");

            Id(f => f.Codigo, "CD_RECURSO");

            References(f => f.AcaoPrincipal, "CD_ACAO_PRINCIPAL").Not.Nullable();

            References(f => f.AcaoSecundaria, "CD_ACAO_SECUNDARIA").Not.Nullable();
        }
    }
}
