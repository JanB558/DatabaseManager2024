namespace ASPWebApp.Models.Dto
{
    public class EnrollPersonPageModel
    {
        public int PersonId { get;set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get;set; } = string.Empty;
        public List<Course> Courses { get; set; } = [];
    }

    public class EnrollPersonPagePostModel
    {
        public int PersonId { get;set; }
        public int CourseId { get; set; }
    }
}
