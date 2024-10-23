using ASPWebAPI.Context;
using ASPWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace ASPWebAPI.Services
{
    public class SQLServerService : ISQLServerService
    {
        private readonly CourseDBContext _context;

        public SQLServerService(CourseDBContext context)
        {
            _context = context;
        }

        #region people
        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            return await _context.Person.ToListAsync();
        }
        public async Task<Person> GetPersonAsync(int id)
        {
            return await _context.Person
                .Where(p => p.ID == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Person> AddPersonAsync(Person person)
        {
            await _context.Person.AddAsync(person);
            await _context.SaveChangesAsync();
            return person;
        }
        public async Task<bool> UpdatePersonAsync(Person person)
        {
            var personToUpdate = await _context.Person.FindAsync(person.ID);

            if (personToUpdate is null) return false;

            personToUpdate.FirstName = person.FirstName;
            personToUpdate.LastName = person.LastName;

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
        #endregion

        #region course
        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            return await _context.Course.ToListAsync();
        }
        public async Task<Course> GetCourseAsync(int id)
        {
            return await _context.Course
                .Where(c => c.ID == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Course> AddCourseAsync(Course course)
        {
            await _context.Course.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }
        public async Task<bool> UpdateCourseAsync(Course course)
        {
            var courseToUpdate = await _context.Course.FindAsync(course.ID);

            if (courseToUpdate is null) return false;

            courseToUpdate.CourseName = course.CourseName;

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _context.Course.FindAsync(id);

            if (course is null) return false;

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region enrollment
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsAsync()
        {
            return await _context.Enrollment.ToListAsync();
        }
        #endregion
    }
}
