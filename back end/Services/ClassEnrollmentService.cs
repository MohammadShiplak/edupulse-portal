using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class ClassEnrollmentService : IClassEnrollmentService
    {

        private readonly ApplicationDbContext _context;


     public ClassEnrollmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        private static ClassEnrollmentDTO MapToDTO(
           StudentClass sc,
           int currentEnrollment)
        {
            return new ClassEnrollmentDTO
            {
                StudentId = sc.StudentId,
                // WHY null-conditional (.?):
                //   If Student navigation wasn't loaded (no .Include()),
                //   sc.Student would be null → NullReferenceException.
                //   The ?. operator returns null safely instead of crashing.
                StudentName = sc.Student != null
                    ? $"{sc.Student.FirstName} {sc.Student.LastName}"
                    : $"Student #{sc.StudentId}",

                ClassId = sc.ClassId,
                ClassName = sc.Class?.Name ?? $"Class #{sc.ClassId}",

                CurrentEnrollment = currentEnrollment,
                Capacity = sc.Class?.Capacity ?? 0,

                EnrolledAt = sc.EnrolledAt
            };
        }



        public async  Task<ClassEnrollmentDTO> EnrollStudentAsync(EnrollStudentDTO DTO)
        {
           var targetClass= await _context.Class.FirstOrDefaultAsync(c=>c.Id == DTO.ClassId);


            if (targetClass == null)
                throw new InvalidOperationException($"Class with Id {DTO.ClassId} was not found");

            var studentExists=await _context.Students.AnyAsync(s=>s.Id ==  DTO.StudentId);


            if (!studentExists)
                throw new InvalidOperationException(
                    $"Student with Id {DTO.StudentId} was not found.");


            var alreadyEnrolled = await _context.StudentClasses.AnyAsync(s => s.StudentId == DTO.StudentId
            && s.ClassId == DTO.ClassId);


            if (alreadyEnrolled)
                throw new InvalidOperationException(
                    $"Student {DTO.StudentId} is already enrolled in Class {DTO.ClassId}.");

            var currentEnrollment=await _context.StudentClasses
                .CountAsync(sc=>sc.ClassId == DTO.ClassId);

            if (currentEnrollment >= targetClass.Capacity)
                throw new InvalidOperationException(
                    $"Class '{targetClass.Name}' is full. " +
                    $"Capacity: {targetClass.Capacity}, " +
                    $"Currently enrolled: {currentEnrollment}.");


            var enrollment = new StudentClass
            {
                StudentId = DTO.StudentId,
                ClassId = DTO.ClassId,
                EnrolledAt = DateTime.UtcNow
            };


            _context.StudentClasses.Add(enrollment);


            await _context.SaveChangesAsync();


            await _context.Entry(enrollment)
                .Reference(sc=>sc.Student)
                .LoadAsync();

            await _context.Entry(enrollment)
            .Reference(sc => sc.Class)
            .LoadAsync();

            return MapToDTO(enrollment, currentEnrollment + 1);
        }

        public async  Task<List<ClassEnrollmentDTO>> GetClassesByStudentAsync(int studentId)
        {
            // 1. Validate student exists (optional but good practice)
            var studentExists = await _context.Students
                .AnyAsync(s => s.Id == studentId);

            if (!studentExists)
                throw new InvalidOperationException($"Student with Id {studentId} was not found.");

            var enrollments = await _context.StudentClasses
                .AsNoTracking()
                .Where(sc => sc.StudentId == studentId)
                .Select(sc=> new
                {

                    sc.StudentId,
                    sc.ClassId,
                    sc.EnrolledAt,
                   ClassName =sc.Class.Name,
                    StudentName = sc.Student.FirstName + " " + sc.Student.LastName
                }).ToListAsync();


            var classIds=enrollments.Select(e=>e.ClassId).Distinct().ToList();

            var classCounts = await _context.StudentClasses
                .Where(sc => classIds.Contains(sc.ClassId))
                .GroupBy(sc => sc.ClassId)
                .Select(g => new
                {

                    ClassId = g.Key,
                    count = g.Count()



                })
                .ToDictionaryAsync(x => x.ClassId, x => x.count);

            var result = enrollments
      .Select(sc => new ClassEnrollmentDTO
      {
          StudentId = sc.StudentId,
          ClassId = sc.ClassId,
          ClassName = sc.ClassName,
          StudentName = sc.StudentName,
          EnrolledAt = sc.EnrolledAt,
          CurrentEnrollment = classCounts.ContainsKey(sc.ClassId)
              ? classCounts[sc.ClassId]
              : 0
      })
      .OrderBy(x => x.ClassName)
      .ToList();


            return result;  





        }

        public async Task<List<ClassEnrollmentDTO>> GetStudentByClassAsync(int classId)
        {
            var classExists=await _context.Class
                .AnyAsync(c=>c.Id == classId);

            if (!classExists)
                throw new InvalidDataException($"Class with Id {classId} was not found");

          
                    var enrollments=await _context
                .StudentClasses.AsNoTracking()
                .Where(sc => sc.ClassId == classId)
                .Select(sc => new ClassEnrollmentDTO
                {

                    StudentId = sc.StudentId, 
                    ClassId=sc.ClassId,
                    StudentName=sc.Student.FirstName + " " + sc.Student.LastName,   
                    ClassName=sc.Class.Name,
                    EnrolledAt=sc.EnrolledAt

                })
                .OrderBy(e=>e.StudentName).ToListAsync();


            return enrollments;
                

        }

        public async Task<bool> UnenrollStudentAsync(int StudentId, int classID)
        {

            var enrollment = await _context.StudentClasses
                .FirstOrDefaultAsync(sc => sc.StudentId == StudentId &&
                sc.ClassId == classID);

            if (enrollment == null)
                return false;

            _context.StudentClasses.Remove(enrollment);

            await _context.SaveChangesAsync();


            return true;    

        }
    }
}
