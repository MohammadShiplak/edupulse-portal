using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace School_Project_API.Entities
{
    public class StudentSubjects
    {

        public int StudentId { get; set; }

        public Student Student { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; }    

     

        // Optional: extra data about the enrollment
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public string? Grade { get; set; }

    }

}
