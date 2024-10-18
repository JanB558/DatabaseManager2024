using Microsoft.AspNetCore.Mvc;

namespace ASPWebApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;
        public CourseController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiUrl = _configuration.GetConnectionString("ApiConnectionString");
        }

        public async Task<IActionResult> Index()
        {
            var endpoint = $"{_apiUrl}/course";
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
            //return View("../DBM/Course");
        }
    }
}
