namespace ASPWebAPI.Model
{
    public class CoursePersonCount : Course
    {
        public int EnrollmentCount { get; set; }
        public void Copy(CoursePersonCount other)
        { 
            this.EnrollmentCount = other.EnrollmentCount;
            this.CourseName = other.CourseName;
        }
    }
}
