using kanbarugym.Clases;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace kanbarugym.Lib;

public static class EntrenadoresLib
{
    public static async Task<(bool ok, string? error)> CrearEntrenador(object entrenador)
    {
        try
        {
            var json = JsonConvert.SerializeObject(entrenador);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient api = new APIService().ObtenerClientHttp();
            var response = await api.PostAsync("entrenadores", content);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            var status = (int)response.StatusCode;
            return (false, $"HTTP {status} - {response.ReasonPhrase}. Respuesta: {body}");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public static async Task<(bool ok, string? error)> ActualizarEntrenador(string id, object entrenador)
    {
        try
        {
            var json = JsonConvert.SerializeObject(entrenador);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient api = new APIService().ObtenerClientHttp();
            var response = await api.PutAsync($"entrenadores/{id}", content);
            if (response.IsSuccessStatusCode)
                return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            var status = (int)response.StatusCode;
            return (false, $"HTTP {status} - {response.ReasonPhrase}. Respuesta: {body}");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public static async Task<(bool ok, string? error)> EliminarEntrenador(string id)
    {
        try
        {
            HttpClient api = new APIService().ObtenerClientHttp();
            var response = await api.DeleteAsync($"entrenadores/{id}");
            if (response.IsSuccessStatusCode)
                return (true, null);

            var body = await response.Content.ReadAsStringAsync();
            var status = (int)response.StatusCode;
            return (false, $"HTTP {status} - {response.ReasonPhrase}. Respuesta: {body}");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public static async Task<List<EntrenadorClass>> ObtenerEntrenadores()
    {
        HttpClient api = new APIService().ObtenerClientHttp();
        var response = await api.GetAsync("entrenadores");
        if (!response.IsSuccessStatusCode)
            return [];

        var json = await response.Content.ReadAsStringAsync();

        // Soportar tanto { "entrenadores": [...] } como directamente [ ... ]
        try
        {
            var root = JObject.Parse(json);
            var token = root["entrenadores"];
            var list = token?.ToObject<List<EntrenadorClass>>();
            if (list is not null) return list;
        }
        catch
        {
            // Ignorar, se intentará parsear como arreglo directo
        }

        try
        {
            var list = JsonConvert.DeserializeObject<List<EntrenadorClass>>(json);
            return list ?? [];
        }
        catch
        {
            return [];
        }
    }
}
