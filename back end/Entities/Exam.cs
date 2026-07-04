namespace School_Project_API.Entities
{
    public class Exam
    {

        public int Id { get; set; } 

        public int? StudentId { get; set; }

        public Student? Student { get; set; }

        public int? CourseId { get; set; }  

public Course? Course { get; set; }

        public decimal Score { get; set; }  

        public decimal MaxScore { get; set; }

        public DateTime ExamDate { get; set; }

        public string? Notes { get;set; }








    }
}
