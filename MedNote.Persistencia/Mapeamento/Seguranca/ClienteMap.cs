using MedNote.Infra.Dominio.Seguranca;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class ClienteMap : ClassMap<Cliente>
    {
        public ClienteMap()
        {
            Table("SEG_CLIENTE");

            Id(u => u.Codigo, "CD_CLIENTE");
            Map(u => u.Nome, "TX_NOME").Not.Nullable();
            Map(u => u.Email, "TX_EMAIL").Nullable();
            Map(u => u.NumDocumento, "TX_DOCUMENTO").Nullable();
            Map(u => u.NomeGestorContrato, "TX_GESTOR_CONTRATO").Nullable();
            Map(u => u.ValorContrato, "NM_VALOR_CONTRATO").Nullable();
            Map(u => u.Telefone, "TX_TELEFONE").Nullable();
            Map(u => u.DiaVencimento, "TX_DIA_VENCIMENTO").Nullable();
            Map(u => u.DataDeCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVACAO").Nullable();
            Map(u => u.DataInicioContrato, "DT_INICIO_CONTRATO").Nullable();
            Map(u => u.DataTerminoContrato, "DT_TERMINO_CONTRATO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVO").Not.Nullable();
        }
    }
}
