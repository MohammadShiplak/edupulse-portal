namespace School_Project_API.DTO
{
    public class CourseProgressDTO
    {
        public int Id { get; set; }


    public int StudentId { get; set; }

        public  string? StudentName { get; set; }   

     public int CourseId { get; set; }  

        public string? CourseName { get; set; }   

          public decimal HomeworkScore { get; set; }
        
        public decimal ExamScore { get; set; }

        public decimal AttendanceScore { get; set; }    

        public decimal OverallProgress { get; set; }    
        public DateTime CalculatedAt { get; set; }

        public int TotalHomeworks { get; set; }
        public int TotalAttendanceDays { get; set; }    

           public int TotalExams { get; set; }

        public string GradeLabel => OverallProgress switch
        {
>= 90 => "A - Excellent",
>= 80 => "B - Good",
>= 70 => "C - Average",
>= 60 => "D - Bellow Average",
           _ => "A - Excellent"

        };


    }
}
