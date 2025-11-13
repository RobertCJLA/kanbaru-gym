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
 if (clientes.Count ==0) return;

 var reciente = clientes.Last();
 NuevoCliente.Title = "Nuevo cliente"; NuevoCliente.Tag = "Ingreso";
 NuevoCliente.DateText = DateTime.Now.ToString("dd/MM/yy"); NuevoCliente.Name = reciente.Nombres;

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
 if (DateTime.TryParseExact(s, f, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)) return d;
 if (DateTime.TryParse(s, out var gen)) return gen; return null;
 }

 ClientesClass? fielCliente = null; DateTime? fielStart = null; string? fielMembresia = null;
 foreach (var kv in pagosPorCliente)
 {
 var first = kv.Value.Select(p => new { P = p, Start = Parse(p.FechaInicio) }).Where(x => x.Start.HasValue).OrderBy(x => x.Start).FirstOrDefault();
 if (first is null) continue;
 if (fielStart is null || first.Start!.Value < fielStart.Value)
 { fielStart = first.Start; fielCliente = clientes.FirstOrDefault(c => c.Id == kv.Key); fielMembresia = first.P.Membresia; }
 }
 if (fielCliente != null && fielStart != null)
 { ClienteFiel.Title = "Cliente fiel"; ClienteFiel.Tag = fielMembresia ?? string.Empty; ClienteFiel.DateText = fielStart.Value.ToString("dd/MM/yy"); ClienteFiel.Name = fielCliente.Nombres; }

 ClientesClass? prontoCliente = null; DateTime? prontoFin = null; string? prontoMemb = null;
 foreach (var kv in pagosPorCliente)
 {
 var soon = kv.Value.Select(p => new { P = p, Fin = Parse(p.FechaFin), p.Activo }).Where(x => x.Fin.HasValue && x.Fin >= DateTime.Today && x.Activo).OrderBy(x => x.Fin).FirstOrDefault();
 if (soon is null) continue;
 if (prontoFin is null || soon.Fin!.Value < prontoFin.Value)
 { prontoFin = soon.Fin; prontoCliente = clientes.FirstOrDefault(c => c.Id == kv.Key); prontoMemb = soon.P.Membresia; }
 }
 if (prontoCliente != null && prontoFin != null)
 { TerminaPronto.Title = "Termina pronto"; TerminaPronto.Tag = prontoMemb ?? string.Empty; TerminaPronto.DateText = prontoFin.Value.ToString("dd/MM/yy"); TerminaPronto.Name = prontoCliente.Nombres; }

 ClientesClass? terminadaCliente = null; DateTime? terminadaFin = null; string? terminadaMemb = null;
 foreach (var kv in pagosPorCliente)
 {
 var lastEnded = kv.Value.Select(p => new { P = p, Fin = Parse(p.FechaFin), p.Activo }).Where(x => x.Fin.HasValue && x.Fin < DateTime.Today && !x.Activo).OrderByDescending(x => x.Fin).FirstOrDefault();
 if (lastEnded is null) continue;
 if (terminadaFin is null || lastEnded.Fin!.Value > terminadaFin.Value)
 { terminadaFin = lastEnded.Fin; terminadaCliente = clientes.FirstOrDefault(c => c.Id == kv.Key); terminadaMemb = lastEnded.P.Membresia; }
 }
 if (terminadaCliente != null && terminadaFin != null)
 { Terminada.Title = "Terminada"; Terminada.Tag = terminadaMemb ?? string.Empty; Terminada.DateText = terminadaFin.Value.ToString("dd/MM/yy"); Terminada.Name = terminadaCliente.Nombres; }
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
