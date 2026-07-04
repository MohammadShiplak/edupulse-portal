namespace School_Project_API.Entities
{
    public class Class
    {

      public int Id { get; set; }

        public int ?TeacherId { get; set; }  
        public string Name { get; set; }


        public int Capacity { get; set; }
        public string Description { get; set; }


        public int DepartmentId { get; set; }   // FK
        public Department Department { get; set; }  // Navigation


        public Teacher ?Teacher { get; set; }

        public ICollection<StudentClass> StudentClasses { get; set; }=new List<StudentClass>();

        public ICollection<ClassSubject> ClassSubjects { get; set; } =new List<ClassSubject>(); 









    }
}
