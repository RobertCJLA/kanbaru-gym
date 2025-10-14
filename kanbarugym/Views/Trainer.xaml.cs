using kanbarugym.ViewModels;

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
}