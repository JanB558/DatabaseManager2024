using System.ComponentModel.DataAnnotations;

namespace ASPWebAPI.Model
{
    public class Person
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string FirstName { get; set; }
        [StringLength(50, MinimumLength = 2)]
        public required string LastName { get; set; }

        public void Copy(Person other)
        {
            this.FirstName = other.FirstName;
            this.LastName = other.LastName;
        }
    }
}
