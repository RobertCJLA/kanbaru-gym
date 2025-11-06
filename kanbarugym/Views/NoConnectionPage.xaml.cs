using Microsoft.Maui.Controls;
using Microsoft.Maui.Networking;

namespace kanbarugym.Views;

public partial class NoConnectionPage : ContentPage
{
    public NoConnectionPage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    private async void OnRetryClicked(object sender, EventArgs e)
    {
        if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
        {
            await Navigation.PopModalAsync();
        }
        else
        {
            await DisplayAlert("Sin conexión", "Todavía no hay Internet.", "OK");
        }
    }
}
