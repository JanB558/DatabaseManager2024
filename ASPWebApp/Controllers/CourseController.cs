using ASPWebApp.Models;
using ASPWebApp.Models.Dto;
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
            _apiUrl = _configuration.GetConnectionString("ApiConnectionString"); // WARNING - REAL CONNECTION STRING SHOULD NOT BE PUSHED TO GITHUB
            if (_apiUrl == null) throw new NullReferenceException("Missing connection string.");
            _httpClient.BaseAddress = new Uri(_apiUrl);
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/courseenrollmentcount");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<CoursePersonCount>>(content);
                if (courses is null) 
                    return StatusCode((int)response.StatusCode, "No content.");
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
                if (course is null) 
                    return StatusCode((int)response.StatusCode, "No content.");
                return View(course);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Course model)
        {
            if (ModelState.IsValid)
            {
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/course/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error calling the API");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/enrollmentcompletecourse/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(content);
                if (enrollments is null || enrollments.Count == 0)
                {
                    //no content, adjust
                    response = await _httpClient.GetAsync($"course/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        var course = JsonConvert.DeserializeObject<Course>(content);
                        if (course is null) return NoContent();
                        ViewBag.CourseId = course.ID;
                        ViewBag.CourseName = course.CourseName;
                        return View(enrollments);
                    }
                    return StatusCode((int)response.StatusCode, "Error calling the API");
                }
                if (enrollments[0].Course is null) return Content("API returned incomplete data. Cannot proceed.");
                ViewBag.CourseId = enrollments[0].CourseID;
                ViewBag.CourseName = enrollments[0].Course!.CourseName; //can't be null, checked above
                return View(enrollments);
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
