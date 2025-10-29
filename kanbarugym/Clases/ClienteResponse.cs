using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbarugym.Clases
{
    public class ClienteResponse
    {
        public required string message { get; set; }
        public required string? id { get; set; }
    }
}
