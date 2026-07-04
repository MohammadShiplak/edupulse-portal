using School_Project_API.Entities;

namespace School_Project_API.DTO
{
    public class AttendanceDTO
    {
        public int Id { get; set; } 

        public int ?StudentId { get; set; }  

        public string? StudentName { get; set; }    

        public DateTime ?Date { get; set; }  

        public AttendanceStatus Status { get; set; }

        public string Notes { get; set; }   
    }
}
