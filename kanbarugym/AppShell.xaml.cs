using System.ComponentModel;

namespace kanbarugym
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            PropertyChanged += OnShellPropertyChanged;
        }

        private async void OnShellPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentItem))
            {
                try
                {
                    await Navigation.PopToRootAsync(false);
                }
                catch
                {
                    // Ignorar si no hay stack
                }
            }
        }
    }
}
