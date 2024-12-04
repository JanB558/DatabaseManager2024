namespace ASPWebAPI.Model
{
    public class Course
    {
        public int ID { get; set; }
        public required string CourseName { get; set; }

        public void Copy(Course other)
        {
            this.CourseName = other.CourseName;
        }
    }
}
