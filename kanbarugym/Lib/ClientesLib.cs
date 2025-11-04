using kanbarugym.Clases;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbarugym.Lib
{
    public class ClientesLib
    {
        public static async Task<ClienteResponse> CrearCliente(object cliente)
        {
            var json = JsonConvert.SerializeObject(cliente);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient api = new APIService().ObtenerClientHttp();

            var response = await api.PostAsync("clientes", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    string message = errorObj?.message ?? "Error desconocido";

                    return new ClienteResponse { message = message, id = null};
                }
                catch
                {
                    return new ClienteResponse { message = $"Error del servidor {responseBody}", id = null};
                }
            }
            else
            {
                var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
                return new ClienteResponse { message = result?.message ?? "Cliente creado", id = result?.id ?? null };
            }
        }
        public static async Task<List<ClientesClass>> ObtenerClientes()
        {
            HttpClient api = new APIService().ObtenerClientHttp();
            var response = await api.GetAsync("clientes");

            if (!response.IsSuccessStatusCode) return [];

            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);

            var clientesToken = data["clientes"];
            var clientes = clientesToken?.ToObject<List<ClientesClass>>();

            return clientes ?? [];
        }
    }
}
