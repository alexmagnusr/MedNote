using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class EspecialidadeMap : ClassMap<Especialidade>
    {
        public EspecialidadeMap()
        {
            Table("SEG_ESPECIALIDADE");

            Id(u => u.Codigo, "CD_ESPECIALIDADE");
            Map(u => u.Descricao, "TX_DESCRICAO").Not.Nullable(); 
            Map(u => u.Medica, "BL_MEDICA").Not.Nullable(); 
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();

            References(u => u.Cliente, "CD_CLIENTE").Nullable();
        }
    }
}
