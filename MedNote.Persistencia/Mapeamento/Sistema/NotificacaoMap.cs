using MedNote.Infra.Dominio.Sistema;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Sistema
{
    public class NotificacaoMap : ClassMap<Notificacao>
    {
        public NotificacaoMap()
        {
            Table("SISTEMA.NOTIFICACAO");

            Id(u => u.Codigo, "CD_NOTIFICACAO");

            Map(u => u.Nome, "TX_NOME").Not.Nullable();

            Map(u => u.Descricao, "TX_DESCRICAO").Not.Nullable();

            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();

            Map(u => u.Pagina, "TX_PAGINA_REF").Not.Nullable();

            Map(u => u.Visualizado, "BL_VISUALIZADO").Not.Nullable();

            Map(u => u.DataDeVisualizacao, "DT_VISUALIZACAO").Nullable();

            References(u => u.Usuario, "CD_USUARIO").Not.Nullable();
        }
    }
}
