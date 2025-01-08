using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPWebApp.Models.Dto
{
    public class EnrollPersonPageModel
    {
        public int PersonId { get;set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get;set; } = string.Empty;
        public int SelectedCourseId { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; } = [];
    }
}
