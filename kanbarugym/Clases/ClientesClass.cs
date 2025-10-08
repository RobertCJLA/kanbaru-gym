using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbarugym.Clases
{
    public class ClientesClass
    {
        public required string Id { get; set; }
        public required string Nombres { get; set; }
        public required string FechaNacimiento { get; set; }
        public required string CorreoElectronico { get; set; }
        public required string Telefono { get; set; }
        public required string Sexo { get; set; }
    }
}
