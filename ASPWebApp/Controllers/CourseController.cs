using ASPWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

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
                var courses = JsonConvert.DeserializeObject<List<Course>>(content);
                if (courses is null) return StatusCode(200, "List is null.");
                CoursePageModel cpm = new();
                cpm.CourseList = courses.ToList();
                return View(cpm);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
