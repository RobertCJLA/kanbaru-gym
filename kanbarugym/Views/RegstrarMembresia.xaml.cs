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
			await DisplayAlert("Error", "Fecha de nacimiento inválida. Use el formato AAAA-MM-DD.", "OK");
				return;
		}

    }
}