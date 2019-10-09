using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globalsys.Exceptions
{
    public class CoreException : Exception
    {
        public CoreException()
        {
        }

        public CoreException(string msg)
            : base (msg)
        {
        }

        public CoreException(string msg, Exception innerException)
            : base(msg, innerException)
        {
        }
    }
}
