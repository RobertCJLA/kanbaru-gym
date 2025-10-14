using kanbarugym.Clases;
using kanbarugym.Lib;
using kanbarugym.ViewModels;

namespace kanbarugym.Views;

public partial class Clients : ContentPage
{
    public ClientsViewModel ViewModel { get; } = new();

    public Clients()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }
}