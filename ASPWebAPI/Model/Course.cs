namespace ASPWebAPI.Model
{
    public class Course
    {
        public int ID { get; set; }
        public required string CourseName { get; set; }

        public ICollection<Person> Persons { get; set; } = new List<Person>();
    }
}
