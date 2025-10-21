using kanbarugym.Lib;
using System.Text.RegularExpressions;

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

        
        var nuevoPago = new
        {
            cliente,
            fechaInicio,
            monto = montoDecimal,
            membresia
        };

       
        bool response = await PagoLib.CrearPago(nuevoPago);
        if (response)
        {
            await DisplayAlert("Éxito", "Membresía registrada correctamente.", "OK");

            txtCliente.Text = this.name;
            txtFechaInicio.Text = string.Empty;
            txtMonto.Text = string.Empty;
            cmbMembresia.SelectedItem = null;
        }
        else
        {
            await DisplayAlert("Error", "No se pudo registrar la membresía. Intente nuevamente.", "OK");
        }
    }
}