using Microsoft.AspNetCore.Mvc;

namespace ASPWebApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiUrl;
        public CourseController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiUrl = _configuration.GetConnectionString("ApiConnectionString"); 
            //WARNING - CONNECTION STRING SHOULD NOT BE PUSHED TO GITHUB - IT'S JUST AN EXAMPLE
            if (_apiUrl == null) throw new NullReferenceException("Missing connection string.");
        }

        public async Task<IActionResult> Index()
        {
            var endpoint = $"{_apiUrl}/course";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return View();
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }
    }
}
