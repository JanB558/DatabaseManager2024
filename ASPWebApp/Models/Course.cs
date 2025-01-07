using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPWebApp.Models
{
    public class Course
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("Course name")]
        public required string CourseName { get; set; }
        public byte[] VersionStamp { get; set; } = [];
    }
}
