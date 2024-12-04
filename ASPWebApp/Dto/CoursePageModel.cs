using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPWebApp.Dto
{
    public class CoursePageModel : PageModel
    {
        public List<CoursePersonCount>? CourseList { get; set; }
    }
}
