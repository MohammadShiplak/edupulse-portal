using System.ComponentModel.DataAnnotations;

namespace School_Project_API.DTO
{
    public class ClassSubjectDTO
    {
        public class ClassSubjectWriteDTO
        {
            // [Required] = if this field is missing from the JSON body,
            //   ASP.NET Core returns 400 Bad Request automatically.
            //   No need to write manual null-checks in the controller.
            [Required(ErrorMessage = "Class ID is required")]
            public int ClassId { get; set; }

            [Required(ErrorMessage = "Subject ID is required")]
            public int SubjectId { get; set; }
        }


        public class ClassSubjectReadDTO
        {
            // Primary key pair — so the client knows what was created
            public int ClassId { get; set; }
            public int SubjectId { get; set; }

            // Human-readable names — client shows these in the UI, not raw IDs
            public string ClassName { get; set; } = string.Empty;
            public string SubjectName { get; set; } = string.Empty;

            // When was this subject assigned to this class?
            public DateTime AssignedAt { get; set; }
        }










    }
}
