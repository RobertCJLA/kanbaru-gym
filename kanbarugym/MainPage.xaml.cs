namespace kanbarugym
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigateToNextPage();
        }
        static private async void NavigateToNextPage()
        {
            await Task.Delay(4000);

            var app = Application.Current;
            if (app != null && app.Windows.Count > 0 && app.Windows[0] != null)
            {
                app.Windows[0].Page = new Login();
            }
        }
    }
}
