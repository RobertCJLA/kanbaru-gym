using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using kanbarugym.Clases;
using kanbarugym.Lib;

namespace kanbarugym.ViewModels;

public partial class ClientsViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ClientesClass> Clientes { get; } = new();

    private List<ClientesClass> _allClientes = new();

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy == value) return;
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public ClientsViewModel()
    {
        _ = CargarClientes();
    }

    public async Task CargarClientes()
    {
        try
        {
            IsBusy = true;
            var clientes = await ClientesLib.ObtenerClientes();
            _allClientes = clientes ?? [];
            ApplyFilter();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private string? _searchText;
    public string? SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText == value) return;
            _searchText = value;
            ApplyFilter();
        }
    }

    private void ApplyFilter()
    {
        IEnumerable<ClientesClass> filtered = _allClientes;
        var query = (SearchText ?? string.Empty).Trim();
        if (!string.IsNullOrWhiteSpace(query))
        {
            var tokens = query.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            filtered = _allClientes.Where(c =>
            {
                var nombre = c.Nombres ?? string.Empty;
                return tokens.All(t => nombre.Contains(t, StringComparison.OrdinalIgnoreCase));
            });
        }

        foreach (var it in _allClientes)
            it.IsExpanded = false;

        Clientes.Clear();
        foreach (var c in filtered)
            Clientes.Add(c);
    }

    ICommand? _toggleExpandCommand;
    public ICommand ToggleExpandCommand => _toggleExpandCommand ??= new Command<ClientesClass>(cliente =>
    {
        if (cliente is null) return;
        var willExpand = !cliente.IsExpanded;
        foreach (var it in Clientes)
            it.IsExpanded = false;
        cliente.IsExpanded = willExpand;
    });

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


}
