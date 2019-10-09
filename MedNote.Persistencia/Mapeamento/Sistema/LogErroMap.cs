using MedNote.Infra.Dominio.Sistema;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Sistema
{
    public class LogErroMap : ClassMap<LogErro>
    {
        public LogErroMap()
        {
            Table("SISTEMA.LOG_ERRO");

            Id(u => u.Codigo, "CD_LOG_ERRO");



            Map(u => u.Acao, "TX_ACAO").Not.Nullable().CustomType("StringClob").CustomSqlType("nvarchar(max)");
            Map(u => u.DataErro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.Mensagem, "TX_MENSAGEM").Not.Nullable();
            Map(u => u.StackTrace, "TX_STACKTRACE").Not.Nullable().CustomType("StringClob").CustomSqlType("nvarchar(max)");
            References(u => u.Usuario, "CD_USUARIO").Not.Nullable();


            //Map(u => u.Ativo, "BL_ATIVO").Nullable();
            //Map(u => u.VIP, "BL_VIP").Nullable();

            //References(u => u.CentroCusto, "CD_CENTRO_CUSTO").Nullable();

            //HasMany(u => u.Chamados).Table("HD_CHAMADO").KeyColumn("CD_USUARIO_RESPONSAVEL");
            //HasMany(u => u.MeusChamados).Table("HD_CHAMADO").KeyColumn("CD_USUARIO_SOLICITANTE");
            //HasMany(u => u.MeusFeedbacks).Table("HD_FEEDBACK_CHAMADO").KeyColumn("CD_USUARIO");
            //HasMany(u => u.AreasResponsaveis).Table("SEG_AREA").KeyColumn("CD_USUARIO_RESPONSAVEL");

            //HasManyToMany(a => a.Areas).Not.LazyLoad().Table("SEG_AREA_USUARIO").ParentKeyColumn("CD_USUARIO").ChildKeyColumn("CD_AREA").Where("DT_DESATIVACAO IS NULL");
        }
    }
}
