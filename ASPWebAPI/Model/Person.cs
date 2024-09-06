namespace ASPWebAPI.Model
{
    public class Person
    {
        public int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int CourseID { get; set; }
    }
}
