using ASPWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

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
            HttpResponseMessage response = await _httpClient.GetAsync("/personenrollmentcount");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var people = JsonConvert.DeserializeObject<List<PersonCourseCount>>(content);
                if (people is null)
                    return StatusCode((int)response.StatusCode, "No content.");
                return View(people);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var response = await _httpClient.DeleteAsync($"/enrollment/{id}");
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
            var response = await _httpClient.GetAsync($"/enrollmentcompleteperson/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(content);
                if (enrollments is null || enrollments.Count == 0)
                {
                    //no content to show, just get basic info
                    response = await _httpClient.GetAsync($"person/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                        var person = JsonConvert.DeserializeObject<Person>(content);
                        if (person is null)
                            return NoContent();
                        ViewBag.PersonId = person.ID;
                        ViewBag.FirstName = person.FirstName;
                        ViewBag.LastName = person.LastName;
                        return View(enrollments);
                    }
                    return StatusCode((int)response.StatusCode, "Error calling the API");                  
                }
                if (enrollments[0].Person is null) return Content("API returned incomplete data. Cannot proceed.");
                ViewBag.PersonId = enrollments[0].PersonID;
                ViewBag.FirstName = enrollments[0].Person!.FirstName; //can't be null - we just checked above
                ViewBag.LastName = enrollments[0].Person!.LastName;
                return View(enrollments);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [HttpGet]
        public async Task<IActionResult> Enroll(int id)
        {        
            EnrollPersonPageModel model = new();
            //get person data
            var response = await _httpClient.GetAsync($"/person/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var person = JsonConvert.DeserializeObject<Person>(content);
                if (person is null)
                    return NoContent();
                Debug.WriteLine($"personId {person.ID}");
                model.PersonId = person.ID;
                model.FirstName = person.FirstName;
                model.LastName = person.LastName;
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error calling the API");
            }
            //get course
            response = await _httpClient.GetAsync("/course");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<Course>>(content);
                if (courses is null || courses.Count == 0)
                    return NoContent();

                var courseList = CourseListToSelectListItem(courses.ToList()).ToList();
                courseList.First().Selected = true;
                Debug.WriteLine($"courseList {courseList.Count}");                             
                model.Courses = courseList;                         
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error calling the API");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(EnrollPersonPageModel model)
        {
            HttpResponseMessage response;
            //attempt to perform POST
            if (ModelState.IsValid)
            {
                Enrollment e = new()
                {
                    PersonID = model.PersonId,
                    CourseID = model.SelectedCourseId,
                    EnrollmentDate = DateTime.Now //no need to save to UTC since DB stores it as DATE without hour
                };
                response = await _httpClient.PostAsJsonAsync("/enrollment", e);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"{response.StatusCode} Success");
                    return RedirectToAction("Details", new { id = model.PersonId });
                }
                else
                {
                    Debug.WriteLine($"{response.StatusCode} Error");
                    ModelState.AddModelError(string.Empty, "Error while creating entity.");
                }
            }
            //POST failed - repopulate the list and go back to page
            response = await _httpClient.GetAsync("/course");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<Course>>(content);
                if (courses is null || courses.Count == 0)
                    return NoContent();

                var courseList = CourseListToSelectListItem(courses.ToList()).ToList();
                courseList.First().Selected = true;
                Debug.WriteLine($"courseList {courseList.Count}");
                model.Courses = courseList;
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error calling the API");
            }
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">enrollment id to mark as completed</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Complete(int id)
        {
            Debug.WriteLine("Controller Person, Action Complete");
            HttpResponseMessage response;
            Enrollment? enrollment;
            //get the enrollment, and update it
            response = await _httpClient.GetAsync($"/enrollment/{id}");
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Success getting the enrollment");
                var content = await response.Content.ReadAsStringAsync();
                enrollment = JsonConvert.DeserializeObject<Enrollment>(content);
                if (enrollment is null)
                    return NoContent();
                enrollment.CompletionDate = DateTime.Now; //mark as completed, no need to convert hour to UTC since there is no hour in db
            }
            else
            {
                Debug.WriteLine("Failed to get the enrollment");
                return StatusCode((int)response.StatusCode, "Error calling the API");
            }
            //post the enrollment
            response = await _httpClient.PutAsJsonAsync($"/enrollment", enrollment);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Successfully posted the enrollment");
                return RedirectToAction("Details", new { id = enrollment.PersonID });
            }
            else
            {
                Debug.WriteLine("Failed to post enrollment");
                return StatusCode((int)response.StatusCode, "Error calling the API");
            }
        }

        #region private_methods
        //this could be separate service
        private ICollection<SelectListItem> CourseListToSelectListItem(ICollection<Course> courses)
        {
            var courseList = courses.Select(
                    c => new SelectListItem
                    {
                        Value = c.ID.ToString(),
                        Text = c.CourseName
                    }).ToList();
            return courseList;
        }
        #endregion
    }
}
