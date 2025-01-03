using System.ComponentModel.DataAnnotations;

namespace ASPWebAPI.Model
{
    public class Course
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string CourseName { get; set; }
        [Timestamp]
        public byte[]? VersionStamp { get; set; }

        public void Copy(Course other)
        {
            this.CourseName = other.CourseName;
        }
    }
}
