using Microsoft.Identity.Client;

namespace School_Project_API.Entities
{
    public class Course
    {

       public int Id { get; set; }  

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImagePath { get; set; }


        public ICollection<Subject> Subject { get; set; }=new List<Subject>();  


    }
}
