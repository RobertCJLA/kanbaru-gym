using kanbarugym.Clases;
using kanbarugym.Lib;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace kanbarugym.ViewModels;

public class AdminCard : INotifyPropertyChanged
{
 string _title = string.Empty;
 public string Title { get => _title; set { if (_title == value) return; _title = value; OnPropertyChanged(); } }
 string _tag = string.Empty;
 public string Tag { get => _tag; set { if (_tag == value) return; _tag = value; OnPropertyChanged(); } }
 string _dateText = string.Empty;
 public string DateText { get => _dateText; set { if (_dateText == value) return; _dateText = value; OnPropertyChanged(); } }
 string _name = string.Empty;
 public string Name { get => _name; set { if (_name == value) return; _name = value; OnPropertyChanged(); } }

 public event PropertyChangedEventHandler? PropertyChanged;
 protected void OnPropertyChanged([CallerMemberName] string? prop = null)
 => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}

public class AdminPanelViewModel : INotifyPropertyChanged
{
 public AdminCard NuevoCliente { get; } = new();
 public AdminCard ClienteFiel { get; } = new();
 public AdminCard TerminaPronto { get; } = new();
 public AdminCard Terminada { get; } = new();

 bool _isBusy;
 public bool IsBusy { get => _isBusy; private set { if (_isBusy == value) return; _isBusy = value; OnPropertyChanged(); } }

    public async Task LoadAsync()
    {
        try
        {
            IsBusy = true;
            var clientes = await ClientesLib.ObtenerClientes() ?? new List<ClientesClass>();
            SetCardDefaults();
            if (clientes.Count == 0) return;

            var pagosPorCliente = new Dictionary<string, List<PagoClass>>();
            foreach (var c in clientes)
            {
                try { pagosPorCliente[c.Id] = await PagoLib.ObtenerPagosClientes(c.Id) ?? new List<PagoClass>(); }
                catch { pagosPorCliente[c.Id] = new List<PagoClass>(); }
            }

            DateTime? Parse(string? s)
            {
                if (string.IsNullOrWhiteSpace(s)) return null;
                var formats = new[] { "dd/MM/yy", "dd/MM/yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };
                foreach (var f in formats)
                    if (DateTime.TryParseExact(s, f, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                        return d;
                if (DateTime.TryParse(s, out var gen)) return gen;
                return null;
            }

            var rand = new Random();

            // Nuevo Cliente random
            var ultimosClientes = clientes.OrderByDescending(c => c.Id).Take(5).ToList();
            var randomNuevo = ultimosClientes[rand.Next(ultimosClientes.Count)];
            NuevoCliente.Title = "Nuevo cliente";
            NuevoCliente.Tag = "Ingreso";
            NuevoCliente.DateText = DateTime.Now.ToString("dd/MM/yy");
            NuevoCliente.Name = randomNuevo.Nombres;

            // Cliente fiel random
            var clientesConPagos = pagosPorCliente.Where(kv => kv.Value.Any()).ToList();
            if (clientesConPagos.Any())
            {
                var randomFiel = clientesConPagos[rand.Next(clientesConPagos.Count)];
                var primerPago = randomFiel.Value.OrderBy(p => Parse(p.FechaInicio)).FirstOrDefault();
                if (primerPago != null)
                {
                    ClienteFiel.Title = "Cliente fiel";
                    ClienteFiel.Tag = primerPago.Membresia ?? "";
                    ClienteFiel.DateText = Parse(primerPago.FechaInicio)?.ToString("dd/MM/yy") ?? "-";
                    ClienteFiel.Name = clientes.First(c => c.Id == randomFiel.Key).Nombres;
                }
            }

            // Termina pronto random
            var activos = pagosPorCliente
                .SelectMany(kv => kv.Value.Where(p => p.Activo && Parse(p.FechaFin) >= DateTime.Today)
                .Select(p => new { ClienteId = kv.Key, Pago = p, Fin = Parse(p.FechaFin) }))
                .ToList();
            if (activos.Any())
            {
                var randomPronto = activos[rand.Next(activos.Count)];
                TerminaPronto.Title = "Termina pronto";
                TerminaPronto.Tag = randomPronto.Pago.Membresia ?? "";
                TerminaPronto.DateText = randomPronto.Fin?.ToString("dd/MM/yy") ?? "-";
                TerminaPronto.Name = clientes.First(c => c.Id == randomPronto.ClienteId).Nombres;
            }

            // Terminada random
            var terminados = pagosPorCliente
                .SelectMany(kv => kv.Value.Where(p => !p.Activo && Parse(p.FechaFin) < DateTime.Today)
                .Select(p => new { ClienteId = kv.Key, Pago = p, Fin = Parse(p.FechaFin) }))
                .ToList();
            if (terminados.Any())
            {
                var randomTerm = terminados[rand.Next(terminados.Count)];
                Terminada.Title = "Terminada";
                Terminada.Tag = randomTerm.Pago.Membresia ?? "";
                Terminada.DateText = randomTerm.Fin?.ToString("dd/MM/yy") ?? "-";
                Terminada.Name = clientes.First(c => c.Id == randomTerm.ClienteId).Nombres;
            }
        }
        finally { IsBusy = false; }
    }


    void SetCardDefaults()
 {
 NuevoCliente.Title = "Nuevo cliente"; NuevoCliente.Tag = ""; NuevoCliente.DateText = "-"; NuevoCliente.Name = "-";
 ClienteFiel.Title = "Cliente fiel"; ClienteFiel.Tag = ""; ClienteFiel.DateText = "-"; ClienteFiel.Name = "-";
 TerminaPronto.Title = "Termina pronto"; TerminaPronto.Tag = ""; TerminaPronto.DateText = "-"; TerminaPronto.Name = "-";
 Terminada.Title = "Terminada"; Terminada.Tag = ""; Terminada.DateText = "-"; Terminada.Name = "-";
 }

 public event PropertyChangedEventHandler? PropertyChanged;
 protected void OnPropertyChanged([CallerMemberName] string? name = null)
 => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
