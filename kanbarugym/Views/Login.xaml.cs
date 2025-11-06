using kanbarugym;
using kanbarugym.Lib;
using System.Threading.Tasks;
using Microsoft.Maui.Networking;

namespace kanbarugym.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}


    private async void OnSendLogin(object sender, EventArgs e)
    {
        Button btnLogi = btnLogin;
        btnLogi.Text = "Buscando";
        btnLogi.IsEnabled = false;

        string email = txtEmail.Text;
        string password = txtPassword.Text;

        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            btnLogi.Text = "Comenzar";
            btnLogi.IsEnabled = true;
            await DisplayAlert("Sin conexión", "No hay Internet. Conéctate y vuelve a intentarlo.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            btnLogi.Text = "Comenzar";
            btnLogi.IsEnabled = true;
            await DisplayAlert("Error", "Usuario o contraseña vacíos.", "OK");
            return;
        }

        string response = await new AdministradoresLib().Login(email, password);

        if (response != "Login exitoso")
        {
            btnLogi.Text = "Comenzar";
            btnLogi.IsEnabled = true;
            await DisplayAlert("Error", $"{response}.", "OK");
            return;
        }

        new AdministradoresLib().ActualizarAdministrador(email);

        var app = Application.Current;
        if (app != null && app.Windows.Count > 0 && app.Windows[0] != null)
        {
            app.Windows[0].Page = new AppShell();
            await Shell.Current.GoToAsync("//PanelAdministrativo");
        }
    }

}