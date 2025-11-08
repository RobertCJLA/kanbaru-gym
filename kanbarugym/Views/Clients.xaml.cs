// Clients.xaml.cs
using kanbarugym.Clases;
using kanbarugym.Pages;
using kanbarugym.ViewModels;
using Microsoft.Maui.Controls;
using kanbarugym.Views.Controls;

namespace kanbarugym.Views;

public partial class Clients : ContentPage
{
    public ClientsViewModel ViewModel { get; } = new();
    ClientesClass? _currentClient;
    VisualElement? _currentAnchor;

    public Clients()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.CargarClientes();
    }

    void OnGearClicked(object sender, EventArgs e)
    {
        if (sender is not ImageButton btn || btn.CommandParameter is not ClientesClass cliente) return;
        var menu = ClientsMenu; // x:Name in XAML
        if (menu == null) return;

        if (menu.IsOpen && ReferenceEquals(_currentClient, cliente))
        {
            menu.Hide();
            _currentClient = null;
            _currentAnchor = null;
            return;
        }

        _currentClient = cliente;
        _currentAnchor = btn;
        menu.TargetClient = cliente;

        // Asignar comandos (navegación) usando closures con cliente actual
        menu.PayCommand = new Command(() => Navigation.PushAsync(new RegstrarMembresia(cliente.Id, cliente.Nombres)));
        menu.ViewCommand = new Command(() => Navigation.PushAsync(new PagosCliente(cliente.Id, cliente.Nombres)));
        menu.EditCommand = new Command(() => Navigation.PushAsync(new EditarCliente(cliente.Id, cliente.Nombres, cliente.FechaNacimiento, cliente.CorreoElectronico, cliente.Telefono, cliente.Sexo)));

        menu.ShowFor(btn, RootGrid, ClientesCollection);
    }
}
