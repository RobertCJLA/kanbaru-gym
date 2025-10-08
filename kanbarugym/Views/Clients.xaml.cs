using kanbarugym.Clases;
using kanbarugym.Lib;

namespace kanbarugym.Views;

public partial class Clients : ContentPage
{
    public Clients()
    {
        InitializeComponent();
        CargarClientes();
    }

    public async void CargarClientes()
    {
        var clientes = await ClientesLib.ObtenerClientes() as List<ClientesClass>;
        ClientesCollection.ItemsSource = clientes;
    }
}