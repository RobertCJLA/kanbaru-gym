namespace kanbarugym
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnStartClicked(object sender, EventArgs e)
        {
            // Solución para CS0618 y CS8602:
            // Usar Windows[0].Page para cambiar la página raíz de la ventana principal.
            var app = Application.Current;
            if (app != null && app.Windows.Count > 0 && app.Windows[0] != null)
            {
                app.Windows[0].Page = new Login();
            }
        }
    }
}
