namespace ASPWebApp.Models.Dto
{
    public class PersonDetailsPageModel
    {
        public Person? Person { get; set; }
        public List<Course> CourseList { get; set; } = [];
    }
}
