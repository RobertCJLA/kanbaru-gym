// Clients.xaml.cs
using kanbarugym.Clases;
using kanbarugym.Lib;
using kanbarugym.Pages;
using kanbarugym.ViewModels;
using Microsoft.Maui.Controls;

namespace kanbarugym.Views;

public partial class Clients : ContentPage
{
    public ClientsViewModel ViewModel { get; } = new();

    public Clients()
    {
        InitializeComponent();
        BindingContext = ViewModel;
        MessagingCenter.Subscribe<EditarCliente>(this, "ClienteActualizado", async _ =>
        {
            await ViewModel.CargarClientes();
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.CargarClientes(); // refresca cada vez que regresas a la pestaña
    }

    private void CloseAllMenus()
    {
        foreach (var c in ViewModel.Clientes)
            c.IsMenuVisible = false;
    }

    private VisualElement? FindMenuBorder(Element? start)
    {
        Element? node = start;
        for (int i = 0; i < 6 && node is not null; i++)
        {
            if (node is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    if (child is VisualElement ve && ve.AutomationId == "MenuBorder")
                        return ve;
                    if (child is Border b && b.AutomationId == "MenuBorder")
                        return b;
                }
            }
            node = node?.Parent;
        }
        return null;
    }

    private async void OnPageMembership(object sender, EventArgs e)
    {
        if ((sender as Button)?.CommandParameter is ClientesClass cliente)
            await Navigation.PushAsync(new RegstrarMembresia(cliente.Id, cliente.Nombres));
    }

    private async void OnViewMembership(object sender, EventArgs e)
    {
        if ((sender as Button)?.CommandParameter is ClientesClass cliente)
            await Navigation.PushAsync(new PagosCliente(cliente.Id, cliente.Nombres));
    }

    private async void OnEditPage(object sender, EventArgs e)
    {
        if ((sender as Button)?.CommandParameter is ClientesClass cliente)
        {
            await Navigation.PushAsync(new EditarCliente(
                cliente.Id,
                cliente.Nombres,
                cliente.FechaNacimiento,
                cliente.CorreoElectronico,
                cliente.Telefono,
                cliente.Sexo
            ));
        }
    }
}
