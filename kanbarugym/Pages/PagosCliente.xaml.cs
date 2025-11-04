using kanbarugym.Clases;
using kanbarugym.Lib;

namespace kanbarugym.Pages;

public partial class PagosCliente : ContentPage
{
    private List<PagoClass> pagos = [];

    public PagosCliente(string id)
    {

        InitializeComponent();
        CargarPagos(id);
    }

    private async void CargarPagos(string id)
    {
        pagos = await PagoLib.ObtenerPagosClientes(id);
        PagosCollection.ItemsSource = pagos;
    }
    private async void EliminarPago(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is string idPago)
        {
            await DisplayAlert("Eliminar", $"Eliminar pago con ID: {idPago}", "OK");
        }
    }
}