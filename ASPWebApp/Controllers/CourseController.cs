using Microsoft.AspNetCore.Mvc;

namespace ASPWebApp.Controllers
{
    public class CourseController : Controller
    {
        [Route("Course/Course")]
        public IActionResult Index()
        {
            return View("../DBM/Course");
        }
    }
}
