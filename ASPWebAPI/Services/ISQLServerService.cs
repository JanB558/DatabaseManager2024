using ASPWebAPI.Model;

namespace ASPWebAPI.Services
{
    public interface ISQLServerService
    {
        public Task<IEnumerable<Person>> GetPeopleAsync();
        public Task<Person?> GetPersonAsync(int id);
        public Task<Person> AddPersonAsync(Person person);
        public Task<bool> UpdatePersonAsync(Person person);
        public Task<bool> DeletePersonAsync(int id);
        //
        public Task<IEnumerable<Course>> GetCoursesAsync();
        public Task<Course?> GetCourseAsync(int id);
        public Task<Course> AddCourseAsync(Course course);
        public Task<bool> UpdateCourseAsync(Course course);
        public Task<bool> DeleteCourseAsync(int id);
        //
        Task<IEnumerable<Enrollment>> GetEnrollmentsAsync();
        Task<IEnumerable<Enrollment>> GetEnrollmentsWithDetailsAsync();
        Task<Enrollment?> GetEnrollmentAsync(int id);
        Task<bool> UpdateEnrollmentAsync(Enrollment enrollment);
        Task<IEnumerable<Enrollment>> GetEnrollmentsWithDetailsByPersonAsync(int personID);
        Task<IEnumerable<Enrollment>> GetEnrollmentsWithDetailsByCourseAsync(int courseID);
        Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment);
        Task<bool> DeleteEnrollmentAsync(int enrollmentID);
        //
        Task<IEnumerable<CoursePersonCount>> GetCoursesWithPersonCountAsync();
        Task<IEnumerable<PersonCourseCount>> GetPeopleWithCourseCountAsync();
    }
}
