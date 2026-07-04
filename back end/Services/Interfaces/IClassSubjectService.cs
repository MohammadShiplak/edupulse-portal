using School_Project_API.Entities;
using static School_Project_API.DTO.ClassSubjectDTO;

namespace School_Project_API.Services.Interfaces
{
    public interface IClassSubjectService
    {
     
        // Returns a list because a class can have many subjects
        Task<List<ClassSubjectReadDTO>> GetSubjectsByClassAsync(int classId);

        // Get all classes that a specific subject is assigned to
        // Useful for: "Which classes are teaching C#?"
        Task<List<ClassSubjectReadDTO>> GetClassesBySubjectAsync(int subjectId);

        // Assign a subject to a class
        // Returns the created record (with names, timestamps) — richer than what was sent
        // Throws InvalidOperationException if already assigned or if IDs don't exist
        Task<ClassSubjectReadDTO> AssignSubjectToClassAsync(ClassSubjectWriteDTO dto);

        // Remove a subject from a class
        // Returns false if the assignment doesn't exist (so controller can return 404)
        Task<bool> RemoveSubjectFromClassAsync(int classId, int subjectId);

        // Check if a subject is already assigned to a class
        // WHY expose this as a method?
        //   Useful for validation before assigning — prevents duplicates.
        //   Also useful in the controller to return a meaningful error message.
        Task<bool> IsSubjectAssignedToClassAsync(int classId, int subjectId);




    }
}
