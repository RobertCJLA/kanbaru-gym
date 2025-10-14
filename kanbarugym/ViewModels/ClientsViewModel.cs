using System.Collections.ObjectModel;
using System.Windows.Input;
using kanbarugym.Clases;
using kanbarugym.Lib;

namespace kanbarugym.ViewModels;

public class ClientsViewModel
{
    public ObservableCollection<ClientesClass> Clientes { get; } = new();

    private List<ClientesClass> _allClientes = new();

    public ClientsViewModel()
    {
        _ = CargarClientes();
    }

    public async Task CargarClientes()
    {
        var clientes = await ClientesLib.ObtenerClientes();
        if (clientes is null) return;
        _allClientes = clientes;
        ApplyFilter();
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
                // Debe contener todos los tokens (nombre o apellidos) en cualquier orden
                return tokens.All(t => nombre.Contains(t, StringComparison.OrdinalIgnoreCase));
            });
        }

        // Colapsar todas las cards al aplicar filtro para evitar estados inconsistentes
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
}
