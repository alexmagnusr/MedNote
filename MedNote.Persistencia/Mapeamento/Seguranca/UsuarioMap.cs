using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("SEG_USUARIO");

            Id(u => u.Codigo, "CD_USUARIO");
            Map(u => u.Nome, "TX_NOME").Not.Nullable();
            Map(u => u.NumDocumento, "TX_DOCUMENTO").Not.Nullable();
            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.Senha, "TX_SENHA").Not.Nullable();
            Map(u => u.Login, "TX_LOGIN").Not.Nullable();
            Map(u => u.Email, "TX_EMAIL").Nullable(); 
            Map(u => u.LoginAD, "BL_LOGIN_AD").Not.Nullable(); 
            Map(u => u.Desativado, "BL_DESATIVACAO").Not.Nullable();
            Map(u => u.Admin, "BL_ADMIN").Not.Nullable();
            Map(u => u.AdminCliente, "BL_ADMIN_CLIENTE").Not.Nullable();

            References(u => u.Especialidade, "CD_ESPECIALIDADE").Nullable();
            References(u => u.Grupo, "CD_PERFIL").Not.Nullable();
            References(u => u.Cliente, "CD_CLIENTE").Nullable();
            References(u => u.EstabelecimentoSaude, "CD_ESTABELECIMENTO").Nullable();

        }
    }
}
