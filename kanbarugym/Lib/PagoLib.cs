using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbarugym.Lib
{
    public class PagoLib
    {
        public static async Task<string> CrearPago(object pago)
        {
            var json = JsonConvert.SerializeObject(pago);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient api = new APIService().ObtenerClientHttp();

            var response = await api.PostAsync("pagos", content);
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
                return result?.message ?? "Pago creado";
            }
        }
    }
}