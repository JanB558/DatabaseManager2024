﻿using ASPWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace ASPWebAPI.Context
{
    public class CourseDBContext : DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<Course> Course { get; set; }

        public CourseDBContext(DbContextOptions<CourseDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Person)
                .HasForeignKey(p => p.CourseID);
        }

    }
}
