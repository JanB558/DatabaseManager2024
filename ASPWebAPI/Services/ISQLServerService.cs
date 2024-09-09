using ASPWebAPI.Model;

namespace ASPWebAPI.Services
{
    public interface ISQLServerService
    {
        public IQueryable<Person> GetPeople();
        public Person GetPersonByID(int id);
        public void UpdatePerson(Person person);
        public void DeletePerson(int id);
        //
        public IQueryable<Course> GetCourses();
        public Course GetCourseByID(int id);
        public void UpdateCourse(Course course);
        public void DeleteCourse(int id);
    }
}
