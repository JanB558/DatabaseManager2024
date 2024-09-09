using ASPWebAPI.Context;
using ASPWebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace ASPWebAPI.Services
{
    public class SQLServerService : ISQLServerService
    {
        private readonly CourseDBContext _context;

        public SQLServerService(CourseDBContext context)
        {
            _context = context;
        }
        //people
        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            return await _context.Person.Include(x => x.Course).ToListAsync();
        }
        public async Task<Person> GetPersonAsync(int id)
        {
            return await _context.Person
                .Include(x => x.Course)
                .Where(p => p.ID == id)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> AddPersonAsync(Person person)
        {
            await _context.Person.AddAsync(person);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdatePersonAsync(Person person)
        {
            var personToUpdate = await _context.Person.FindAsync(person.ID);

            if (personToUpdate is null) return false;

            personToUpdate.FirstName = person.FirstName;
            personToUpdate.LastName = person.LastName;
            personToUpdate.CourseID = person.CourseID;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeletePersonAsync(int id)
        {
            var person = await _context.Person.FindAsync(id);

            if (person is null) return false;

            _context.Person.Remove(person);
            await _context.SaveChangesAsync();

            return true;
        }
        //course
        public Task<bool> DeleteCourseAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<Course> GetCourseAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Course>> GetCoursesAsync()
        {
            throw new NotImplementedException();
        }     
        public Task<bool> UpdateCourseAsync(Course course)
        {
            throw new NotImplementedException();
        }  
    }
}
