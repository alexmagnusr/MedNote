
namespace Globalsys.MEF.Contratos
{
    public interface IContratoBase
    {        
        string LinkRelativeUrl { get; }
        string IconRelativeUrl { get; }
        string Text { get; }
        string Description { get; }

        bool CanShow(string usuarioLogado, string url);
    }
}
