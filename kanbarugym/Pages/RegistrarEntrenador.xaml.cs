using System;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using kanbarugym.Lib;
using kanbarugym.Clases;
using System.Globalization;

namespace kanbarugym.Pages
{
    public partial class RegistrarEntrenador : ContentPage
    {
        public ObservableCollection<string> Especialidades { get; set; }

        private bool _isEdit;
        private EntrenadorClass? _original;

        public RegistrarEntrenador()
        {
            InitializeComponent();

            //lista de especialidades
            Especialidades = new ObservableCollection<string>
            {
                "Cardio",
                "Fuerza",
                "Yoga",
                "Pilates"
            };

            // Establecer el contexto de enlace de datos
            BindingContext = this;
        }

        public RegistrarEntrenador(EntrenadorClass entrenador) : this()
        {
            _isEdit = true;
            _original = entrenador;
            Title = "Editar entrenador";

            // Prefill campos manteniendo el formato yyyy-MM-dd
            txtNombreEntrenador.Text = entrenador.Nombres;
            if (DateTime.TryParseExact(entrenador.FechaNacimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                txtFechaNacimientoEntrenador.Text = dt.ToString("yyyy-MM-dd");
            else if (DateTime.TryParse(entrenador.FechaNacimiento, out var dt2))
                txtFechaNacimientoEntrenador.Text = dt2.ToString("yyyy-MM-dd");
            else
                txtFechaNacimientoEntrenador.Text = entrenador.FechaNacimiento;

            cmbSexoEntrenador.SelectedItem = entrenador.Sexo;
            txtExperiencia.Text = entrenador.Experiencia.ToString();
            cmbEspecialidad.SelectedItem = entrenador.Especialidad;
            txtCorreoEntrenador.Text = entrenador.CorreoElectronico;
            txtTelefonoEntrenador.Text = entrenador.Telefono;
        }

        private async void OnCreateEntrenador(object sender, EventArgs e)
        {
            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombreEntrenador.Text))
            {
                await DisplayAlert("Error", "El nombre es obligatorio.", "OK");
                return;
            }

            // Validar fecha de nacimiento (formato yyyy-MM-dd)
            if (string.IsNullOrWhiteSpace(txtFechaNacimientoEntrenador.Text) ||
                !DateTime.TryParseExact(txtFechaNacimientoEntrenador.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fechaNacimiento))
            {
                await DisplayAlert("Error", "Ingresa una fecha válida en formato yyyy-MM-dd.", "OK");
                return;
            }

            // Validar sexo
            if (cmbSexoEntrenador.SelectedIndex == -1 || cmbSexoEntrenador.SelectedItem is not string sexo)
            {
                await DisplayAlert("Error", "Selecciona el sexo.", "OK");
                return;
            }

            // Validar experiencia
            if (string.IsNullOrWhiteSpace(txtExperiencia.Text) ||
                !int.TryParse(txtExperiencia.Text, out int experiencia) || experiencia < 0)
            {
                await DisplayAlert("Error", "Ingresa una experiencia válida en años.", "OK");
                return;
            }

            // Validar especialidad
            if (cmbEspecialidad.SelectedIndex == -1 || cmbEspecialidad.SelectedItem is not string especialidad)
            {
                await DisplayAlert("Error", "Selecciona una especialidad.", "OK");
                return;
            }

            // Validar correo electrónico
            if (string.IsNullOrWhiteSpace(txtCorreoEntrenador.Text) ||
                !Regex.IsMatch(txtCorreoEntrenador.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                await DisplayAlert("Error", "Ingresa un correo electrónico válido.", "OK");
                return;
            }

            // Validar número de teléfono
            if (string.IsNullOrWhiteSpace(txtTelefonoEntrenador.Text) ||
                txtTelefonoEntrenador.Text.Length < 7)
            {
                await DisplayAlert("Error", "Ingresa un número de teléfono válido.", "OK");
                return;
            }

            if (_isEdit && _original is not null)
            {
                // Actualizar (enviar la fecha en yyyy-MM-dd)
                var updateBody = new
                {
                    id = _original.Id,
                    nombres = txtNombreEntrenador.Text.Trim(),
                    fechaNacimiento = fechaNacimiento.ToString("yyyy-MM-dd"),
                    especialidad,
                    experiencia,
                    correoElectronico = txtCorreoEntrenador.Text.Trim(),
                    telefono = txtTelefonoEntrenador.Text.Trim(),
                    sexo,
                };

                var (ok, error) = await EntrenadoresLib.ActualizarEntrenador(_original.Id, updateBody);
                if (!ok)
                {
                    await DisplayAlert("Error", $"No se pudo actualizar el entrenador. Detalle: {error}", "OK");
                    return;
                }

                await DisplayAlert("Éxito", "Entrenador actualizado correctamente.", "OK");
                await Navigation.PopAsync();
                return;
            }

            // Crear nuevo (enviar la fecha en yyyy-MM-dd)
            var entrenador = new
            {
                id = "",
                nombres = txtNombreEntrenador.Text.Trim(),
                fechaNacimiento = fechaNacimiento.ToString("yyyy-MM-dd"),
                especialidad,
                experiencia,
                correoElectronico = txtCorreoEntrenador.Text.Trim(),
                telefono = txtTelefonoEntrenador.Text.Trim(),
                sexo,
            };

            var (okCreate, errorCreate) = await EntrenadoresLib.CrearEntrenador(entrenador);
            if (!okCreate)
            {
                await DisplayAlert("Error", $"No se pudo guardar el entrenador. Detalle: {errorCreate}", "OK");
                return;
            }

            await DisplayAlert("Éxito", "Entrenador registrado correctamente.", "OK");

            // Opcional: limpiar campos
            txtNombreEntrenador.Text = string.Empty;
            txtFechaNacimientoEntrenador.Text = string.Empty;
            cmbSexoEntrenador.SelectedIndex = -1;
            txtExperiencia.Text = string.Empty;
            cmbEspecialidad.SelectedIndex = -1;
            txtCorreoEntrenador.Text = string.Empty;
            txtTelefonoEntrenador.Text = string.Empty;
        }
    }
}
