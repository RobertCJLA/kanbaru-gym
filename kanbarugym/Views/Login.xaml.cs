using kanbarugym;

namespace kanbarugym.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void OnSendLogin(object sender, EventArgs e)
    {
        string email = txtEmail.Text;
        string password = txtPassword.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return;

        var app = Application.Current;
        if (app != null && app.Windows.Count > 0 && app.Windows[0] != null)
        {

            app.Windows[0].Page = new AppShell();

            Shell.Current.GoToAsync("//PanelAdministrativo");
        }
    }

}