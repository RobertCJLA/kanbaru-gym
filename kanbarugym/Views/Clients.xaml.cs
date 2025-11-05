using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using kanbarugym.Clases;
using kanbarugym.Lib;
using kanbarugym.Pages;
using kanbarugym.ViewModels;
using System;
using Microsoft.Maui.Controls;

namespace kanbarugym.Views;

public partial class Clients : ContentPage
{
    public ClientsViewModel ViewModel { get; } = new();

    public Clients()
    {
        InitializeComponent();
        BindingContext = ViewModel;
    }

    private void OnViewMembership(object sender, EventArgs e)
    {
        var button = sender as Button;
        if (button?.CommandParameter is ClientesClass cliente)
        {
            string id = cliente.Id;
            string nombre = cliente.Nombres;

            //Navigation.PushAsync(new RegstrarMembresia(id, nombre));
            Navigation.PushAsync(new PagosCliente(id, nombre));
        }
    }
}