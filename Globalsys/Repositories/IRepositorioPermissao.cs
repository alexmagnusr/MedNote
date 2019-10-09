using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Globalsys;

namespace Globalsys.Repositories
{
    public interface IRepositorioPermissao
    {
        bool PossuiPermissao(string controller, string action);
    }
}
