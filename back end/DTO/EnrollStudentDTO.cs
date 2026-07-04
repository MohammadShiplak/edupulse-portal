using System.ComponentModel.DataAnnotations;

namespace School_Project_API.DTO
{
    public class EnrollStudentDTO
    {
        [Required(ErrorMessage = "Student ID is required")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Class ID is required")]
        public int ClassId { get; set; }
    }
}
