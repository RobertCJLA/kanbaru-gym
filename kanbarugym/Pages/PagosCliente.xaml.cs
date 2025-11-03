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
    }
}