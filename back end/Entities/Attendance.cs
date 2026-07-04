namespace School_Project_API.Entities
{
    public class Attendance
    {
        public int Id { get; set; }

        public int? StudentId { get; set; }  

        public Student? Student { get; set; }   


        public DateTime? Date { get; set; }

        public AttendanceStatus Status { get; set; }    

        public string? Notes { get; set; }  

    }

    public enum AttendanceStatus
    {
        Present=1,
        Absent=2,
        Late =3,
        Excused =4
    }
}
