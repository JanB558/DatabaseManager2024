using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Course? Course { get; set; }
        [JsonIgnore]
        public Person? Person { get; set; }
    }
}
