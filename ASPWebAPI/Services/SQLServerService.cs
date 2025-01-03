using ASPWebAPI.Context;
using ASPWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace ASPWebAPI.Services
{
    // MS SQL Server implementation
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
        public async Task<Person?> GetPersonAsync(int id)
        {
            return await _context.Person
                .Where(p => p.ID == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Person> AddPersonAsync(Person person)
        {
            ArgumentNullException.ThrowIfNull(person);
            await _context.Person.AddAsync(person);
            await _context.SaveChangesAsync();
            return person;
        }
        public async Task<bool> UpdatePersonAsync(Person person)
        {
            ArgumentNullException.ThrowIfNull(person);
            var personToUpdate = await _context.Person.FindAsync(person.ID);

            if (personToUpdate is null) 
                return false;
            if (!personToUpdate.VersionStamp.SequenceEqual(person.VersionStamp)) 
                throw new DbUpdateConcurrencyException("The data has changed. Another user edited this record.");

            personToUpdate.Copy(person);

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
        public async Task<Course?> GetCourseAsync(int id)
        {
            return await _context.Course
                .Where(c => c.ID == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Course> AddCourseAsync(Course course)
        {
            ArgumentNullException.ThrowIfNull(course);
            await _context.Course.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }
        public async Task<bool> UpdateCourseAsync(Course course)
        {
            ArgumentNullException.ThrowIfNull(course);
            var courseToUpdate = await _context.Course.FindAsync(course.ID);

            if (courseToUpdate is null) 
                return false;
            if (!courseToUpdate.VersionStamp.SequenceEqual(course.VersionStamp))
                throw new DbUpdateConcurrencyException("The data has changed. Another user edited this record.");

            courseToUpdate.Copy(course);

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
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsWithDetailsAsync()
        {
            return await _context.Enrollment
                .Include(x => x.Person)
                .Include(x => x.Course)
                .ToListAsync();
        }
        public async Task<bool> UpdateEnrollmentAsync(Enrollment enrollment)
        {
            ArgumentNullException.ThrowIfNull(enrollment);
            var enrollmentToUpdate = await _context.Enrollment.FindAsync(enrollment.ID);

            if (enrollmentToUpdate is null)
                return false;
            if (!enrollmentToUpdate.VersionStamp.SequenceEqual(enrollment.VersionStamp))
                throw new DbUpdateConcurrencyException("The data has changed. Another user edited this record.");

            enrollmentToUpdate.Copy(enrollment);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<Enrollment>> GetEnrollmentsPersonAsync(int personID)
        {
            return await _context.Enrollment
                .Include(x => x.Person)
                .Where(x => x.PersonID.Equals(personID))
                .ToListAsync();
        }
        public async Task<Enrollment> AddEnrollmentAsync(Enrollment enrollment)
        {
            ArgumentNullException.ThrowIfNull(enrollment);
            await _context.Enrollment.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }
        public async Task<bool> DeleteEnrollmentAsync(int enrollmentID)
        {
            var enrollment = await _context.Enrollment.FindAsync(enrollmentID);

            if (enrollment is null) return false;

            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region mix
        public async Task<IEnumerable<CoursePersonCount>> GetCoursesWithPersonCountAsync()
        {
            return await _context.Course
                .GroupJoin(
                    _context.Enrollment,
                    course => course.ID,
                    enrollment => enrollment.CourseID,
                    (course, enrollments) => new CoursePersonCount
                    {
                        ID = course.ID,
                        CourseName = course.CourseName,
                        EnrollmentCount = enrollments.Count()
                    })
                .ToListAsync();
        }
        #endregion
    }
}
