using kanbarugym.Clases;
using kanbarugym.Lib;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace kanbarugym.ViewModels
{
    public partial class ClientsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ClientesClass> Clientes { get; } = new();
        private List<ClientesClass> _allClientes = new();

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { if (_isBusy == value) return; _isBusy = value; OnPropertyChanged(); }
        }

        public ICommand ToggleExpandCommand { get; }
        public ICommand ToggleMenuCommand { get; }
        public ICommand CloseAllMenusCommand { get; }

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set { if (_searchText == value) return; _searchText = value; ApplyFilter(); }
        }

        public ClientsViewModel()
        {
            ToggleExpandCommand = new Command<ClientesClass>(ToggleExpand);
            ToggleMenuCommand = new Command<ClientesClass>(ToggleMenu);
            CloseAllMenusCommand = new Command(CloseAllMenus);
        }

        public async Task CargarClientes()
        {
            try
            {
                IsBusy = true;
                var clientes = await ClientesLib.ObtenerClientes();
                _allClientes = clientes ?? new List<ClientesClass>();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error cargando clientes: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ApplyFilter()
        {
            var filtered = _allClientes.AsEnumerable();
            var query = (SearchText ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(query))
            {
                var tokens = query.Split(' ', System.StringSplitOptions.RemoveEmptyEntries | System.StringSplitOptions.TrimEntries);
                filtered = _allClientes.Where(c => tokens.All(t => (c.Nombres ?? "").Contains(t, System.StringComparison.OrdinalIgnoreCase)));
            }

            foreach (var c in _allClientes) c.IsExpanded = false;

            Clientes.Clear();
            foreach (var c in filtered) Clientes.Add(c);
        }

        private void ToggleExpand(ClientesClass cliente)
        {
            if (cliente == null) return;
            var willExpand = !cliente.IsExpanded;
            foreach (var c in Clientes) c.IsExpanded = false;
            cliente.IsExpanded = willExpand;
        }

        private void ToggleMenu(ClientesClass cliente)
        {
            if (cliente == null) return;
            var willShow = !cliente.IsMenuVisible;

            foreach (var c in Clientes) c.IsMenuVisible = false;

            cliente.IsMenuVisible = willShow;
            OnPropertyChanged(nameof(AnyMenuOpen));
        }

        private void CloseAllMenus()
        {
            foreach (var c in Clientes) c.IsMenuVisible = false;
            OnPropertyChanged(nameof(AnyMenuOpen));
        }

        public bool AnyMenuOpen => Clientes.Any(c => c.IsMenuVisible);

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
