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
		string? membresia = (cmbMonto.SelectedItem != null)
	}
}