using System;
using System.Text.RegularExpressions;
using kanbarugym.Lib;
using Microsoft.Maui.Controls;

namespace kanbarugym.Pages
{
    public partial class EditarCliente : ContentPage
    {
        private readonly string _id;

        public EditarCliente(string id, string nombres, string fechaNacimiento, string correoElectronico, string telefono, string sexo)
        {
            InitializeComponent();
            _id = id;
            txtNombreCliente.Text = nombres;
            txtFechaNacimiento.Text = fechaNacimiento;
            txtNECliente.Text = correoElectronico;
            txtNTClient.Text = telefono;
            cmbSexo.Title = sexo;
        }

        // Método controlador de eventos para el botón "Guardar"
        private async void OnEditClient(object sender, EventArgs e)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(txtNombreCliente.Text))
            { await DisplayAlert("Error", "El nombre es obligatorio.", "OK"); return; }

            if (string.IsNullOrWhiteSpace(txtFechaNacimiento.Text) ||
                !DateTime.TryParse(txtFechaNacimiento.Text, out var fn))
            { await DisplayAlert("Error", "Fecha de nacimiento inválida.", "OK"); return; }

            if (string.IsNullOrWhiteSpace(txtNECliente.Text) ||
                !Regex.IsMatch(txtNECliente.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            { await DisplayAlert("Error", "Correo electrónico inválido.", "OK"); return; }

            if (string.IsNullOrWhiteSpace(txtNTClient.Text) || txtNTClient.Text.Replace(" ", "").Length < 7)
            { await DisplayAlert("Error", "Número de teléfono inválido.", "OK"); return; }

            var sexo = string.IsNullOrWhiteSpace(cmbSexo.Title) ? null : cmbSexo.Title;
            if (string.IsNullOrWhiteSpace(sexo))
            { await DisplayAlert("Error", "Selecciona el sexo.", "OK"); return; }

            var body = new
            {
                id = _id,
                nombres = txtNombreCliente.Text.Trim(),
                fechaNacimiento = fn.ToString("yyyy-MM-dd"),
                correoElectronico = txtNECliente.Text.Trim(),
                telefono = txtNTClient.Text.Trim(),
                sexo = sexo
            };

            var (ok, error) = await ClientesLib.ActualizarCliente(_id, body);
            if (!ok)
            {
                await DisplayAlert("Error", $"No se pudo actualizar el cliente. Detalle: {error}", "OK");
                return;
            }

            // Notificar a la lista para que refresque
            MessagingCenter.Send(this, "ClienteActualizado");
            await DisplayAlert("Éxito", "Cliente actualizado correctamente.", "OK");
            await Navigation.PopAsync();
        }
    }
}