using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kanbarugym.Lib
{
    public class APIService
    {
        private readonly HttpClient _httpClient;

        public APIService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.1.29:3000/")
            };
        }

        public HttpClient ObtenerClientHttp()
        {
            return _httpClient;
        }

    }
}
