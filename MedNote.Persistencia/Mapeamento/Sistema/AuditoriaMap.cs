using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Sistema
{
    //public class AuditoriaMap : ClassMap<MedNote.Infra.Dominio.Sistema.Auditoria>
    //{
    //    public AuditoriaMap()
    //    {
    //        Table("SISTEMA.AUDITORIA");

    //        Id(u => u.Codigo, "CD_AUDITORIA");
    //        References(u => u.Usuario, "CD_USUARIO").Not.Nullable();
    //        Map(u => u.Acao, "TX_ACAO").Not.Nullable();
    //        Map(u => u.CodigoRegistro, "TX_COD_REGISTRO").Not.Nullable();
    //        Map(u => u.Descricao, "TX_DESCRICAO").Not.Nullable();
    //        Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
    //        Map(u => u.IP, "TX_IP").Not.Nullable();
    //        Map(u => u.NomeCompletoEntidade, "TX_COMPLETO_ENTIDADE").Not.Nullable();
    //        Map(u => u.NomeEntidade, "TX_NOME_ENTIDADE").Not.Nullable();

    //    }
    //}
}
