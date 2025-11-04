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
        var button = sender as Button;
        if(button?.CommandParameter is ClientesClass cliente)
        {
            string id = cliente.Id;
            string nombre = cliente.Nombres;

            //Navigation.PushAsync(new RegstrarMembresia(id, nombre));
            Navigation.PushAsync(new PagosCliente(id, nombre));
        }
    }
}