using School_Project_API.Entities;

namespace School_Project_API.DTO
{
    public class AttendanceAlertDTO
    {

       public int Id { get; set; }

        public int? StudentId { get; set; }

        public string? StudentName { get; set; }

        public int ConsecutiveAbsences { get; set; }

        public DateTime AlertDate { get; set; }

        public AlertStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Notes { get; set; }
    }
    public class ResolveAlertDTO
    {
        public AlertStatus Status { get; set; }

        public string? Notes { get; set; }
    }   
}
