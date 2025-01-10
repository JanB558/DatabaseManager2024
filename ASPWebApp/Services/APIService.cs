
using System.Configuration;
using System.Net.Http;

namespace ASPWebApp.Services
{
    public class APIService<T> : IAPIService<T>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiUrl;
        public APIService(HttpClient httpClient, IConfiguration configuration) 
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiUrl = _configuration.GetConnectionString("ApiConnectionString"); 
            // WARNING - REAL CONNECTION STRING SHOULD NOT BE PUSHED TO GITHUB
            if (_apiUrl == null) throw new NullReferenceException("Missing connection string.");
            _httpClient.BaseAddress = new Uri(_apiUrl);
        }
        public async Task<HttpResponseMessage?> Delete(string endpoint, int id)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.DeleteAsync($"{endpoint}/{id}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage?> Get(string endpoint)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(endpoint);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<HttpResponseMessage?> Get(string endpoint, int id)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync($"{endpoint}/{id}");
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage?> Post(string endpoint, T model)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsJsonAsync($"{endpoint}", model);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage?> Put(string endpoint, T model)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PutAsJsonAsync($"{endpoint}", model);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
