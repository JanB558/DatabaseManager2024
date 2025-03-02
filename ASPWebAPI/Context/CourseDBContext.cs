﻿using ASPWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace ASPWebAPI.Context
{
    public class CourseDBContext : DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }

        public CourseDBContext(DbContextOptions<CourseDBContext> options) : base(options)
        {
        }
    }
}
