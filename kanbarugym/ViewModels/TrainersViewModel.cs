using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using kanbarugym.Clases;
using kanbarugym.Lib;
using kanbarugym.Pages;

namespace kanbarugym.ViewModels;

public class TrainersViewModel : INotifyPropertyChanged
{
    public ObservableCollection<EntrenadorClass> Entrenadores { get; } = new();
    private List<EntrenadorClass> _all = new();

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { if (_isBusy == value) return; _isBusy = value; OnPropertyChanged(); }
    }

    public ICommand EditTrainerCommand { get; }
    public ICommand DeleteTrainerCommand { get; }

    public TrainersViewModel()
    {
        EditTrainerCommand = new Command<EntrenadorClass>(async t => await EditAsync(t));
        DeleteTrainerCommand = new Command<EntrenadorClass>(async t => await DeleteAsync(t));
        _ = CargarEntrenadores();
    }

    public async Task CargarEntrenadores()
    {
        try
        {
            IsBusy = true;
            var list = await EntrenadoresLib.ObtenerEntrenadores();
            _all = list ?? [];
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

    private async Task EditAsync(EntrenadorClass? t)
    {
        if (t is null) return;
        // Navegar a la nueva pantalla EditarEntrenador con el entrenador seleccionado
        await Shell.Current.Navigation.PushAsync(new EditarEntrenador(t));
    }

    private async Task DeleteAsync(EntrenadorClass? t)
    {
        if (t is null) return;
        bool confirm = await Shell.Current.DisplayAlert("Confirmar", $"¿Eliminar a {t.Nombres}?", "Sí", "No");
        if (!confirm) return;

        var (ok, error) = await EntrenadoresLib.EliminarEntrenador(t.Id);
        if (!ok)
        {
            await Shell.Current.DisplayAlert("Error", $"No se pudo eliminar. {error}", "OK");
            return;
        }

        // quitar de colecciones y refrescar
        _all.RemoveAll(x => x.Id == t.Id);
        ApplyFilter();
        await Shell.Current.DisplayAlert("Eliminado", "Entrenador eliminado correctamente.", "OK");
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
