using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class StudentService : IStudentService
    {

        private readonly ApplicationDbContext _context;


        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ─── PRIVATE HELPER ──────────────────────────────────────────────────────────
        // DRY principle: "Don't Repeat Yourself"
        // Instead of writing the same mapping code in every method, we write it once here.
        private static StudentDTO MapToDto(Student student)
        {

            return new StudentDTO
            {

Id=student.Id,
FirstName=student.FirstName,    
LastName=student.LastName,
DateOfBirth=student.DateOfBirth,    
Gender=student.Gender,  
Address=student.Address,    
Email=student.Email,
Phone=student.Phone,
            };


        }
        public async  Task<StudentDTO> AddStudentAsync(StudentDTO studentDTO)
        {

        


            var NewStudent = new Student
            {

                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                DateOfBirth = studentDTO.DateOfBirth,
                Gender = studentDTO.Gender,
                Address = studentDTO.Address,
                Email = studentDTO.Email,
                Phone = studentDTO.Phone,
            };


            _context.Students.Add(NewStudent);

            await _context.SaveChangesAsync();

          



            return MapToDto(NewStudent);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return false;

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();

            return true;    
        }

        public async  Task<PagedResponse<StudentDTO>> GetAllStudentsAsync(int pageNumber,int pageSize)
        {

            var query = _context.Students.AsNoTracking();


            var totalRecords = await query.CountAsync();


            var students = await _context.Students
               
.OrderBy(s=>s.Id)
            .Skip((pageNumber-1)*pageSize)  
            .Take(pageSize)
      
        .Select(d => new StudentDTO
        {
            Id = d.Id,
            FirstName = d.FirstName,
            LastName = d.LastName,
            DateOfBirth = d.DateOfBirth,
            Gender = d.Gender,
            Address = d.Address,
            Email = d.Email,
            Phone = d.Phone,

        })
        .ToListAsync();

            return new PagedResponse<StudentDTO>
            {

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling
               (
                    totalRecords / (double)pageSize
                ),

                Data=students

            };

        }

        public async Task<StudentDTO> GetStudentByIdAsync(int id)
        {
   

            var Student = await _context.Students.
                FirstOrDefaultAsync(s => s.Id == id);



            if (Student == null)
                return null;
      

            return MapToDto(Student);   

        }

        public async Task<StudentDTO> UpdateStudentAsync(int id, StudentDTO studentDTO)
        {
           

     
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return null;


            student.FirstName = studentDTO.FirstName;
            student.LastName = studentDTO.LastName;
            student.DateOfBirth = studentDTO.DateOfBirth;
            student.Gender = studentDTO.Gender;
            student.Address = studentDTO.Address;
            student.Email = studentDTO.Email;
            student.Phone = studentDTO.Phone;




            // Save changes and get the updated student
            await _context.SaveChangesAsync();
            
         return MapToDto(student);  
        }
    }
}
