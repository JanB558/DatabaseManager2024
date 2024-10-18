using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPWebApp.Models
{
    public class CoursePageModel : PageModel
    {
        public List<Course>? CourseList { get; set; }
    }
}
