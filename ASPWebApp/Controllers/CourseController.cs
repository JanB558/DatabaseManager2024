﻿using ASPWebApp.Models;
using ASPWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

namespace ASPWebApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly IAPIService<Course> _apiService;
        public CourseController(IAPIService<Course> apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _apiService.Get("/courseenrollmentcount");
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var courses = JsonConvert.DeserializeObject<List<CoursePersonCount>>(content);
                return View(courses);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        public async Task<IActionResult> Create(Course model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.Post("/course", model);
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
            var response = await _apiService.Get("/course", id);
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var course = JsonConvert.DeserializeObject<Course>(content);
                if (course is null)
                    return Content($"204 No Content");
                return View(course);
            }
            return StatusCode((int)response.StatusCode, "Error calling the API");
        }

        [HttpPost]
        public async Task<IActionResult> Update(Course model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.Put("/course", model);
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
            var response = await _apiService.Delete("/course", id);
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
            var response = await _apiService.Get("/enrollmentcompletecourse", id);
            if (response == null) return Content("500 Internal Server Error");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var enrollments = JsonConvert.DeserializeObject<List<Enrollment>>(content);
                if (enrollments is null || enrollments.Count == 0)
                {
                    //no content, adjust
                    //response = await _httpClient.GetAsync($"course/{id}");
                    var response2 = await _apiService.Get("/course", id);
                    if (response2 == null) return Content("500 Internal Server Error");
                    if (response2.IsSuccessStatusCode)
                    {
                        content = await response2.Content.ReadAsStringAsync();
                        var course = JsonConvert.DeserializeObject<Course>(content);
                        if (course is null) return Content($"204 No Content");
                        ViewBag.CourseId = course.ID;
                        ViewBag.CourseName = course.CourseName;
                        return View(enrollments);
                    }
                    return StatusCode((int)response2.StatusCode, "Error calling the API");
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
