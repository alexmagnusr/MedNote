using System.ComponentModel;

namespace MedNote.Dominio.MedNote.Enums
{
    public enum EnumParametrosBase
    {
        [Description("Convênio")]
        Convenio = 1,
        [Description("Origem")]
        Origem = 2,
        [Description("Tipo de Internação")]
        TipoInternacao = 3,
        [Description("Tipo de Diagnóstico")]
        TipoDiagnostico = 4,
        [Description("Comorbidade")]
        Comorbidade = 5,
    }
}
