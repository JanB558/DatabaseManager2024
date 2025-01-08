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

        public async Task<IActionResult> Create(Person model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("/person", model);
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
            HttpResponseMessage response = await _httpClient.GetAsync($"/person/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var person = JsonConvert.DeserializeObject<Person>(content);
                if (person is null)
                    return StatusCode((int)response.StatusCode, "No content.");
                return View(person);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Person model)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync("/person", model);
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
            var response = await _httpClient.DeleteAsync($"/person/{id}");
            if(response.IsSuccessStatusCode)
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
            HttpResponseMessage response = await _httpClient.GetAsync($"/enrollmentcompleteperson/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(content);
                if (enrollments is null)
                    return StatusCode((int)response.StatusCode, "No content.");
                return View(enrollments);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }
    }
}
