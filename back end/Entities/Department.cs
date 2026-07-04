using System.Text.Json.Serialization;

namespace School_Project_API.Entities
{
    public class Department
    {

        public int Id { get; set; }

        public string Name { get; set; }



        public ICollection<Teacher> Teachers { get; set; } =new List<Teacher>();     

       
  
    public ICollection<Subject> Subjects { get; set; } =new List<Subject>();   
    public ICollection<Class> Classes { get; set; } = new List<Class>();

       public ICollection<Student> Students { get; set; } =new List<Student>();   


    }
}
