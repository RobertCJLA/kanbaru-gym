using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

    namespace kanbarugym.Clases
{
    public class ClientesClass : INotifyPropertyChanged
    {
        public required string Id { get; set; }
        public required string Nombres { get; set; }
        public required string FechaNacimiento { get; set; }
        public required string CorreoElectronico { get; set; }
        public required string Telefono { get; set; }
        public required string Sexo { get; set; }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value) return;
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        private bool _isMenuVisible;
        public bool IsMenuVisible
        {
            get => _isMenuVisible;
            set
            {
                if (_isMenuVisible == value) return;
                _isMenuVisible = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}