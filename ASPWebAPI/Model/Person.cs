namespace ASPWebAPI.Model
{
    public class Person
    {
        public int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public void Copy(Person other)
        {
            this.FirstName = other.FirstName;
            this.LastName = other.LastName;
        }
    }
}
