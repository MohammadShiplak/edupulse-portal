namespace School_Project_API.Entities
{
    public class AttendanceAlert
    {

        public int Id { get; set; }

        public int? StudentId { get; set; }  

        public Student? Student { get; set; }


        public int ConsecutiveAbsences { get; set; }    


        public DateTime AlertDate { get; set; } 

        public DateTime  CreatedAt { get; set; } = DateTime.UtcNow;

        public AlertStatus Status { get; set; } = AlertStatus.Active;

        public string ?Notes { get; set; }  

    }


    public enum AlertStatus
    {
        Active=1,
        Resolved = 2,
        Dismissed = 3
    }




}
