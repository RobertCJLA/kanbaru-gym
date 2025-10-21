using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbarugym.Lib
{
    public class AdministradoresLib
    {
        private string administrador = String.Empty;
        public async Task<string> Login(string usuario, string contrasena)
        {
            var user = new
            {
                usuario,
                contrasena
            };

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient api = new APIService().ObtenerClientHttp();

            var response = await api.PostAsync("administradores/login", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    string message = errorObj?.message ?? "Error desconocido";

                    return message;
                }
                catch
                {
                    return $"Error del servidor: {responseBody}";
                }
            }
            else
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                return result?.message ?? "Login exitoso";
            }
        }
        public void ActualizarAdministrador(string administrador)
        {
            this.administrador = administrador;
        }

        public string ObtenerAdministrador()
        {
            return this.administrador;
        }
    }
}
