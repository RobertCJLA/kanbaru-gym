using System;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace kanbarugym.Pages
{
    public partial class RegistrarEntrenador : ContentPage
    {
        public ObservableCollection<string> Especialidades { get; set; }

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

        private async void OnCreateEntrenador(object sender, EventArgs e)
        {
            // Validar nombre
            if (string.IsNullOrWhiteSpace(txtNombreEntrenador.Text))
            {
                await DisplayAlert("Error", "El nombre es obligatorio.", "OK");
                return;
            }

            // Validar fecha de nacimiento (formato YYYY-MM-DD)
            if (string.IsNullOrWhiteSpace(txtFechaNacimientoEntrenador.Text) ||
                !DateTime.TryParse(txtFechaNacimientoEntrenador.Text, out DateTime fechaNacimiento))
            {
                await DisplayAlert("Error", "Ingresa una fecha válida en formato YYYY-MM-DD.", "OK");
                return;
            }

            // Validar sexo
            if (cmbSexoEntrenador.SelectedIndex == -1)
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
            if (cmbEspecialidad.SelectedIndex == -1)
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

            // Si todo está bien, mostrar mensaje de éxito
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
