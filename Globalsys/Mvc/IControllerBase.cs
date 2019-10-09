using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalsys.Mvc
{
    public interface IControllerBase
    {
        IUnidadeTrabalho UnidadeTrabalho { get; set; }
    }
}
