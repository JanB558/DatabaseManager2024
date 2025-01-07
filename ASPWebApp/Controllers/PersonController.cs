using ASPWebApp.Models;
using ASPWebApp.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASPWebApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiUrl;
        public PersonController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiUrl = _configuration.GetConnectionString("ApiConnectionString"); // WARNING - REAL CONNECTION STRING SHOULD NOT BE PUSHED TO GITHUB
            if (_apiUrl == null) throw new NullReferenceException("Missing connection string.");
            _httpClient.BaseAddress = new Uri(_apiUrl);
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/person");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var people = JsonConvert.DeserializeObject<List<Person>>(content);
                if (people is null)
                    return StatusCode((int)response.StatusCode, "No content.");
                PersonPageModel ppm = new();
                ppm.PersonList = people.ToList();
                return View(ppm);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }
    }
}
