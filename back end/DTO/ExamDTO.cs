namespace School_Project_API.DTO
{
    public class ExamDTO
    {

        public int Id { get; set; }

        public int? StudentId { get; set; } 

        public string? StudentName { get; set; }   

           public int? CourseId { get; set; }

        public string? CourseName { get; set; }

        public decimal Score { get; set; }

        public decimal MaxScore { get; set; }   

        public DateTime ExamDate { get; set; }  

        public string Notes { get; set; }

        public decimal ScorePercentage =>
            MaxScore > 0 ? Math.Round((Score / MaxScore) * 100, 1) : 0;


    }
}
