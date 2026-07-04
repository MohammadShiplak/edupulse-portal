using System.Diagnostics;

namespace School_Project_API.Entities
{
    public class ClassSubject
    {

        public int ClassId { get; set; }    

        public Class Class { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; } = null;

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;




    }
}
