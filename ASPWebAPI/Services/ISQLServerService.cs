using ASPWebAPI.Model;

namespace ASPWebAPI.Services
{
    public interface ISQLServerService
    {
        public Task<IEnumerable<Person>> GetPeopleAsync();
        public Task<Person> GetPersonAsync(int id);
        public Task<bool> AddPersonAsync(Person person);
        public Task<bool> UpdatePersonAsync(Person person);
        public Task<bool> DeletePersonAsync(int id);
        //
        public Task<IEnumerable<Course>> GetCoursesAsync();
        public Task<Course> GetCourseAsync(int id);
        public Task<bool> AddCourseAsync(Course course);
        public Task<bool> UpdateCourseAsync(Course course);
        public Task<bool> DeleteCourseAsync(int id);
    }
}
