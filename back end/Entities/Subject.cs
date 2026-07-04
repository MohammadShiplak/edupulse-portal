using System.Text.Json.Serialization;

namespace School_Project_API.Entities
{
    public class Subject
    {

        public int Id { get; set; } 

        public int CourseId { get; set; }

        public int TeacherId { get; set; }  
        public string ?SubjectName { get; set; }

        public int DepartmentId { get; set; }   // FK
        public Department Department { get; set; }  // Navigation
        public decimal Price { get; set; }

        public Course Course { get; set; }

        public Teacher Teacher { get; set; }    

        public ICollection<Student> Student { get; set; }  = new List<Student>();
        public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();

    }
}
