﻿using ASPWebAPI.Context;
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

        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            return await _context.Person.Include(x => x.Course).ToListAsync();
        }

        public Task<bool> AddPersonAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCourseAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePersonAsync(int id)
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

        

        public Task<Person> GetPersonAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCourseAsync(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePersonAsync(Person person)
        {
            throw new NotImplementedException();
        }
    }
}
