using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalsys.Dominio
{
    public class LogError
    {
        public virtual long Codigo { get; set; }

        public virtual string Message { get; set; }

        public virtual string StackTrace { get; set; }

        public virtual string Url { get; set; }

        public virtual string UserName { get; set; }

        public virtual DateTime DataRegistro { get; set; }
    }
}
