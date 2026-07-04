using School_Project_API.Entities;
using System.ComponentModel.DataAnnotations;

namespace School_Project_API.DTO
{
    public class HomeworkDTO
    {
        public int Id { get; set; } 

        public string? TeacherName { get; set; }    

        public string? ClassName { get; set; }  

        public string? SubjectName { get; set; }


        [Required(ErrorMessage = "Teacher ID is required")]
        public int TeacherId { get; set; }

      
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

     
        public DateTime CreatedAt { get; set; }

 
        public HomeworkStatus Status { get; set; } = HomeworkStatus.Active;

        public string ?FilePath { get; set; } = string.Empty;    


        public string? FileName { get; set; }

    }
}
public class HomeworkCreateDTO
{
    [Required(ErrorMessage = "Teacher ID is required")]
    public int TeacherId { get; set; }

    public int? ClassId { get; set; }
    public int? SubjectId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Due date is required")]
    public DateTime DueDate { get; set; }

    public HomeworkStatus Status { get; set; } = HomeworkStatus.Active;

    public IFormFile? AssignmentFile { get; set; }



}
