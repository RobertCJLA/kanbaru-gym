// Trainer.xaml.cs
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
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.CargarEntrenadores(); // refresca cada vez que vuelves a la pestaña
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
            var show = !entrenador.IsMenuVisible;
            CloseAllMenus();
            entrenador.IsMenuVisible = show;
        }
    }
}
