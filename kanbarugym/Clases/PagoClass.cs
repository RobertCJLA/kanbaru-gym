using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbarugym.Clases
{
 
public class PagoClass : INotifyPropertyChanged
    {
        public required string Cliente { get; set; }

        public required string FechaInicio { get; set; }

        public required string Monto { get; set; }

        public required string Membresia { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
