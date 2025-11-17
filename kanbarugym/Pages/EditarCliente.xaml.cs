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


            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                IsVisible = false
            });
        }



        private async void OnCancelButton(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


        private async void OnEditClient(object sender, EventArgs e)
        {
            string nombres = txtNombreCliente.Text;
            string fechaNacimiento = txtFechaNacimiento.Text;
            string correoElectronico = txtNECliente.Text;
            string telefono = txtNTClient.Text;
            string? sexo = (cmbSexo.SelectedItem?.ToString() == "Femenino") ? "F" : (cmbSexo.SelectedItem != null ? "M" : null);

            Regex nameRegex = new(@"^(?:[A-ZÁÉÍÓÚ][a-záéíóú]+(?:\s[A-ZÁÉÍÓÚ][a-záéíóú]+){0,4})$");
            Regex emailRegex = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            Regex dateRegex = new(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$");
            Regex phoneRegex = new(@"^[0-9]{8}$");

            if (string.IsNullOrWhiteSpace(nombres) ||
                string.IsNullOrWhiteSpace(fechaNacimiento) ||
                string.IsNullOrWhiteSpace(correoElectronico) ||
                string.IsNullOrWhiteSpace(telefono) ||
                string.IsNullOrWhiteSpace(sexo))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
                return;
            }

            if (!nameRegex.IsMatch(nombres))
            {
                await DisplayAlert("Error", "Nombre inválido.", "OK");
                return;
            }

            if (!emailRegex.IsMatch(correoElectronico))
            {
                await DisplayAlert("Error", "Correo electrónico inválido.", "OK");
                return;
            }

            if (!dateRegex.IsMatch(fechaNacimiento))
            {
                await DisplayAlert("Error", "Fecha de nacimiento inválida. Use el formato AAAA-MM-DD.", "OK");
                return;
            }

            if (!phoneRegex.IsMatch(telefono))
            {
                await DisplayAlert("Error", "Número de teléfono inválido. Debe contener exactamente 8 dígitos.", "OK");
                return;
            }

            var body = new
            {
                id = "",
                nombres,
                fechaNacimiento,
                correoElectronico,
                telefono,
                sexo
            };

            var (ok, error) = await ClientesLib.ActualizarCliente(_id, body);
            if (!ok)
            {
                await DisplayAlert("Error", $"No se pudo actualizar el cliente. Detalle: {error}", "OK");
                return;
            }

            await DisplayAlert("Éxito", "Cliente actualizado correctamente.", "OK");
            await Navigation.PopAsync();
        }
    }
}