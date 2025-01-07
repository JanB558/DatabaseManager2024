using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ASPWebAPI.Model
{
    public class Enrollment
    {
        public int ID { get; set; }
        public int PersonID {  get; set; }
        public int CourseID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        [Timestamp]
        public byte[] VersionStamp { get; set; } = [];

        //navigation properties
        public Course? Course { get; set; }
        public Person? Person { get; set; }

        public void Copy(Enrollment enrollment)
        {          
            this.PersonID = enrollment.PersonID;
            this.CourseID = enrollment.CourseID;
            this.EnrollmentDate = enrollment.EnrollmentDate;
            this.CompletionDate = enrollment.CompletionDate;
        }
    }
}
