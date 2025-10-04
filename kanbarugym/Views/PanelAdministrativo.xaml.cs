using kanbarugym.Pages;
using kanbarugym.Views;

namespace kanbarugym.Views;

public partial class PanelAdministrativo : ContentPage
{
	public PanelAdministrativo()
	{
		InitializeComponent();
	}

	private void OnRegisterClient(object sender, EventArgs e)
	{
		Navigation.PushAsync(new RegistrarCliente());
	}
    private void OnRegisterCouch(object sender, EventArgs e)
    {
		Navigation.PushAsync(new RegistrarEntrenador());
    }
}