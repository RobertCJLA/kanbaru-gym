// Trainer.xaml.cs
using kanbarugym.ViewModels;
using kanbarugym.Clases;
using Microsoft.Maui.Controls;

namespace kanbarugym.Views;

public partial class Trainer : ContentPage
{
    public TrainersViewModel ViewModel { get; } = new();
    private ImageButton? _currentGearBtn;
    private EntrenadorClass? _currentTrainer;

    public Trainer()
    {
        InitializeComponent();
        BindingContext = ViewModel;
        EntrenadoresCollection.SizeChanged += (_, __) => RepositionOverlay();
        RootGrid.SizeChanged += (_, __) => RepositionOverlay();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.CargarEntrenadores();
    }

    private void RepositionOverlay()
    {
        var overlay = this.FindByName<kanbarugym.Views.Controls.FloatingMenu>("OverlayMenu");
        if (overlay is null || !overlay.IsOpen || _currentGearBtn is null)
            return;
        overlay.ShowFor(_currentGearBtn, RootGrid, EntrenadoresCollection);
    }

    private void OnGearClicked(object sender, EventArgs e)
    {
        if (sender is not ImageButton btn || btn.BindingContext is not EntrenadorClass entrenador)
            return;
        var overlay = this.FindByName<kanbarugym.Views.Controls.FloatingMenu>("OverlayMenu");
        if (overlay is null) return;

        // Toggle si el mismo engrane
        if (overlay.IsOpen && ReferenceEquals(_currentTrainer, entrenador))
        {
            overlay.Hide();
            _currentGearBtn = null;
            _currentTrainer = null;
            return;
        }

        _currentGearBtn = btn;
        _currentTrainer = entrenador;
        overlay.BindingContext = entrenador;
        overlay.ShowFor(btn, RootGrid, EntrenadoresCollection);
    }
}
