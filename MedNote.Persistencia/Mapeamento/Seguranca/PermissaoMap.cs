using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class PermissaoMap : ClassMap<PermissaoGrupoAcao>
    {
        public PermissaoMap()
        {
            Table("SEG_PERM_GRUPO_ACAO");

            Id(u => u.Codigo, "CD_PERM_GRUPO_ACAO");            
                  
            References(u => u.Acao, "CD_ACAO").Not.Nullable();
            References(u => u.Grupo, "CD_PERFIL").Not.Nullable();
            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
        

        }
    }
}
