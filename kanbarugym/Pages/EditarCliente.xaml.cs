using System;
using Microsoft.Maui.Controls;

namespace kanbarugym.Pages
{
    public partial class EditarCliente : ContentPage
    {
        public EditarCliente(string id, string nombres, string fechaNacimiento, string correoElectronico, string telefono, string sexo)
        {
            InitializeComponent();
            txtNombreCliente.Text = nombres;
            txtFechaNacimiento.Text = fechaNacimiento;
            txtNECliente.Text = correoElectronico;
            txtNTClient.Text = telefono;
            cmbSexo.Title = sexo;
        }

        // Método controlador de eventos para el botón "Guardar"
        private void OnEditClient(object sender, EventArgs e)
        {
            // Aquí puedes agregar la lógica para editar el cliente
            // Por ejemplo, validar y guardar los datos ingresados
        }
    }
}