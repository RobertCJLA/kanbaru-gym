using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using kanbarugym.Clases;
using kanbarugym.Lib;
using kanbarugym.Pages;
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

    private async void OnMemberShipPage(object sender, EventArgs e)
    {
        
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }

    // Asegúrate de que este método esté en la clase parcial 'Clients'
    private void OnRealizarPago(object sender, EventArgs e)
    {
        // Lógica para realizar el pago aquí
    }

    private void OnVerMembresias(object sender, EventArgs e)
    {
        // Lógica para manejar el evento "Ver membresías"
        // Puedes dejarlo vacío o implementar la navegación deseada
    }

    // Agrega este método en la clase 'Clients' (code-behind de Clients.xaml)
    private void OnActualizar(object sender, EventArgs e)
    {
        // Lógica para actualizar el cliente
        // Puedes obtener el cliente seleccionado usando el CommandParameter si es necesario
    }

    // Asegúrate de que este método esté en la clase parcial 'Clients'
    private void OnEliminar(object sender, EventArgs e)
    {
        // Aquí puedes agregar la lógica para eliminar el cliente seleccionado.
        // Por ejemplo, puedes obtener el cliente desde el CommandParameter si es necesario.
    }
}