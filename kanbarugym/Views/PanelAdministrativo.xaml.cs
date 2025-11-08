using kanbarugym.Pages;
using kanbarugym.ViewModels;

namespace kanbarugym.Views;

public partial class PanelAdministrativo : ContentPage
{
	public AdminPanelViewModel ViewModel { get; } = new();
	public PanelAdministrativo()
	{
		InitializeComponent();
		BindingContext = ViewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await ViewModel.LoadAsync();
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