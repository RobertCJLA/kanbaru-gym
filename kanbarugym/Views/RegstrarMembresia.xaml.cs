using System.Text.RegularExpressions;

namespace kanbarugym.Views;

public partial class RegstrarMembresia : ContentPage
{
	public RegstrarMembresia()
	{
		InitializeComponent();
	}

	public async void OnCreateMembresia (object sender, EventArgs e)
	{
		string cliente = txtCliente.Text;
		string fechaInicio = txtFechaInicio.Text;
		string monto = txtMonto.Text;
		string? membresia = cmbMembresia.SelectedItem?.ToString();


        Regex dateRegex = new(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$");

		if(string.IsNullOrWhiteSpace(fechaInicio))
		{
			await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
            return;
		}
		if(!dateRegex.IsMatch(fechaInicio))
		{
			await DisplayAlert("Error", "Fecha de nacimiento inválida. Use el formato AAAA-MM-DD.", "OK")
				return;
		}

    }
}