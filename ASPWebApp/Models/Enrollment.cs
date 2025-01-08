namespace ASPWebApp.Models
{
    public class Enrollment
    {
        public int ID { get; set; }
        public int PersonID { get; set; }
        public int CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public byte[] VersionStamp { get; set; } = [];
        public Course? Course { get; set; }
        public Person? Person { get; set; }
    }
}
