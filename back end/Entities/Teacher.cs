namespace School_Project_API.Entities
{
    public class Teacher
    {

        public int Id { get; set; }

        public int? DepartmentId { get; set; }
        public string Name { get; set; }
  
        public string Specialization { get; set; }
       
        public DateTime HireDate { get; set; }

        public Department ?Department { get; set; }

        public ICollection<Subject> Subject { get; set; } = new List<Subject>();       

        public ICollection<Class> Class { get; set;}  =new List<Class>();    
    }
}
