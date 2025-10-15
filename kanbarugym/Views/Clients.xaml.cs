using kanbarugym.Clases;
using kanbarugym.Lib;
using kanbarugym.ViewModels;
using kanbarugym.Pages;

namespace kanbarugym.Views;

public partial class Clients : ContentPage
{
    public ClientsViewModel ViewModel { get; } = new();

    public Clients()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }

    private void OnMemberShipPage(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NuevaMembresia());
    }
}