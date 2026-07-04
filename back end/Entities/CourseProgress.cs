namespace School_Project_API.Entities
{
    public class CourseProgress
    {
        public int Id { get; set; } 

        public int StudentId { get; set; }  

        public Student Student { get; set; }    


        public int CourseId { get; set; }   

        public Course Course { get; set; }  


        public decimal HomeworkScore  { get; set; }   

        public decimal AttendanceScore { get; set; }    

        public decimal ExamScore { get; set; }  


        public decimal OverallProgress { get; set; }

        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;


        public int TotalHomeworks { get; set; }
        public int TotalAttendanceDays { get; set; }

        public int TotalExams { get; set; } 




      
    }
}
