namespace kanbarugym.Pages;

public partial class RegistrarCliente : ContentPage
{
	public RegistrarCliente()
	{
		InitializeComponent();
	}

    private void OnMemberShip(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NuevaMembresia());
    }

}