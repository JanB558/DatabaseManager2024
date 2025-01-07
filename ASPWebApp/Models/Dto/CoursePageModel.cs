using ASPWebApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPWebApp.Models.Dto
{
    public class CoursePageModel : PageModel
    {
        public List<CoursePersonCount>? CourseList { get; set; }
    }
}
