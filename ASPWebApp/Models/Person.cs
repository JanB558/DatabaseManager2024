using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPWebApp.Models
{
    public class Person
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("First name")]
        public required string FirstName { get; set; }
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("Last name")]
        public required string LastName { get; set; }
        //public int CourseID { get; set; }
        public byte[] VersionStamp { get; set; } = [];
    }
}
