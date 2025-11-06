using System.ComponentModel;
using kanbarugym.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;

namespace kanbarugym;

public partial class AppShell : Shell
{
    private ShellItem? _previousItem;

    public AppShell()
    {
        InitializeComponent();
        PropertyChanged += OnShellPropertyChanged;

        Connectivity.Current.ConnectivityChanged += Connectivity_ConnectivityChanged;
    }

    private async void OnShellPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CurrentItem))
        {
            try
            {
                await Navigation.PopToRootAsync(false);
            }
            catch { }

            if (CurrentItem == _previousItem)
            {
                if (CurrentShellPage is Clients clientsPage)
                    await clientsPage.ViewModel.CargarClientes();
                else if (CurrentShellPage is Trainer trainerPage)
                    await trainerPage.ViewModel.CargarEntrenadores();
            }

            _previousItem = CurrentItem;
        }
    }

    private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
        if (e.NetworkAccess != NetworkAccess.Internet)
        {
            if (CurrentShellPage is not NoConnectionPage)
                await CurrentShellPage?.Navigation.PushModalAsync(new NoConnectionPage());
        }
        else
        {
            if (CurrentShellPage is NoConnectionPage)
                await CurrentShellPage.Navigation.PopModalAsync();
        }
    }

    public Page? CurrentShellPage
    {
        get
        {
            if (CurrentItem?.CurrentItem?.CurrentItem?.Content is Page page)
                return page;
            return base.CurrentPage;
        }
    }
}
