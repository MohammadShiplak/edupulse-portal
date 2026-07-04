using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class TeacherService : ITeacherService
    {

        private readonly ApplicationDbContext _context;

        public TeacherService(ApplicationDbContext context)
        {
            _context = context;

        }

        private static TeacherDTO MapToDto(Teacher teacher)
        {

            return new TeacherDTO
            {
Id = teacher.Id,  
Name = teacher.Name,
Specialization = teacher.Specialization,
HireDate = teacher.HireDate,
             
            };


        }



        public async Task<TeacherDTO> AddTeachersAsync(TeacherDTO teacherDTO)
        {
            var NewTeacher = new Teacher
            {
       
                Name = teacherDTO.Name,
                Specialization = teacherDTO.Specialization,
                HireDate = teacherDTO.HireDate,
              
            };


            _context.Teacher.Add(NewTeacher);

            await _context.SaveChangesAsync();





            return MapToDto(NewTeacher);


        }

        public async Task<bool> DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);

            if (teacher == null)
                return false;

            _context.Teacher.Remove(teacher);

            await _context.SaveChangesAsync();

            return true;
        }

        public async  Task<PagedResponse<TeacherDTO>> GetAllTeachersAsync(int pageNumber,int pageSize)
        {

            var query = _context.Teacher.AsNoTracking();

            var totalRecords = await query.CountAsync();

            var teachers = await _context.Teacher

.OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)

        .Select(d => new TeacherDTO
        {
            Id = d.Id,
            Name=d.Name,
            Specialization=d.Specialization,
            HireDate=d.HireDate,

        })
        .ToListAsync();

            return new PagedResponse<TeacherDTO>
            {

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling
               (
                    totalRecords / (double)pageSize
                ),

                Data = teachers

            };
        }

        public async Task<TeacherDTO> GetTeacherByIdAsync(int id)
        {
            var teacherId = await _context.Teacher.FirstOrDefaultAsync(d => d.Id == id);

            if (teacherId == null)
                return null;

            return MapToDto(teacherId);
        }

        public async  Task<TeacherDTO> UpdateTeachersAsync(int id, TeacherDTO teacherDTO)
        {
            var teacher = await _context.Teacher.FindAsync(id);

            if (teacher == null)
                return null;

        
            teacher.Name = teacherDTO.Name; 
            teacher.Specialization = teacherDTO.Specialization; 
            teacher.HireDate = teacherDTO.HireDate; 



            // Save changes and get the updated student
            await _context.SaveChangesAsync();

            return MapToDto(teacher);
        }
    }
}
