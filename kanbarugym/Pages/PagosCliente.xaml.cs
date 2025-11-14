using kanbarugym.Clases;
using kanbarugym.Lib;

namespace kanbarugym.Pages;

public partial class PagosCliente : ContentPage
{
    private List<PagoClass> pagos = new();

    public PagosCliente(string id, string nombre)
    {
        InitializeComponent();
        Title = $"Pagos de {nombre}";

        // Llamamos a la carga de datos de manera segura después de que la página aparece
        this.Appearing += async (s, e) =>
        {
            try
            {
                await CargarPagosAsync(id);
            }
            catch (Exception ex)
            {
                // Mostrar error para debug sin cerrar la app
                await DisplayAlert("Error", ex.Message + "\n" + ex.StackTrace, "OK");
            }
        };
    }

    private async Task CargarPagosAsync(string id)
    {
        // Aseguramos que nunca sea null
        var pagosObtenidos = await PagoLib.ObtenerPagosClientes(id) ?? new List<PagoClass>();
        pagosObtenidos.Sort((x, y) => y.FechaInicio.CompareTo(x.FechaInicio));

        pagos.Clear();
        pagos.AddRange(pagosObtenidos);

        PagosCollection.ItemsSource = pagos;

        labelPagos.IsVisible = pagos.Count == 0;
    }

    private async void EliminarPago(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is string idPago)
        {
            await DisplayAlert("Eliminar", $"Eliminar pago con ID: {idPago}", "OK");
        }
    }
}
