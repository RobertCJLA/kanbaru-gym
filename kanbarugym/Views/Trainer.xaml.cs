using kanbarugym.ViewModels;
using kanbarugym.Clases;

namespace kanbarugym.Views;

public partial class Trainer : ContentPage
{
    public TrainersViewModel ViewModel { get; } = new();

    public Trainer()
    {
        InitializeComponent();
        BindingContext = ViewModel;
        // El seeder fue eliminado. El ViewModel carga datos si existen.
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.CargarEntrenadores();
    }

    private void CloseAllMenus()
    {
        foreach (var it in ViewModel.Entrenadores)
            it.IsMenuVisible = false;
    }

    private void OnGearClicked(object sender, EventArgs e)
    {
        if (sender is ImageButton btn && btn.BindingContext is EntrenadorClass entrenador)
        {
            // Alternar menú anclado al engranaje
            var show = !entrenador.IsMenuVisible;
            CloseAllMenus();
            entrenador.IsMenuVisible = show;
        }
    }

    private void OnEditTrainer(object? sender, TappedEventArgs e)
    {
        if ((sender as Element)?.BindingContext is EntrenadorClass entrenador)
        {
            CloseAllMenus();
            if (ViewModel.EditTrainerCommand?.CanExecute(entrenador) == true)
                ViewModel.EditTrainerCommand.Execute(entrenador);
        }
    }

    private void OnDeleteTrainer(object? sender, TappedEventArgs e)
    {
        if ((sender as Element)?.BindingContext is EntrenadorClass entrenador)
        {
            CloseAllMenus();
            if (ViewModel.DeleteTrainerCommand?.CanExecute(entrenador) == true)
                ViewModel.DeleteTrainerCommand.Execute(entrenador);
        }
    }
}