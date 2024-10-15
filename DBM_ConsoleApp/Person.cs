using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBM_ConsoleApp
{
    internal class Person
    {
        public int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int CourseID { get; set; }

        public Course? Course { get; set; }
    }
}
