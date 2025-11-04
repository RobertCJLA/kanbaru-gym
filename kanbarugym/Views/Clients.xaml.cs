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

    // Aseg�rate de que este m�todo est� en la clase parcial 'Clients'
    private void OnRealizarPago(object sender, EventArgs e)
    {
        // L�gica para realizar el pago aqu�
    }

    private void OnVerMembresias(object sender, EventArgs e)
    {
        // L�gica para manejar el evento "Ver membres�as"
        // Puedes dejarlo vac�o o implementar la navegaci�n deseada
    }

    // Agrega este m�todo en la clase 'Clients' (code-behind de Clients.xaml)
    private void OnActualizar(object sender, EventArgs e)
    {
        // L�gica para actualizar el cliente
        // Puedes obtener el cliente seleccionado usando el CommandParameter si es necesario
    }

    // Aseg�rate de que este m�todo est� en la clase parcial 'Clients'
    private void OnEliminar(object sender, EventArgs e)
    {
        // Aqu� puedes agregar la l�gica para eliminar el cliente seleccionado.
        // Por ejemplo, puedes obtener el cliente desde el CommandParameter si es necesario.
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