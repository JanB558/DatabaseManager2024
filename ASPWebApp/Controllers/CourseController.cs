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
            _httpClient.BaseAddress = new Uri(_apiUrl);
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/course");
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

        public async Task<IActionResult> Create(Course model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("/course", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error while creating entity.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/course/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var course = JsonConvert.DeserializeObject<Course>(content);
                if (course is null) return StatusCode(200, "Course is null.");
                return View(course);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Course model)
        {
            if (ModelState.IsValid)
            {
                Debug.WriteLine($"{model.ID} {model.CourseName}");
                var response = await _httpClient.PutAsJsonAsync("/course", model);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error while updating entity.");
                }
            }
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
