using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FH_Project;

class InventoryFullException : Exception
{
    public InventoryFullException() { }

    public InventoryFullException(string message) : base(message) { }

    public InventoryFullException(string message, Exception innerException) : base(message, innerException) { }
}

