using School_Project_API.DTO;

namespace School_Project_API.Services.Interfaces
{
    public interface IClassEnrollmentService
    {

        Task<ClassEnrollmentDTO> EnrollStudentAsync(EnrollStudentDTO DTO);


            Task<bool> UnenrollStudentAsync(int StudentId, int classID);

        Task<List<ClassEnrollmentDTO>> GetStudentByClassAsync(int classId);

        Task<List<ClassEnrollmentDTO>> GetClassesByStudentAsync(int studentId);


    }
}
