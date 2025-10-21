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
        public static async Task<bool> CrearPago(object pago)
        {
            var json = JsonConvert.SerializeObject(pago);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient api = new APIService().ObtenerClientHttp();

            var response = await api.PostAsync("pagos", content);

            return response.IsSuccessStatusCode;
        }


    }
}