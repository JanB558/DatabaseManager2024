using ASPWebApp.Models;
using ASPWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

namespace ASPWebApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly IAPIService<Person> _apiService;
        private readonly IAPIService<Enrollment> _apiEnrollmentService;
        public PersonController(IAPIService<Person> apiService, IAPIService<Enrollment> apiService2)
        {
            _apiService = apiService;
            _apiEnrollmentService = apiService2;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.Get("/personenrollmentcount");
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var people = JsonConvert.DeserializeObject<List<PersonCourseCount>>(content);
                return View(people);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Person model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.Post("/person", model);
                if (response == null) return Content("500 Internal Server Error");
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
            var response = await _apiService.Get("/person", id);
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var person = JsonConvert.DeserializeObject<Person>(content);
                if (person is null)
                    return Content($"204 No Content");
                return View(person);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Person model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.Put("/person", model);
                if (response == null) return Content("500 Internal Server Error");
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
            var response = await _apiService.Delete("/person", id);
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
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
            var response = await _apiService.Delete("/enrollment", id);
            if (response == null) return Content("500 Internal Server Error");
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
            var response = await _apiService.Get("/enrollmentcompleteperson", id);
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(content);
                if (enrollments is null || enrollments.Count == 0)
                {
                    //no content to show, just get basic info
                    var response2 = await _apiService.Get("/person", id);
                    if (response2 == null) return Content("500 Internal Server Error");
                    if (response2.IsSuccessStatusCode)
                    {
                        content = await response2.Content.ReadAsStringAsync();
                        var person = JsonConvert.DeserializeObject<Person>(content);
                        if (person is null)
                            return Content($"204 No Content");
                        ViewBag.PersonId = person.ID;
                        ViewBag.FirstName = person.FirstName;
                        ViewBag.LastName = person.LastName;
                        return View(enrollments);
                    }
                    return StatusCode((int)response2.StatusCode, "Error calling the API");                  
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
            var response = await _apiService.Get("/person", id);
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var person = JsonConvert.DeserializeObject<Person>(content);
                if (person is null)
                    return Content("204 No Content");
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
            response = await _apiService.Get("/course");
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<Course>>(content);
                if (courses is null || courses.Count == 0)
                    return Content($"204 No Content");

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
            HttpResponseMessage? response;
            //attempt to perform POST
            if (ModelState.IsValid)
            {
                Enrollment e = new()
                {
                    PersonID = model.PersonId,
                    CourseID = model.SelectedCourseId,
                    EnrollmentDate = DateTime.Now.ToUniversalTime()
                };
                response = await _apiEnrollmentService.Post("/enrollment", e);
                if (response == null) return Content("500 Internal Server Error");
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
            response = await _apiService.Get("/course");
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<Course>>(content);
                if (courses is null || courses.Count == 0)
                    return Content("204 No Content");

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
            HttpResponseMessage? response;
            Enrollment? enrollment;
            //get the enrollment, and update it
            response = await _apiService.Get("/enrollment", id);
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Success getting the enrollment");
                var content = await response.Content.ReadAsStringAsync();
                enrollment = JsonConvert.DeserializeObject<Enrollment>(content);
                if (enrollment is null)
                    return Content("204 No Content");
                enrollment.CompletionDate = DateTime.Now.ToUniversalTime(); //mark as completed
            }
            else
            {
                Debug.WriteLine("Failed to get the enrollment");
                return StatusCode((int)response.StatusCode, "Error calling the API");
            }
            //post/update the enrollment
            response = await _apiEnrollmentService.Put("/enrollment", enrollment);
            if (response == null) return Content("500 Internal Server Error");
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
