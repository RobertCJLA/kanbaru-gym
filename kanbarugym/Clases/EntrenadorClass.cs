using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace kanbarugym.Clases;

public class EntrenadorClass : INotifyPropertyChanged
{
    public required string Id { get; set; }
    public required string Nombres { get; set; }
    public required string FechaNacimiento { get; set; }
    public required string CorreoElectronico { get; set; }
    public required string Telefono { get; set; }
    public required string Sexo { get; set; }
    public string? Especialidad { get; set; }
    public int Experiencia { get; set; }

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

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
