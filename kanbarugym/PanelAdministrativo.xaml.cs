using kanbarugym.Pages;

namespace kanbarugym;

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