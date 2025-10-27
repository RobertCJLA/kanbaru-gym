using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using kanbarugym.Clases;
using kanbarugym.Lib;
using Microsoft.Maui.Controls;

namespace kanbarugym.Pages
{
 public partial class EditarEntrenador : ContentPage
 {
 public ObservableCollection<string> Especialidades { get; set; }
 private readonly EntrenadorClass _original;

 public EditarEntrenador(EntrenadorClass entrenador)
 {
 InitializeComponent();
 _original = entrenador;

 Especialidades = new ObservableCollection<string>
 {
 "Cardio",
 "Fuerza",
 "Yoga",
 "Pilates"
 };
 BindingContext = this;

 txtNombreEntrenador.Text = entrenador.Nombres;
 // Normalizar formato a yyyy-MM-dd sin hora ni separadores diferentes
 if (DateTime.TryParseExact(entrenador.FechaNacimiento, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtExact))
 txtFechaNacimientoEntrenador.Text = dtExact.ToString("yyyy-MM-dd");
 else if (DateTime.TryParse(entrenador.FechaNacimiento, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtAny))
 txtFechaNacimientoEntrenador.Text = dtAny.ToString("yyyy-MM-dd");
 else
 txtFechaNacimientoEntrenador.Text = entrenador.FechaNacimiento.Replace('/', '-').Split(' ')[0];

 cmbSexoEntrenador.SelectedItem = entrenador.Sexo;
 txtExperiencia.Text = entrenador.Experiencia.ToString();
 cmbEspecialidad.SelectedItem = entrenador.Especialidad;
 txtCorreoEntrenador.Text = entrenador.CorreoElectronico;
 txtTelefonoEntrenador.Text = entrenador.Telefono;
 }

 private async void OnUpdateEntrenador(object sender, EventArgs e)
 {
 if (string.IsNullOrWhiteSpace(txtNombreEntrenador.Text))
 { await DisplayAlert("Error", "El nombre es obligatorio.", "OK"); return; }

 if (string.IsNullOrWhiteSpace(txtFechaNacimientoEntrenador.Text) ||
 !DateTime.TryParseExact(txtFechaNacimientoEntrenador.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fechaNacimiento))
 { await DisplayAlert("Error", "Ingresa una fecha válida en formato yyyy-MM-dd.", "OK"); return; }

 if (cmbSexoEntrenador.SelectedIndex == -1 || cmbSexoEntrenador.SelectedItem is not string sexo)
 { await DisplayAlert("Error", "Selecciona el sexo.", "OK"); return; }

 if (string.IsNullOrWhiteSpace(txtExperiencia.Text) || !int.TryParse(txtExperiencia.Text, out int experiencia) || experiencia <0)
 { await DisplayAlert("Error", "Ingresa una experiencia válida en años.", "OK"); return; }

 if (cmbEspecialidad.SelectedIndex == -1 || cmbEspecialidad.SelectedItem is not string especialidad)
 { await DisplayAlert("Error", "Selecciona una especialidad.", "OK"); return; }

 if (string.IsNullOrWhiteSpace(txtCorreoEntrenador.Text) ||
 !Regex.IsMatch(txtCorreoEntrenador.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
 { await DisplayAlert("Error", "Ingresa un correo electrónico válido.", "OK"); return; }

 if (string.IsNullOrWhiteSpace(txtTelefonoEntrenador.Text) || txtTelefonoEntrenador.Text.Length <7)
 { await DisplayAlert("Error", "Ingresa un número de teléfono válido.", "OK"); return; }

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
 { await DisplayAlert("Error", $"No se pudo actualizar el entrenador. Detalle: {error}", "OK"); return; }

 await DisplayAlert("Éxito", "Entrenador actualizado correctamente.", "OK");
 await Navigation.PopToRootAsync();
 }

 private async void OnCancel(object sender, EventArgs e)
 { await Navigation.PopAsync(); }
 }
}
