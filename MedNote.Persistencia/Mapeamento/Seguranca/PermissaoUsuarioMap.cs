using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class PermissaoUsuarioMap : ClassMap<PermisaoUsuarioAcao>
    {
        public PermissaoUsuarioMap()
        {
            Table("SEG_PERM_USUARIO_ACAO");

            Id(u => u.Codigo, "CD_PERM_USUARIO_ACAO");            
                  
            References(u => u.Acao, "CD_ACAO").Not.Nullable();
            References(u => u.Usuario, "CD_USUARIO").Not.Nullable();
            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            
        }
    }
}
