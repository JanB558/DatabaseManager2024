using ASPWebApp.Dto;
using ASPWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ASPWebApp.Controllers
{
    /// <summary>
    /// Obsolete TODO remove later
    /// </summary>
    public class SummaryController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string? _apiUrl;
        public SummaryController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiUrl = _configuration.GetConnectionString("ApiConnectionString");
            //WARNING - CONNECTION STRING SHOULD NOT BE PUSHED TO GITHUB - IT'S JUST AN EXAMPLE
            if (_apiUrl == null) throw new NullReferenceException("Missing connection string.");
            _httpClient.BaseAddress = new Uri(_apiUrl);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Course()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/courseenrollmentcount");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<CoursePersonCount>>(content);
                if (courses is null) return StatusCode(200, "List is null.");
                SummaryCoursePageModel scpm = new();
                scpm.CourseList = courses.ToList();
                return View(scpm);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }
    }
}
