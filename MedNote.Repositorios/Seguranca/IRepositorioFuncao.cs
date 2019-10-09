using MedNote.Infra.Dominio.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedNote.Repositorios.Seguranca
{
    public interface IRepositorioFuncao
    {
        IList<Funcao> ObterPorTipo(TipoFuncao tipo);

        IList<Funcao> ObterPaginasPorModulo(int idModulo);


    }
}
