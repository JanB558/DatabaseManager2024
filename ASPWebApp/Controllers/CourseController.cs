using Microsoft.AspNetCore.Mvc;

namespace ASPWebApp.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
