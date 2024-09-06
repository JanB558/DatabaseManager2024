using ASPWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace ASPWebAPI.Context
{
    public class CourseDBContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Course> Courses { get; set; }

        public CourseDBContext(DbContextOptions<CourseDBContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Course>().ToTable("Course");
        }
    }
}
