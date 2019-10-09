using FluentNHibernate.Mapping;
using MedNote.Dominio.MedNote;

namespace MedNote.Persistencia.Mapeamento.Seguranca
{
    public class PacienteMap : ClassMap<Paciente>
    {
        public PacienteMap()
        {
            Table("SIS_PACIENTE");

            Id(u => u.Codigo, "CD_PACIENTE");
            Map(u => u.Nome, "TX_NOME").Not.Nullable();
            Map(u => u.DataNascimento, "DT_NASCIMENTO").Not.Nullable();
            Map(u => u.Documento, "TX_DOCUMENTO").Not.Nullable();
            Map(u => u.TipoDocumento, "NM_TIPO_DOCUMENTO").Not.Nullable(); 
            Map(u => u.Genero, "TX_GENERO").Not.Nullable();
            Map(u => u.DataCadastro, "DT_CADASTRO").Not.Nullable();
            Map(u => u.DataDesativacao, "DT_DESATIVADO").Nullable();
            Map(u => u.Desativado, "BL_DESATIVADO").Not.Nullable();
            Map(u => u.NumProntuario, "TX_NUM_PRONTUARIO").Nullable();

            References(u => u.EstabelecimentoSaude, "CD_ESTABELECIMENTO").Not.Nullable();
        }
    }
}
