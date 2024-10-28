namespace ASPWebAPI.Model
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int PersonID {  get; set; }
        public int CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }

        //navigation properties
        public Course? Course { get; set; }
        public Person? Person { get; set; }
    }
}
