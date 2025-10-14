using System.Collections.ObjectModel;
using System.Windows.Input;
using kanbarugym.Clases;
using kanbarugym.Lib;

namespace kanbarugym.ViewModels;

public class TrainersViewModel
{
    public ObservableCollection<EntrenadorClass> Entrenadores { get; } = new();
    private List<EntrenadorClass> _all = new();

    public TrainersViewModel()
    {
        _ = CargarEntrenadores();
    }

    public async Task CargarEntrenadores()
    {
        var list = await EntrenadoresLib.ObtenerEntrenadores();
        _all = list ?? [];
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
        var query = (SearchText ?? string.Empty).Trim();
        IEnumerable<EntrenadorClass> filtered = _all;
        if (!string.IsNullOrWhiteSpace(query))
        {
            var tokens = query.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            filtered = _all.Where(c => tokens.All(t => (c.Nombres ?? string.Empty).Contains(t, StringComparison.OrdinalIgnoreCase)));
        }
        foreach (var it in _all) it.IsExpanded = false;
        Entrenadores.Clear();
        foreach (var e in filtered) Entrenadores.Add(e);
    }

    ICommand? _toggleExpandCommand;
    public ICommand ToggleExpandCommand => _toggleExpandCommand ??= new Command<EntrenadorClass>(e =>
    {
        if (e is null) return;
        var willExpand = !e.IsExpanded;
        foreach (var it in Entrenadores) it.IsExpanded = false;
        e.IsExpanded = willExpand;
    });
}
