using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace School_Project_API.Entities
{
    
    public class Student
    {

     
        public int Id { get; set; }


        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }  

        public string Address { get; set; }   

        public string Phone {  get; set; }  

        public string Email { get; set; }

       // understand relationships one to one we add a navigation property to two tables and foreign key to any other 

        /*
         each one student has one accesscard

        */

        public int ?CardId { get; set; } 
        public AccessCard ?AccessCard { get; set; }  

        /*
         One to Many

        */
        

        public Department ?Department { get; set; }  
        public int ?DepartmentId { get; set; }   

        /*
         Many to Many 

        */

        public ICollection<Subject> Subjects { get; set; }=new List<Subject>(); 

        public ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();

     


    }
}
