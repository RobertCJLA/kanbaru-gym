using kanbarugym.Clases;
using kanbarugym.Lib;

namespace kanbarugym.Pages;

public partial class PagosCliente : ContentPage
{
    private List<PagoClass> pagos = [];

    public PagosCliente(string id, string nombre)
    {
        InitializeComponent();
        Title = $"Pagos de {nombre}";
        CargarPagos(id);
    }

    private async void CargarPagos(string id)
    {
        pagos = await PagoLib.ObtenerPagosClientes(id);

        pagos.Sort((x, y) => y.FechaInicio.CompareTo(x.FechaInicio));

        PagosCollection.ItemsSource = pagos;

        if (pagos.Count == 0)
        {
            labelPagos.IsVisible = true;
        }
    }
    private async void EliminarPago(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is string idPago)
        {
            await DisplayAlert("Eliminar", $"Eliminar pago con ID: {idPago}", "OK");
        }
    }
}