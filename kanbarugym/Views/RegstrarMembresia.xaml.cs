using kanbarugym.Lib;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;

namespace kanbarugym.Views;

public partial class RegstrarMembresia : ContentPage
{
    private string id = string.Empty;
    private string name = string.Empty;

    public RegstrarMembresia(string id, string name)
    {
        InitializeComponent();
        this.id = id;
        this.name = name;

        txtCliente.Text = this.name;
        txtCliente.IsEnabled = false;
    }

    public async void OnCreateMembresia(object sender, EventArgs e)
    {
        string cliente = txtCliente.Text;
        string fechaInicio = txtFechaInicio.Text;
        string monto = txtMonto.Text;
        string? membresia = cmbMembresia.SelectedItem?.ToString();

        
        Regex dateRegex = new(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$");

        
        if (string.IsNullOrWhiteSpace(cliente) ||
            string.IsNullOrWhiteSpace(fechaInicio) ||
            string.IsNullOrWhiteSpace(monto) ||
            string.IsNullOrWhiteSpace(membresia))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
            return;
        }

       
        if (!dateRegex.IsMatch(fechaInicio))
        {
            await DisplayAlert("Error", "Fecha inválida. Use el formato AAAA-MM-DD.", "OK");
            return;
        }

        
        if (!decimal.TryParse(monto, out decimal montoDecimal) || montoDecimal <= 0)
        {
            await DisplayAlert("Error", "Monto inválido. Ingrese un número mayor a 0.", "OK");
            return;
        }

        
        if (cliente.Length < 3)
        {
            await DisplayAlert("Error", "El nombre del cliente es demasiado corto.", "OK");
            return;
        }

        string fechaFin = CalcularProximaFecha(fechaInicio, membresia);

        if(fechaFin == "Error")
        {
            await DisplayAlert("Error", "Error al calcular las fechas.", "OK");
            return;
        }

        string administrador = new AdministradoresLib().ObtenerAdministrador();

        var nuevoPago = new
        {
            idCliente = this.id,
            cliente,
            fechaInicio,
            fechaFin,
            membresia = membresia.ToLower(),
            monto = montoDecimal,
            administrador
        };

       
        string response = await PagoLib.CrearPago(nuevoPago);

        if (response == "Pago creado")
        {
            await DisplayAlert("Éxito", "Membresía registrada correctamente.", "OK");

            txtCliente.Text = this.name;
            txtFechaInicio.Text = string.Empty;
            txtMonto.Text = string.Empty;
            cmbMembresia.SelectedItem = null;
        }
        else
        {
            await DisplayAlert("Error", $"{response}.", "OK");
        }
    }

    private string CalcularProximaFecha(string fechaStr, string tipo)
    {
        DateTime fecha = DateTime.ParseExact(fechaStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime nuevaFecha;

        switch (tipo.ToLower()) {
            case "semanal":
                nuevaFecha = fecha.AddDays(7);
                break;
            case "mensual":
                nuevaFecha = fecha.AddMonths(1);
                break;
            case "anual":
                nuevaFecha = fecha.AddYears(1);
                break;
            default:
                return "Error";
        }

        return nuevaFecha.ToString("yyyy-MM-dd");
    }
}