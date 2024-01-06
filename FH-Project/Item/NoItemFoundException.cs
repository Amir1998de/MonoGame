using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;

class NoItemFoundException : Exception
{
    public NoItemFoundException() { }

    public NoItemFoundException(string message) : base(message) { }

    public NoItemFoundException(string message, Exception innerException) : base(message, innerException) { }
}
