using System.ComponentModel;

namespace MedNote.Dominio.MedNote.Enums
{
    public enum EnumTipoDocumento
    {

        [Description("CPF")]
        CPF = 1,
        [Description("RG")]
        RG = 2,
        [Description("Outros")]
        Outros = 3,
    }
}
