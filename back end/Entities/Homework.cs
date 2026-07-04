namespace School_Project_API.Entities
{
    public class Homework
    {

        public int Id {  get; set; }

        public int TeacherId { get; set; }


        public Teacher Teacher { get; set; } = null!;   

        public int? ClassId { get; set; }

        public Class? Class { get; set; }   

        public int? SubjectId { get; set; } 

        public Subject? Subject { get; set; }

        public string Title { get; set; }   

        public string? Description { get; set; }
        public DateTime DueDate { get; set; }   

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;   

        public HomeworkStatus Status { get; set; } = HomeworkStatus.Active;   

        public string? FilePath { get; set; }   
    }
    public enum HomeworkStatus
    {
        Active =1,
        Archived =2,
     
    }   
}
